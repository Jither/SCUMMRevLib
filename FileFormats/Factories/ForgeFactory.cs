using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SCUMMRevLib.FileFormats.Factories
{
    [FileType("Anvil/AnvilNext (Scimitar) Forge files", "forge")]
    public class ForgeFactory : FileFactory
    {
        protected override SRFile CreateFromStream(string path, Stream stream)
        {
            var file = new ForgeFile(path, stream);
            if (!CheckFormat(file))
            {
                file.Close();
                file = null;
            }
            return file;
        }

        private bool CheckFormat(SRFile file)
        {
            return file.ReadString(8) == "scimitar";
        }

    }
}
