using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Katana.Types;

namespace SCUMMRevLib.FileFormats.Factories
{
    /// <summary>
    /// File factory for LABN bundles:
    /// - Grim Fandango
    /// - Escape from Monkey Island
    /// </summary>
    [FileType("Bundles", "lab", "m4b")]
    public class LABNFactory : FileFactory
    {
        protected override SRFile CreateFromStream(string path, Stream stream)
        {
            var file = new LABNFile(path, stream);
            if (!CheckFormat(file))
            {
                file.Close();
                file = null;
            }
            return file;
        }

        private bool CheckFormat(LABNFile file)
        {
            FourCC fourCC = file.ReadFourCC();
            if (fourCC == "LABN")
            {
                return true;
            }
            return false;
        }
    }
}
