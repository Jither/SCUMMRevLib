using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCUMMRevLib.Decoders
{
    public class DecoderMap
    {
        private Dictionary<string, List<BaseDecoder>> map;
        
        public DecoderMap()
        {
            map = new Dictionary<string, List<BaseDecoder>>();
        }

        public IList<BaseDecoder> this[string chunkId]
        {
            get
            {
                if (!map.ContainsKey(chunkId))
                {
                    return null;
                }
                return map[chunkId].AsReadOnly();
            }
        }

        public void Add(string chunkId, BaseDecoder decoder)
        {
            List<BaseDecoder> decoders;
            if (map.ContainsKey(chunkId))
            {
                decoders = map[chunkId];
            }
            else
            {
                decoders = new List<BaseDecoder>();
                map.Add(chunkId, decoders);
            }

            decoders.Add(decoder);
        }
    }
}
