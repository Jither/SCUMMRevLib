using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SCUMMRevLib.Chunks;

namespace SCUMMRevLib.Decoders.Video
{
    public abstract class BaseVideoDecoder : BaseDecoder
    {
        public override DecoderFormat Format
        {
            get { return DecoderFormat.Video; }
        }

        public override string GetOutputDescription(Chunk chunk)
        {
            return "Video";
        }

        public abstract VideoInfo GetInfo(Chunk chunk);
        public abstract void Initialize(Chunk chunk);
        public abstract void DecodeFrame(byte[] buffer);
    }
}
