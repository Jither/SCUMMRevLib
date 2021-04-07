using System;
using System.Text;
using Katana.IO;
using SCUMMRevLib.Chunks;

namespace SCUMMRevLib.Decoders.Text
{
    [DecodesChunks("LOFF")]
    public class LOFFDecoder : BaseTextDecoder
    {
        public override string GetOutputDescription(Chunk chunk)
        {
            return "Room Offsets";
        }

        public override string Decode(Chunk chunk)
        {
            BinReader reader = chunk.GetReader();
            reader.Position = 8;
            uint roomCount = reader.ReadU8();

            StringBuilder builder = new StringBuilder();

            builder.AppendLine("ROOM Offsets:");
            for (int room = 0; room < roomCount; room++)
            {
                byte roomNumber = reader.ReadU8();
                uint offset = reader.ReadU32LE();

                builder.AppendFormat("Room {0,3}: {1,10} (0x{1:x8}){2}", roomNumber, offset, Environment.NewLine);
            }

            return builder.ToString();
        }

        public override bool CanDecode(Chunk chunk)
        {
            return true;
        }
    }
}
