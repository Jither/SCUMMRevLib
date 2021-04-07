using System;
using Katana.IO;
using SCUMMRevLib.Chunks;
using SCUMMRevLib.Utils;

namespace SCUMMRevLib.Decoders.Palettes
{
    [DecodesChunks("XPAL")]
    public class XPALPaletteDecoder : StandardPaletteDecoder
    {
        public override string GetOutputDescription(Chunk chunk)
        {
            return "Initial Palette";
        }

        public override Palette Decode(Chunk chunk)
        {
            BinReader reader = chunk.GetReader();
            reader.Position = 1548;
            return ReadPalette(reader);
        }

        public override bool CanDecode(Chunk chunk)
        {
            return chunk.Size == 2316; // header + two unknown words + 768 delta values (word) + 256 colors
        }
    }
}
