using System;
using Katana.IO;
using SCUMMRevLib.Chunks;
using SCUMMRevLib.Utils;

namespace SCUMMRevLib.Decoders.Palettes
{
    [DecodesChunks("PA")]
    public class PADecoder : StandardPaletteDecoder
    {
        public override Palette Decode(Chunk chunk)
        {
            BinReader reader = chunk.GetReader();
            reader.Position = 6;
            int count = reader.ReadU16LE();
            // Ignore count for now - sometimes (Zak, Last Crusade) it's number of colours, sometimes (MI1) number of RGB values
            return ReadPalette(reader);
        }
    }
}
