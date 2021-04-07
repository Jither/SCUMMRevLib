using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SCUMMRevLib.FileFormats.Factories
{
    /// <summary>
    /// Fallback file factory for unknown file types.
    /// This simply results in a single chunk starting at offset 0 and with a size equal to the file size.
    /// This allows SCUMM Revisited to at least allow hex viewing of unknown file types.
    /// </summary>
    public class UnknownFactory : FileFactory
    {
        protected override SRFile CreateFromStream(string path, Stream stream)
        {
            return new UnknownFile(path, stream);
        }
    }
}
