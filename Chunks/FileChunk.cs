using System;
using System.IO;
using SCUMMRevLib.FileFormats;

namespace SCUMMRevLib.Chunks
{
    public class FileChunk : Chunk
    {
        private readonly FileChunkSpec spec;

        public FileChunk(SRFile file, Chunk parent, string name, ulong offset, uint size)
            : base(file, parent)
        {
            spec = FileChunkSpecs.GetSpec(Path.GetExtension(name).ToLowerInvariant());
            Name = name;
            Offset = offset;
            Size = size;
        }

        public override string ChunkTypeId
        {
            get { return spec.Extension; }
        }

        public override string Description
        {
            get { return spec.Description; }
        }

        public override bool HasChildren
        {
            get { return true; }
        }

        protected override ChunkList InternalGetChildren()
        {
            ChunkList result = new ChunkList();
            ulong offset = Offset;
            while (offset < Offset + Size)
            {
                file.Position = offset;
                Chunk chunk = SCUMM5Chunk.ReadChunk(file, this);
                result.Add(chunk);
                offset += chunk.Size;
            }
            return result;
        }

        public override ImageIndex ImageIndex
        {
            get { return spec.ImageIndex; }
        }
    }
}
