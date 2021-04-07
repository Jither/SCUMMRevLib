using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCUMMRevLib.SpecScript
{
    public class ScannerException : Exception
    {
        public ScannerException(): base()
        {
            
        }

        public ScannerException(string message, params object[] args) : base(String.Format(message, args))
        {
            
        }
    }
}
