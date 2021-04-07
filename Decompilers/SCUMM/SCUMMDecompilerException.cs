using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCUMMRevLib.Decompilers.SCUMM
{
    public class SCUMMDecompilerException : Exception
    {
        public SCUMMDecompilerException(string message, params object[] args) : base(String.Format(message, args))
        {
            
        }
    }
}
