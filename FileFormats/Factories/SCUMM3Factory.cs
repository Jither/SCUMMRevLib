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
    /// File factory for SCUMM 3 and 4 main game files:
    /// - Indiana Jones and the Last Crusade
    /// - The Secret of Monkey Island
    /// - Loom
    /// </summary>
    [FileType("Main resource files", "lfl")]
    public class SCUMM3Factory : FileFactory
    {
        private static readonly byte[] ENCRYPTION_VALUES = { 0, 0x69 };

        protected override Stream CreateStream(string path)
        {
            return new XorStream(path);
        }

        protected override SRFile CreateFromStream(string path, Stream stream)
        {
            var file = new SCUMM3File(path, (XorStream)stream);
            if (!CheckFormat(file))
            {
                file.Close();
                file = null;
            }
            
            return file;
        }

        private bool CheckFormat(SCUMM3File file)
        {
            foreach (byte enc in ENCRYPTION_VALUES)
            {
                file.Encryption = enc;

                file.Position = 0;
                uint size = file.ReadU32LE();
                TwoCC twoCC = file.ReadTwoCC();

                if (twoCC.IsValid && size <= file.Size)
                {
                    file.FileVersion = SCUMMUtils.DetermineSCUMMVersion(file);
                    return true;
                }
            }
            return false;
        }

    }
}
