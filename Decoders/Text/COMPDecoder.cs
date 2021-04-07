using System;
using System.Text;
using SCUMMRevLib.Chunks;
using Katana.IO;

namespace SCUMMRevLib.Decoders.Text
{
    [DecodesChunks("COMP")]
    public class COMPDecoder : BaseTextDecoder
    {
        public override string GetOutputDescription(Chunk chunk)
        {
            return "Compression Map";
        }

        public override string Decode(Chunk chunk)
        {
            BinReader reader = chunk.GetReader();
            reader.Position = 4;
            uint entryCount = reader.ReadU32BE();

            StringBuilder builder = new StringBuilder();

            reader.Position = 12;
            uint lastOutputSize = reader.ReadU32BE();

            builder.AppendLine("COMP Compression Map:");
            builder.AppendFormat("Decompressed size of last block: {0}{1}", lastOutputSize, Environment.NewLine);

            for (int entry = 0; entry < entryCount; entry++)
            {
                uint offset = reader.ReadU32BE();
                uint size = reader.ReadU32BE();
                uint codecId = reader.ReadU32BE();
                uint reserved = reader.ReadU32BE();

                builder.AppendFormat("Offset {0,10} (0x{0:x8}): Compressed Size: {1,10} (0x{1:x8}), Codec: {2,10}, Reserved: {3,10}{4}", offset, size, codecId, reserved, Environment.NewLine);
            }

            return builder.ToString();
        }

        public override bool CanDecode(Chunk chunk)
        {
            return true;
        }
    }
}
