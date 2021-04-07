using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SCUMMRevLib.Chunks;

namespace SCUMMRevLib.Decoders.Boxes
{
    public abstract class BaseBoxDecoder : BaseDecoder
    {
        public override DecoderFormat Format
        {
            get { return DecoderFormat.Boxes; }
        }

        public override string  GetOutputDescription(Chunk chunk)
        {
 	        return "Boxes";
        }

        public abstract uint GetCount(Chunk chunk);
        public abstract List<ScummBox> Decode(Chunk chunk);
    }
}
