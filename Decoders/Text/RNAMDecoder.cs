using System;
using System.Text;
using Katana.IO;
using SCUMMRevLib.Chunks;

namespace SCUMMRevLib.Decoders.Text
{
    [DecodesChunks("RNAM")]
    public class RNAMDecoder : BaseTextDecoder
    {
        public override string GetOutputDescription(Chunk chunk)
        {
            return "Room Names";
        }

        public override string Decode(Chunk chunk)
        {
            BinReader reader = chunk.GetReader();
            reader.Position = 8;

            StringBuilder builder = new StringBuilder();

            builder.AppendLine("ROOM Names:");
            while (true)
            {
                byte roomNumber = reader.ReadU8();
                if (roomNumber == 0) break;

                string name = reader.ReadString(9, 0xff);
                builder.AppendFormat("Room {0,3}: {1}{2}", roomNumber, name, Environment.NewLine);
            }

            return builder.ToString();
        }

        public override bool CanDecode(Chunk chunk)
        {
            return true;
        }
    }
}
