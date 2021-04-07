using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCUMMRevLib.Chunks
{
    public class ChunkSelectException : Exception
    {
        public ChunkSelectException() : base()
        {
            
        }

        public ChunkSelectException(string message) : base(message)
        {
            
        }

        public ChunkSelectException(string message, params object[] args) : base(String.Format(message, args))
        {
            
        }
    }
}
