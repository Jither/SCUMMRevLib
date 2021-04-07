using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Katana.Types;

namespace SCUMMRevLib.FileFormats.Factories
{
    /// <summary>
    /// File factory for DOS 8.3 filename bundles:
    /// - The Dig
    /// - The Curse of Monkey Island
    /// </summary>
    [FileType("Bundles", "bun")]
    public class LB83Factory : FileFactory
    {
        protected override SRFile CreateFromStream(string path, Stream stream)
        {
            var file = new LB83File(path, stream);
            if (!CheckFormat(file))
            {
                file.Close();
                file = null;
            }
            return file;
        }

        private bool CheckFormat(LB83File file)
        {
            FourCC fourCC = file.ReadFourCC();
            if (fourCC == "LB83")
            {
                return true;
            }
            return false;
        }

    }
}
