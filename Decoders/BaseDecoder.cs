using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SCUMMRevLib.Chunks;

namespace SCUMMRevLib.Decoders
{
    public abstract class BaseDecoder
    {
        public abstract DecoderFormat Format { get; }
        public abstract bool CanDecode(Chunk chunk);
        public abstract string GetOutputDescription(Chunk chunk);
    }
}
