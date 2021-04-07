using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Katana.IO;
using SCUMMRevLib.Decoders;
using SCUMMRevLib.Decoders.Palettes;
using SCUMMRevLib.Utils;

namespace SCUMMRevLib.Viewers
{
    public class PaletteToAcoSaver : Saver<BasePaletteDecoder>
    {
        private static List<FileTypeInfo> filetypes = new List<FileTypeInfo>
                                                                 {
                                                                     new FileTypeInfo("Adobe Photoshop Swatch", "*.aco")
                                                                 };
        public override DecoderFormat DecoderFormat
        {
            get { return DecoderFormat.Palette; }
        }

        public override string ActionFormat
        {
            get { return "Save {0} as Photoshop Swatches..."; }
        }

        public override List<FileTypeInfo> GetFileTypes(BasePaletteDecoder decoder, Chunks.Chunk chunk)
        {
            return filetypes;
        }

        public override void Execute(BasePaletteDecoder decoder, Chunks.Chunk chunk, string filename)
        {
            if (String.IsNullOrEmpty(filename)) return;

            Palette pal = decoder.Decode(chunk);
            
            Save(filename, pal);
        }

        private void Save(string filename, Palette pal)
        {
            using (BinWriter writer = new BinWriter(filename))
            {
                writer.WriteU16BE(1); // Version 1
                writer.WriteU16BE(256); // 256 colors
                for (int i = 0; i < 256; i++)
                {
                    PaletteColor col = pal[i];
                    writer.WriteU16BE(0); // RGB
                    writer.WriteU16BE((ushort)(col.R * 256)); // w: Red
                    writer.WriteU16BE((ushort)(col.G * 256)); // x: Green
                    writer.WriteU16BE((ushort)(col.B * 256));  // y: Blue
                    writer.WriteU16BE(0); // z: Unused
                }

                writer.WriteU16BE(2); // Version 2
                writer.WriteU16BE(256); // 256 colors
                for (int i = 0; i < 256; i++)
                {
                    PaletteColor col = pal[i];

                    writer.WriteU16BE(0); // RGB
                    writer.WriteU16BE((ushort)(col.R * 256)); // w: Red
                    writer.WriteU16BE((ushort)(col.G * 256)); // x: Green
                    writer.WriteU16BE((ushort)(col.B * 256));  // y: Blue
                    writer.WriteU16BE(0); // z: Unused

                    string name = "Color " + i;
                    byte[] buffer = Encoding.BigEndianUnicode.GetBytes(name); // Photoshop wants UTF-16, but since these characters are all ASCII, it *is* UTF-16.
                    writer.WriteU16BE(0);
                    writer.WriteU16BE((ushort)(name.Length + 1));
                    writer.Write((uint)buffer.Length, buffer);
                    writer.WriteU16BE(0);
                }
            }
        }
    }
}
