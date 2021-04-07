using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SCUMMRevLib.Decoders;

namespace SCUMMRevLib.Viewers
{
    public class ViewerMap
    {
        private readonly Dictionary<DecoderFormat, List<IViewer>> map;

        public IList<IViewer> this[DecoderFormat index]
        {
            get
            {
                if (!map.ContainsKey(index))
                {
                    return null;
                }
                return map[index].AsReadOnly();
            }
        }

        public ViewerMap()
        {
            map = new Dictionary<DecoderFormat, List<IViewer>>();
        }

        public void Add(DecoderFormat format, IViewer viewer)
        {
            List<IViewer> viewers;
            if (map.ContainsKey(format))
            {
                viewers = map[format];
            }
            else
            {
                viewers = new List<IViewer>();
                map.Add(format, viewers);
            }

            viewers.Add(viewer);
        }
    }
}
