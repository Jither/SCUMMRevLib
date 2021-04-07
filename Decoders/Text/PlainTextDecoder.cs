using System;
using System.Text;
using Katana.IO;
using SCUMMRevLib.Chunks;

namespace SCUMMRevLib.Decoders.Text
{
    [DecodesChunks(".3do", ".anim", ".cos", ".set")]
    public class PlainTextDecoder : BaseTextDecoder
    {
        public override string Decode(Chunk chunk)
        {
            BinReader reader = chunk.GetReader();
            byte[] buffer;
            reader.Position = 0;
            reader.Read(chunk.Size, out buffer);

            return Encoding.ASCII.GetString(buffer);
        }

        public override bool CanDecode(Chunk chunk)
        {
            // Avoid binary coded .3DOs
            if (chunk.ChunkTypeId == ".3do")
            {
                BinReader reader = chunk.GetReader();
                reader.Position = 0;
                if (reader.ReadFourCC() == "LDOM")
                {
                    return false;
                }
            }
            return true;
        }
    }
}
