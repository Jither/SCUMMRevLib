using System;
using SCUMMRevLib.Chunks;

namespace SCUMMRevLib.Decoders.Palettes
{
    public abstract class BasePaletteDecoder : BaseDecoder
    {
        public override DecoderFormat Format
        {
            get { return DecoderFormat.Palette; }
        }

        public override string  GetOutputDescription(Chunk chunk)
        {
 	        return "Palette";
        }

        public abstract Palette Decode(Chunk chunk);
    }
}
