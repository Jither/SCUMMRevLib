using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using Katana.Types;
using SCUMMRevLib.Utils;
using SCUMMRevLib.Chunks;

namespace SCUMMRevLib.FileFormats
{
    public class KAPLFile : SRFile
    {
        public KAPLFile(string path, Stream stream) : base(path, stream)
        {
        }

        public override ChunkList GetRootChunks()
        {
            Position = 0;
            ChunkList result = new ChunkList();
            result.Add(new KAPLChunk(this, RootChunk));
            return result;
        }
    }
}
