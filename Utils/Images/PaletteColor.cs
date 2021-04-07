using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCUMMRevLib.Utils
{
    public struct PaletteColor
    {
        public static PaletteColor Empty = new PaletteColor(0, 0, 0);

        public byte R;
        public byte G;
        public byte B;

        public PaletteColor(byte r, byte g, byte b)
        {
            R = r;
            G = g;
            B = b;
        }
    }
}
