using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SCUMMRevLib.Chunks;
using SCUMMRevLib.Decoders;

namespace SCUMMRevLib.Viewers
{
    public interface IViewer
    {
        DecoderFormat DecoderFormat { get; }
        string ActionFormat { get; }
    }
}
