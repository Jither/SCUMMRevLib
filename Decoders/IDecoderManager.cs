using System.Collections.Generic;
using SCUMMRevLib.Chunks;
using SCUMMRevLib.Viewers;

namespace SCUMMRevLib.Decoders
{
    public interface IDecoderManager
    {
        IList<ActionParameters> GetActions(Chunk chunk);
        IList<BaseDecoder> GetDecoders(string chunkId);
    }
}