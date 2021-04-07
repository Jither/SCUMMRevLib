using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using SCUMMRevLib.Chunks;
using SCUMMRevLib.Viewers;

namespace SCUMMRevLib.Decoders
{
    public class DecoderManager : IDecoderManager
    {
        private readonly DecoderMap chunkDecoders;

        private void RegisterDecoder(string chunkId, BaseDecoder decoder)
        {
            chunkDecoders.Add(chunkId, decoder);
        }

        public IList<ActionParameters> GetActions(Chunk chunk)
        {
            List <ActionParameters> result = new List<ActionParameters>();
            IList<BaseDecoder> decoders = GetDecoders(chunk.ChunkTypeId);
            if (decoders == null) return result;
            foreach (BaseDecoder decoder in decoders)
            {
                if (!decoder.CanDecode(chunk)) continue;
                string output = decoder.GetOutputDescription(chunk);
                IList<IViewer> viewers = ViewerManager.Current.GetViewers(decoder.Format);
                
                if (viewers == null) continue;

                foreach (IViewer viewer in viewers)
                {
                    ActionParameters prms = new ActionParameters(chunk, decoder, viewer);
                    result.Add(prms);
                }
            }
            return result;
        }

        public IList<BaseDecoder> GetDecoders(string chunkId)
        {
            return chunkDecoders[chunkId];
        }

        public DecoderManager()
        {
            chunkDecoders = new DecoderMap();

            // TODO: Move into client
            RegisterDecoders();
        }

        private void RegisterDecoders()
        {
            Type[] types = Assembly.GetExecutingAssembly().GetTypes();
            foreach (Type type in types)
            {
                if (type.IsSubclassOf(typeof(BaseDecoder)) && !type.IsAbstract)
                {
                    BaseDecoder decoder = Activator.CreateInstance(type) as BaseDecoder;
                    object[] attrs = type.GetCustomAttributes(typeof (DecodesChunksAttribute), false);
                    foreach (object attr in attrs)
                    {
                        DecodesChunksAttribute chunkAttr = attr as DecodesChunksAttribute;
                        if (chunkAttr != null)
                        {
                            foreach (string chunkId in chunkAttr.ChunkIds)
                            {
                                RegisterDecoder(chunkId, decoder);
                            }
                        }
                    }
                }
            }
        }
    }
}
