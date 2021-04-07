using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Katana.Types;
using SCUMMRevLib.Utils;
using SCUMMRevLib.Chunks;
using SCUMMRevLib.Encryption;

namespace SCUMMRevLib.FileFormats
{
    public class SCUMM5File : SRXORFile
    {
        public int FileVersion { get; set; }

        public SCUMM5File(string path, XorStream stream) : base(path, stream)
        {
        }

        public override ChunkList GetRootChunks()
        {
            ChunkList result = new ChunkList();
            Position = 0;

            while (Position < Size)
            {
                Chunk chunk = SCUMM5Chunk.ReadChunk(this, RootChunk);
                result.Add(chunk);
                Position = chunk.Offset + chunk.Size;
            }

            return result;
        }
    }
}
