using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCUMMRevLib.SpecScript
{
    public class InterpreterException : Exception
    {
        public InterpreterException() : base()
        {
            
        }

        public InterpreterException(string message, params object[] args) : base(String.Format(message, args))
        {
            
        }
    }
}
