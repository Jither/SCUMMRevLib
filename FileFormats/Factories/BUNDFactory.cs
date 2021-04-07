using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Katana.Types;
using SCUMMRevLib.Encryption;
using SCUMMRevLib.Utils;

namespace SCUMMRevLib.FileFormats.Factories
{
    /// <summary>
    /// File factory for BUND bundles:
    /// - Star Wars: Behind the Magic
    /// </summary>
    [FileType("Behind the Magic Bundles", "000", "001")]
    public class BUNDFactory : FileFactory
    {
        protected override Stream CreateStream(string path)
        {
            return new XorStream(path);
        }

        protected override SRFile CreateFromStream(string path, Stream stream)
        {
            var result = new BUNDFile(path, stream);
            if (!CheckFormat(result))
            {
                result.Close();
                result = null;
            }
            return result;
        }

        private bool CheckFormat(BUNDFile file)
        {
            file.Position = 0;
            FourCC fourCC = file.ReadFourCC();
            uint size = file.ReadU32BE();

            if (fourCC == "BUND" && size <= file.Size)
            {
                return true;
            }
            return false;
        }

    }
}
