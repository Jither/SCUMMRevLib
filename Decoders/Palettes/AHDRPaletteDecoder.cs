using System;
using Katana.IO;
using SCUMMRevLib.Chunks;
using SCUMMRevLib.Utils;

namespace SCUMMRevLib.Decoders.Palettes
{
    [DecodesChunks("AHDR")]
    public class AHDRPaletteDecoder : StandardPaletteDecoder
    {
        public override Palette Decode(Chunk chunk)
        {
            BinReader reader = chunk.GetReader();
            reader.Position = 14;
            return ReadPalette(reader);
        }

        public override bool CanDecode(Chunk chunk)
        {
            return chunk.Size == 802; // header and info + 256 colors + some more info
        }
    }
}
