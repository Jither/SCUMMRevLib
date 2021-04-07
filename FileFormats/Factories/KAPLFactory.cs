using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Katana.Types;

namespace SCUMMRevLib.FileFormats.Factories
{
    /// <summary>
    /// File factory for KAPL LucasArts Special Edition bundles:
    /// - The Secret of Monkey Island: Special Edition
    /// - Monkey Island 2: Special Edition
    /// </summary>
    [FileType("Bundles", "pak")]
    public class KAPLFactory : FileFactory
    {
        protected override SRFile CreateFromStream(string path, Stream stream)
        {
            var file = new KAPLFile(path, stream);
            if (!CheckFormat(file))
            {
                file.Close();
                file = null;
            }
            return file;
        }

        private bool CheckFormat(KAPLFile file)
        {
            FourCC fourCC = file.ReadFourCC();
            if (fourCC == "KAPL")
            {
                return true;
            }
            return false;
        }

    }
}
