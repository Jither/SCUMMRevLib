using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Katana.IO;
using SCUMMRevLib.Encryption;

namespace SCUMMRevLib.FileFormats.Factories
{
    /// <summary>
    /// File factory for SCUMM 1 and 2 main game files:
    /// - Maniac Mansion
    /// - Zak McKracken and the Alien Mindbenders (non FM-TOWNS version - SCUMM 3)
    /// </summary>
    public class SCUMM1Factory : FileFactory
    {
        private static readonly byte[] ENCRYPTION_VALUES = { 0xff, 0 };

        protected override Stream CreateStream(string path)
        {
            return new XorStream(path);
        }

        protected override SRFile CreateFromStream(string path, Stream stream)
        {
            var file = new SCUMM1File(path, (XorStream)stream);
            ;
            if (!CheckFormat(path, file))
            {
                file.Close();
                file = null;
            }
            return file;
        }

        private bool CheckFormat(string path, SCUMM1File file)
        {
            // Detection here is a bit hacky:
            // - We NEED the file to be named xx.LFL
            // - We NEED the file to be stored along with the 00.LFL directory (it provides our check)
            // - We NEED it to use the same encryption as the directory

            if (!String.Equals(Path.GetExtension(path), ".lfl", StringComparison.OrdinalIgnoreCase))
            {
                // Not the correct file extension
                return false;
            }

            int fileNumber;
            string strFileNumber = Path.GetFileNameWithoutExtension(path);
            if (!Int32.TryParse(strFileNumber, out fileNumber))
            {
                // Not the correct file name (decimal digits)
                return false;
            }

            if (fileNumber == 0)
            {
                // Directory file
                file.IsDirectory = true;
                file.FileVersion = GetDirectoryVersion(file);
            }
            else
            {
                // Resource file - determine version through Directory file
                var directoryFile = SCUMM1File.OpenDirectory(path);
                if (directoryFile == null)
                {
                    return false;
                }

                file.FileVersion = GetDirectoryVersion(directoryFile);
                file.Encryption = directoryFile.Encryption;
                directoryFile.Close();
            }

            return true;
        }

        private int GetDirectoryVersion(SRXORFile file)
        {
            foreach (byte enc in ENCRYPTION_VALUES)
            {
                file.Encryption = enc;

                file.Position = 0;

                ushort magic = file.ReadU16LE();
                switch (magic)
                {
                    case 0x0a31:
                        return 1;
                    case 0x0100:
                        return 2;
                }
            }
            return -1;
        }

    }
}
