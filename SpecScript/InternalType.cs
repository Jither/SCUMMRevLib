using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCUMMRevLib.SpecScript
{
    [Flags]
    public enum InternalType
    {
        Invalid = 0,
        Integer = 1,
        Float = 2,
        String = 4,
    }
}
