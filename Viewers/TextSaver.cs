using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using SCUMMRevLib.Chunks;
using SCUMMRevLib.Decoders;
using SCUMMRevLib.Decoders.Text;
using Decoder=SCUMMRevLib.Decoders.BaseDecoder;
using System.IO;

namespace SCUMMRevLib.Viewers
{
    public class TextSaver : Saver<BaseTextDecoder>
    {
        public override DecoderFormat DecoderFormat
        {
            get { return DecoderFormat.Text; }
        }

        public override string ActionFormat
        {
            get { return "Save {0} as Text"; }
        }

        public override void Execute(BaseTextDecoder decoder, Chunk chunk, string filename)
        {
            if (String.IsNullOrEmpty(filename)) return;

            string text = decoder.Decode(chunk);

            Save(filename, text);
        }

        private void Save(string filename, string text)
        {
            File.WriteAllText(filename, text, Encoding.UTF8);
        }
    }
}
