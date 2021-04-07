using System;
using SCUMMRevLib.Chunks;
using SCUMMRevLib.Encryption;

namespace SCUMMRevLib.FileFormats
{
    public class SCUMM3File : SRXORFile
    {
        public int FileVersion { get; set; }

        public SCUMM3File(string path, XorStream stream) : base(path, stream)
        {
        }

        public override ChunkList GetRootChunks()
        {
            ChunkList result = new ChunkList();
            Position = 0;

            while (Position < Size)
            {
                Chunk chunk = SCUMM3Chunk.ReadChunk(this, RootChunk);
                result.Add(chunk);
                Position = chunk.Offset + chunk.Size;
            }

            return result;
        }
    }
}
