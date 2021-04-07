using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCUMMRevLib.Decoders
{
    public class DecodingException : ScummRevisitedException
    {
        public DecodingException() : base()
        {
            
        }

        public DecodingException(string message) : base(message)
        {
            
        }

        public DecodingException(string message, params object[] args) : base(message, args)
        {
            
        }
    }
}
