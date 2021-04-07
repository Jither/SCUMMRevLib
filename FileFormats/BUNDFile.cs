using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Katana.Types;
using SCUMMRevLib.FileFormats;
using SCUMMRevLib.Chunks;

namespace SCUMMRevLib.FileFormats
{
    public class BUNDFile : SRFile
    {
        public BUNDFile(string path, Stream stream) : base(path, stream)
        {
        }

        public override ChunkList GetRootChunks()
        {
            ChunkList result = new ChunkList();
            Position = 0;

            while (Position < Size)
            {
                Chunk chunk = new BUNDChunk(this, RootChunk, "BUND", 0, Size - 8);
                result.Add(chunk);
                Position = chunk.Offset + chunk.Size;
            }
            return result;
        }

        /*private bool  CheckFormat()
        {
            foreach (byte enc in ENCRYPTION_VALUES)
            {
                Encryption = enc;

                Position = 0;
                FourCC fourCC = ReadFourCC();
                uint size = ReadU32BE();
                
                if (fourCC == "BUND" && size <= Size)
                {
                    return true;
                }
            }
            return false;

        }*/
    }
}
