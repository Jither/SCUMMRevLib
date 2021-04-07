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
    /// File factory for SCUMM 5+ main game files:
    /// - Monkey Island 2
    /// - The Secret of Monkey Island, CD-ROM edition
    /// - Indiana Jones and the Fate of Atlantis
    /// - Maniac Mansion 2: Day of the Tentacle
    /// - Full Throttle
    /// - The Dig
    /// - The Curse of Monkey Island
    /// </summary>
    [FileType("Main resource files", "001", "la1", "la2", "lec", "sm1")]
    [FileType("Directory files", "000", "la0", "sm0")]
    [FileType("SMUSH files", "san", "nut")]
    [FileType("Bundles", "sou")]
    public class SCUMM5Factory : FileFactory
    {
        private static readonly byte[] ENCRYPTION_VALUES = { 0, 0x69, 0xff };

        protected override Stream CreateStream(string path)
        {
            return new XorStream(path);
        }

        protected override SRFile CreateFromStream(string path, Stream stream)
        {
            var file = new SCUMM5File(path, (XorStream)stream);
            if (!CheckFormat(file))
            {
                file.Close();
                file = null;
            }
            return file;
        }

        private bool CheckFormat(SCUMM5File file)
        {
            foreach (byte enc in ENCRYPTION_VALUES)
            {
                file.Encryption = enc;

                file.Position = 0;
                FourCC fourCC = file.ReadFourCC();
                uint size = file.ReadU32BE();

                if (fourCC.IsValid && size <= file.Size)
                {
                    file.FileVersion = SCUMMUtils.DetermineSCUMMVersion(file);
                    return true;
                }
            }
            return false;

        }

    }
}
