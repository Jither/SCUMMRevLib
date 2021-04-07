using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCUMMRevLib
{
    public class ScummRevisitedException : Exception
    {
        public ScummRevisitedException()
            : base()
        {

        }

        public ScummRevisitedException(string message)
            : base(message)
        {
            
        }

        public ScummRevisitedException(string message, params object[] args) : base(String.Format(message, args))
        {
            
        }
    }
}
