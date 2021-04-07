using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using SCUMMRevLib.Chunks;

namespace SCUMMRevLib.FileFormats
{
    public class ForgeFile : SRFile
    {
        public ForgeFile(string path, Stream stream) : base(path, stream)
        {
        }

        public override ChunkList GetRootChunks()
        {
            ChunkList result = new ChunkList();
            Position = 0;
            result.Add(new ForgeChunk(this, RootChunk));
            return result;
        }
    }
}
