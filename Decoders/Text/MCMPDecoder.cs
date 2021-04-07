using System;
using System.Text;
using Katana.IO;
using SCUMMRevLib.Chunks;

namespace SCUMMRevLib.Decoders.Text
{
    [DecodesChunks("MCMP")]
    public class MCMPDecoder : BaseTextDecoder
    {
        public override string GetOutputDescription(Chunk chunk)
        {
            return "Compression Map";
        }

        public override string Decode(Chunk chunk)
        {
            BinReader reader = chunk.GetReader();
            reader.Position = 4;
            uint entryCount = reader.ReadU16BE();

            // Read codec FourCC's:
            reader.Position = 6 + entryCount*9;
            // 5 bytes per FourCC - including zero-terminator:
            int codecCount = reader.ReadU16BE()/5;

            string[] codecs = new string[codecCount];

            for (int index = 0; index < codecCount; index++)
            {
                codecs[index] = reader.ReadStringZ();
            }

            StringBuilder builder = new StringBuilder();

            builder.AppendLine("MCMP Compression Map:");

            reader.Position = 6;

            for (int entry = 0; entry < entryCount; entry++)
            {
                uint codecIndex = reader.ReadU8();
                uint sizeDecompressed = reader.ReadU32BE();
                uint sizeCompressed = reader.ReadU32BE();

                builder.AppendFormat("Codec: {0}, Decompressed Size: {1,10} (0x{1:x8}), Compressed Size: {2,10} (0x{2:x8}){3}", codecs[codecIndex], sizeDecompressed, sizeCompressed, Environment.NewLine);
            }

            return builder.ToString();
        }

        public override bool CanDecode(Chunk chunk)
        {
            return true;
        }
    }
}
