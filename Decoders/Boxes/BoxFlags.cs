using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCUMMRevLib.Decoders.Boxes
{
    [Flags]
    public enum BoxFlags : uint
    {
        None = 0,
        FlipX = 0x08,
        FlipY = 0x10,
        IgnoreScalePlayerOnly = 0x20,
        Locked = 0x40,
        Invisible = 0x80
    }
}
