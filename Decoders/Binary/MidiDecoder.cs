using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using SCUMMRevLib.Chunks;

namespace SCUMMRevLib.Decoders.Binary
{
    internal class MidiInfo
    {
        public ulong MThdOffset { get; set; }
        public ulong MDpgOffset { get; set; }
        public ushort Format { get; set; }

        public MidiInfo()
        {
            
        }
    }

    [DecodesChunks("ROL ", "ADL ", "GMD ", "SPK ", "AMI ")]
    public class MidiDecoder : BaseBinaryDecoder
    {
        public override string GetFileExtension(Chunk chunk)
        {
            return "*.mid";
        }

        public override void Decode(Chunk chunk, Stream destination)
        {
            var info = ReadHeader(chunk);
            using (var source = chunk.GetStream())
            {
                source.Position = (long)info.MThdOffset;
                source.CopyTo(destination);
            }
        }

        private MidiInfo ReadHeader(Chunk chunk)
        {
            MidiInfo info = new MidiInfo();
            using (var reader = chunk.GetReader())
            {
                reader.Position = 8;
                var fourCC = reader.ReadFourCC();
                if (fourCC != "MDhd")
                {
                    return null;
                }
                var tagsize = reader.ReadU32BE();
                reader.Position += tagsize;
                
                fourCC = reader.ReadFourCC();

                if (fourCC == "MDpg")
                {
                    info.MDpgOffset = reader.Position - 4;
                    tagsize = reader.ReadU32BE();
                    reader.Position += tagsize;
                    fourCC = reader.ReadFourCC();
                }

                if (fourCC != "MThd")
                {
                    return null;
                }

                info.MThdOffset = reader.Position - 4;

                tagsize = reader.ReadU32BE();
                info.Format = reader.ReadU16BE();
            }
            return info;
        }

        public override bool CanDecode(Chunk chunk)
        {
            return ReadHeader(chunk) != null;
        }

        public override string GetOutputDescription(Chunk chunk)
        {
            var info = ReadHeader(chunk);

            return String.Format("Standard MIDI file (format {0})", info.Format);
        }
    }
}
