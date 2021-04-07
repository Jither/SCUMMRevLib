using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SCUMMRevLib.Decoders.Images;

namespace SCUMMRevLib.Decoders.Video
{
    public class VideoInfo
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public PixelDepth PixelFormat { get; set; }
        public long FrameCount { get; set; }
        public Decimal FrameRate { get; set; }
    }
}
