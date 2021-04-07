using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCUMMRevLib.SpecScript
{
    public class CompilerException : Exception
    {
        public CompilerException() : base()
        {
            
        }

        public CompilerException(string message, params object[] args) : base(String.Format(message, args))
        {
            
        }
    }
}
