using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SCUMMRevLib.FileFormats;

namespace SCUMMRevLib.Chunks
{
    // TODO: Get rid of dependency on Scumm5Chunk - although it's similar, it really isn't SCUMM5. Maybe an IFF base chunk for both?
    public class BUNDChunk : SCUMM5Chunk
    {
        public BUNDChunk(SRFile file, Chunk parent, string name, ulong offset, uint size) : base(file, parent, name, offset, size)
        {
        }

        protected override ChunkList InternalGetChildren()
        {
            ChunkList result = new ChunkList();
            file.Position = Offset + 8;

            ulong maxPosition = Offset + Size;

            while (file.Position < maxPosition)
            {
                Chunk chunk = ReadChunk(file, this);
                if (chunk.ChunkTypeId == "BNDT")
                {
                    chunk = new BNDTChunk(file, this, chunk.Offset, chunk.Size);
                }
                result.Add(chunk);
                file.Position = chunk.Offset + chunk.Size;
            }
            return result;
        }
    }
}
