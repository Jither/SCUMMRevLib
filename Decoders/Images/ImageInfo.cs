using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SCUMMRevLib.Decoders.Palettes;

namespace SCUMMRevLib.Decoders.Images
{
    public enum PixelDepth
    {
        Depth1,
        Depth2,
        Depth4,
        Depth8,
        Depth15,
        Depth16,
        Depth24,
        Depth32
    }

    public class ImageInfo
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public PixelDepth PixelFormat { get; set; }
        public Palette Palette { get; set; }
    }
}
