using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCUMMRevLib.FileFormats
{
    public class FileFormatException : Exception
    {
        public FileFormatException(string message, params object[] args) : base(String.Format(message, args))
        {
            
        }
    }
}
