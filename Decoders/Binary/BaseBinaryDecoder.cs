using System;
using System.IO;
using SCUMMRevLib.Chunks;

namespace SCUMMRevLib.Decoders.Binary
{
    public abstract class BaseBinaryDecoder : BaseDecoder
    {
        public override DecoderFormat Format
        {
            get { return DecoderFormat.Binary; }
        }

        public abstract string GetFileExtension(Chunk chunk);

        public abstract void Decode(Chunk chunk, Stream destination);
    }
}
