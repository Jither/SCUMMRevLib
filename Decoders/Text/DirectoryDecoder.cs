using System;
using System.Text;
using Katana.IO;
using SCUMMRevLib.Chunks;

namespace SCUMMRevLib.Decoders.Text
{
    [DecodesChunks("DROO", "DSCR", "DSOU", "DCOS", "DCHR")]
    public class DirectoryDecoder : BaseTextDecoder
    {
        public override string GetOutputDescription(Chunk chunk)
        {
            return GetItemsName(chunk) + " Directory";
        }

        private string GetItemsName(Chunk chunk)
        {
            switch (chunk.ChunkTypeId)
            {
                case "DROO":
                    return "Room";
                case "DSCR":
                    return "Script";
                case "DSOU":
                    return "Sound";
                case "DCOS":
                    return "Costume";
                case "DCHR":
                    return "Charset";
                default:
                    throw new DecodingException("Unknown chunk type (this shouldn't happen)");
            }
        }

        public override string Decode(Chunk chunk)
        {
            BinReader reader = chunk.GetReader();
            reader.Position = 8;
            uint itemCount = reader.ReadU16LE();

            StringBuilder builder = new StringBuilder();

            string itemName = GetItemsName(chunk);

            // Rooms are referenced by disk, everything else by room:
            string containerName = (chunk.ChunkTypeId == "DROO") ? "Disk" : "Room";

            builder.AppendLine(itemName + " Directory:");

            byte[] containers = new byte[itemCount];
            uint[] offsets = new uint[itemCount];
            for (int item = 0; item < itemCount; item++)
            {
                containers[item] = reader.ReadU8();
            }
            for (int item = 0; item < itemCount; item++)
            {
                offsets[item] = reader.ReadU32LE();
            }

            for (int item = 0; item < itemCount; item++)
            {
                builder.AppendFormat("{0} {1,3}: {2} {3,3}, offset: {4,10} (0x{4:x8}){5}", itemName, item, containerName, containers[item], offsets[item], Environment.NewLine);
            }

            return builder.ToString();
        }

        public override bool CanDecode(Chunk chunk)
        {
            return true;
        }
    }
}
