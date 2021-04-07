using System;
using SCUMMRevLib.Chunks;

namespace SCUMMRevLib.Decoders.Text
{
    public abstract class BaseTextDecoder : BaseDecoder
    {
        public override DecoderFormat Format
        {
            get { return DecoderFormat.Text; }
        }

        public virtual string DecodedSyntax
        {
            get { return Syntax.PLAIN_TEXT; }
        }

        public override string  GetOutputDescription(Chunk chunk)
        {
 	        return "Text";
        }

        public abstract string Decode(Chunk chunk);
    }
}
