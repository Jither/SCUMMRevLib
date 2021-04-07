using System;
using Katana.IO;
using SCUMMRevLib.Chunks;
using SCUMMRevLib.Utils;

namespace SCUMMRevLib.Decoders.Palettes
{
    [DecodesChunks("CLUT", "APAL", "NPAL", "RGBS")]
    public class StandardPaletteDecoder : BasePaletteDecoder
    {
        private byte[] buffer = new byte[768];
        public override Palette Decode(Chunk chunk)
        {
            BinReader reader = chunk.GetReader();
            reader.Position = 8;
            return ReadPalette(reader);
        }

        protected Palette ReadPalette(BinReader reader)
        {
            reader.Read(buffer, 0, 768);
            Palette pal = new Palette(256);
            int bufferPos = 0;
            for (int i = 0; i < 256; i++)
            {
                pal[i] = new PaletteColor(buffer[bufferPos], buffer[bufferPos + 1], buffer[bufferPos + 2]);
                bufferPos += 3;
            }
            return pal;
        }

        public override bool CanDecode(Chunk chunk)
        {
            // Must have 256 colors: 8 byte header + 3 * 256
            // TODO: RGBS may not have 256 colors
            return chunk.Size == 776;
        }
    }
}
