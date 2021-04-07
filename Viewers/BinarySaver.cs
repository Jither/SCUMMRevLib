using System;
using System.Collections.Generic;
using System.Linq;
using SCUMMRevLib.Decoders.Images;
using SCUMMRevLib.Decoders.Text;
using SCUMMRevLib.Utils;
using System.IO;
using SCUMMRevLib.Chunks;
using SCUMMRevLib.Decoders;
using SCUMMRevLib.Decoders.Binary;

namespace SCUMMRevLib.Viewers
{
    public class BinarySaver : Saver<BaseBinaryDecoder>
    {
        public override string ActionFormat
        {
            get { return "Save as {0}..."; }
        }

        public override DecoderFormat DecoderFormat
        {
            get { return DecoderFormat.Binary; }
        }

        public override List<FileTypeInfo> GetFileTypes(BaseBinaryDecoder decoder, Chunk chunk)
        {
            return new List<FileTypeInfo>
                       {
                           new FileTypeInfo(decoder.GetOutputDescription(chunk), decoder.GetFileExtension(chunk))
                       };
        }

        public override void Execute(BaseBinaryDecoder decoder, Chunk chunk, string filename)
        {
            if (String.IsNullOrEmpty(filename)) return;

            SaveBinary(filename, decoder, chunk);
        }

        private void SaveBinary(string filename, BaseBinaryDecoder decoder, Chunk chunk)
        {
            using (FileStream stream = new FileStream(filename, FileMode.Create, FileAccess.Write))
            {
                decoder.Decode(chunk, stream);
            }
        }
    }
}
