using System;
using System.Collections.Generic;
using System.Linq;
using SCUMMRevLib.Chunks;
using SCUMMRevLib.Decoders;

namespace SCUMMRevLib.Viewers
{
    public class ActionParameters
    {
        public BaseDecoder Decoder { get; protected set; }
        public IViewer Viewer { get; protected set; }
        public Chunk Chunk { get; protected set; }

        public string ActionText { get { return String.Format(Viewer.ActionFormat, Decoder.GetOutputDescription(Chunk));}}

        public ActionParameters(Chunk chunk, BaseDecoder decoder, IViewer viewer)
        {
            this.Chunk = chunk;
            this.Decoder = decoder;
            this.Viewer = viewer;
        }
    }
}
