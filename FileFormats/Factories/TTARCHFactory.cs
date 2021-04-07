using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using Katana.IO;
using SCUMMRevLib.Encryption;
using SCUMMRevLib.Utils;

namespace SCUMMRevLib.FileFormats.Factories
{
    /// <summary>
    /// File factory for TellTale archive files (version 1)
    /// </summary>
    [FileType("Bundles", "ttarch")]
    public class TTARCHFactory : FileFactory
    {
        private static string KEY = "96ca999f8dda9a87d7cdd9956295aab8d59596e5a4b99bd0c9529f8590cdcd9fc8b39993c6c49d9da5a4cfcda39dbbddaca78b94d4a36f";

        protected override Stream CreateStream(string path)
        {
            var fileStream = File.OpenRead(path);
            using (BinReader reader = new BinReader(fileStream))
            {
                TellTaleFileStructureInfo fileInfo = PrepareFileInfo(reader);
                if (fileInfo == null)
                {
                    return null;
                }
                byte[] key = FindKey(reader, fileInfo, TellTaleKeyManager.Instance.KeysTTArch);
                // TODO: Make sure key was found
                return new TellTaleBlowfishZlibStream(fileStream, fileInfo, key, fileInfo.FileVersion >= 7);
            }
        }

        protected override SRFile CreateFromStream(string path, Stream stream)
        {
            return new TTArchFile(path, stream);
        }

        private byte[] FindKey(BinReader reader, TellTaleFileStructureInfo fileInfo, IEnumerable<TellTaleKeyInfo> keys)
        {
            // ttarch1 doesn't have a nice magic FourCC to check for.
            // Instead, we have two possibilities:
            // - if the info block is encrypted, we can check if it follows known info block "rules":
            //   - its folder count is a small number (usually 1 or 2)
            //   - its folder name lengths aren't insane
            //   - its folder names decode to typical ASCII characters
            //   - etc.
            // - if it's not, we can instead check if the first block starts with something we can recognize

            if (fileInfo.IsInfoEncrypted)
            {
                return DetermineKeyByInfoTable(reader, fileInfo, keys);
            }
            else
            {
                return DetermineKeyByFirstBlock(reader, fileInfo, keys);
            }
        }

        private static byte[] DetermineKeyByInfoTable(BinReader reader, TellTaleFileStructureInfo fileInfo, IEnumerable<TellTaleKeyInfo> keys)
        {
            byte[] encryptedTable = fileInfo.ReadInfoBlock(reader, null);
            byte[] decryptedTable = new byte[encryptedTable.Length];

            foreach (var info in keys)
            {
                var blowfish = new Blowfish(info.Key, fileInfo.FileVersion >= 7);
                blowfish.Decipher(encryptedTable, decryptedTable, (uint)(encryptedTable.Length / 8) * 8);

                // Now we do our assertions which should be true if the key was right:
                using (MemoryStream infoStream = new MemoryStream(decryptedTable))
                {
                    using (var infoReader = new BinReader(infoStream))
                    {
                        uint folderCount = infoReader.ReadU32LE();
                        // The maximum number of folders is somewhat arbitrary.
                        // In games tested, I haven't seen folder counts this high.
                        if (folderCount > 128)
                        {
                            Trace.TraceInformation("Key check - folder count: {0}", folderCount);
                            continue;
                        }

                        // We just check the first folder:
                        uint nameSize = infoReader.ReadU32LE();
                        if (nameSize > 300)
                        {
                            Trace.TraceInformation("Key check - folder name size: {0}", nameSize);
                            continue;
                        }

                        return info.Key;
                    }
                }

            }

            return null;
        }

        private static byte[] DetermineKeyByFirstBlock(BinReader reader, TellTaleFileStructureInfo fileInfo, IEnumerable<TellTaleKeyInfo> keys)
        {
            // TODO: Implement key finding
            return HexUtils.StringToByteArray(KEY);
        }

        private TellTaleFileStructureInfo PrepareFileInfo(BinReader reader)
        {
            return TTArchFile.PrepareFileInfo(reader);
        }

    }
}
