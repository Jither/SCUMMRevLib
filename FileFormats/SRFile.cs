using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Katana.IO;
using SCUMMRevLib.Utils;
using SCUMMRevLib.Chunks;

namespace SCUMMRevLib.FileFormats
{
    /// <summary>
    /// Base class for file handlers
    /// </summary>
    public abstract class SRFile : BinReader
    {
        public Chunk RootChunk { get; protected set; }

        public string Path { get; private set; }
        public string Name { get { return System.IO.Path.GetFileName(Path); } }

        protected SRFile(string path, Stream stream) : base(stream)
        {
            Path = path;
            RootChunk = new RootChunk(this);
        }

        public virtual ChunkStream GetChunkStream(Chunk chunk)
        {
            return new ChunkStream(stream, chunk.Offset, chunk.Size);
        }

        public BinReader GetChunkReader(Chunk chunk)
        {
            ChunkStream chunkStream = GetChunkStream(chunk);
            return new BinReader(chunkStream);
        }

        public abstract ChunkList GetRootChunks();
    }
}
