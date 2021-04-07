using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Katana.IO;
using SCUMMRevLib.Annotations;
using SCUMMRevLib.FileFormats;

namespace SCUMMRevLib.Chunks
{
    public abstract class Chunk
    {
        // TODO: Replace SRFile with more general reader - while keeping access to e.g. filename (requires abstraction/interface)
        protected SRFile file;
        protected ChunkList children; // Cache for GetChildren

        /// <summary>
        /// Name of the chunk
        /// </summary>
        public string Name { get; protected set; }

        /// <summary>
        /// The offset into the file where the chunk is located. Note that for e.g. compressed archives, this may be an offset into a "virtual" uncompressed archive.
        /// (As indicated by OffsetIsInternal)
        /// </summary>
        public ulong Offset { get; protected set; }

        /// <summary>
        /// Indicates whether offset is an actual file offset or only applicable internally (e.g. due to compression)
        /// </summary>
        public virtual bool OffsetIsInternal { get { return false; } }

        /// <summary>
        /// Size in bytes of the chunk. Note that in compressed archives, this may be the uncompressed size
        /// </summary>
        public uint Size { get; protected set; }


        public Chunk Parent { get; protected set; }

        public SRFile File { get { return file; } }
        
        public string Annotation
        {
            get
            {
                return Annotator.Current.GetAnnotation(this);
            }
        }

        public abstract string ChunkTypeId { get; }
        public abstract string Description { get; }
        public abstract bool HasChildren { get; }
        public abstract ImageIndex ImageIndex { get; }

        protected Chunk(SRFile file, Chunk parent)
        {
            this.file = file;
            Parent = parent;
        }

        protected abstract ChunkList InternalGetChildren();
        
        public ChunkList GetChildren()
        {
            return children ?? (children = InternalGetChildren());
        }

        /// <summary>
        /// Evaluates a ChunkPath (XPath-like expression).
        /// </summary>
        /// <param name="path">ChunkPath to evaluate</param>
        /// <returns>List of resulting chunks</returns>
        public ChunkList Select(string path)
        {
            IEnumerable<Chunk> result = new List<Chunk> {this};
            
            string[] steps = path.Split('/');

            uint stepIndex = 0;
            foreach (string step in steps)
            {
                switch (step)
                {
                    // Current context
                    case ".":
                        break;
                    case "":
                        if (stepIndex == 0)
                        {
                            result = new List<Chunk> {file.RootChunk};
                        }
                        else
                        {
                            throw new ChunkSelectException("descendant-or-self (//) not supported");
                        }
                        break;
                    // Select parent(s)
                    case "..":
                        result = result.Select(c => c.Parent);
                        break;
                    // Select all children
                    case "*":
                        result = result.SelectMany(c => c.GetChildren());
                        break;
                    // Select specific child by name
                    default:
                        string chunkType = step;
                        result = result.SelectMany(c => c.GetChildren());
                        result = result.Where(c => c.ChunkTypeId == chunkType);
                        break;
                }
                // Remove null chunks:
                result = result.Where(c => c != null);
                // Remove duplicate chunks:
                result = result.Distinct();

                stepIndex++;
            }
            return new ChunkList(result);
        }

        public Chunk SelectSingle(string path)
        {
            ChunkList list = Select(path);
            if (list.Count == 0) return null;
            return list[0];
        }

        public Chunk SelectSingleAfter(string path, uint offset)
        {
            ChunkList list = Select(path);
            foreach (Chunk chunk in list)
            {
                if (chunk.Offset >= offset)
                {
                    return chunk;
                }
            }
            return null;
        }

        public override string ToString()
        {
            return Name;
        }

        public ChunkStream GetStream()
        {
            return file.GetChunkStream(this);
        }

        public BinReader GetReader()
        {
            return file.GetChunkReader(this);
        }

        public void Save(Stream stream)
        {
            using (ChunkStream chunkStream = GetStream())
            {
                chunkStream.CopyTo(stream);
            }
        }

        public void Save(string path)
        {
            using (FileStream stream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                Save(stream);
            }
        }
    }
}
