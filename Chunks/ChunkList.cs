using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCUMMRevLib.Chunks
{
    public class ChunkList : List<Chunk>
    {
        public ChunkList() : base()
        {
            
        }

        public ChunkList(IEnumerable<Chunk> chunks) : base(chunks)
        {
            
        }
    }
}
