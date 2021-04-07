using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using Katana.IO;
using Katana.Types;
using SCUMMRevLib.Chunks;
using SCUMMRevLib.Encryption;

namespace SCUMMRevLib.FileFormats
{
    public class TTArch2File : SRFile
    {
        public TTArch2File(string path, Stream stream) : base(path, stream)
        {
        }

        public override ChunkList GetRootChunks()
        {
            Position = 0;
            ChunkList result = new ChunkList();
            result.Add(new TTARCH2Chunk(this, RootChunk));
            return result;
        }
    }
}
