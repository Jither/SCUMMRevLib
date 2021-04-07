using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using SCUMMRevLib.Chunks;

namespace SCUMMRevLib.FileFormats
{
    public class UnknownFile : SRFile
    {
        public UnknownFile(string path, Stream stream) : base(path, stream)
        {
        }

        public override ChunkList GetRootChunks()
        {
            return new ChunkList {new UnknownChunk(this, null, 0, this.Size)};
        }
    }
}
