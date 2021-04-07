using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCUMMRevLib.Utils.Images
{
    /// <summary>
    /// Simple Color value type with no dependency on System.Drawing
    /// </summary>
    public struct Color
    {
        private int value;

        public byte R { get { return (byte) (value >> 16); } }
        public byte G { get { return (byte) (value >> 8); } }
        public byte B { get { return (byte) (value); } }
        public byte A { get { return (byte) (value >> 24); } }

        public static Color FromArgb(int argb)
        {
            return new Color { value = argb };
        }

        public static Color FromArgb(int a, int r, int g, int b)
        {
            return new Color { value = ((byte)a) << 24 | ((byte)r) << 16 | ((byte)g) << 8 | ((byte)b) };
        }

        public static Color FromArgb(int r, int g, int b)
        {
            return FromArgb(255, r, g, b);
        }

        public int ToArgb()
        {
            return value;
        }
    }
}
