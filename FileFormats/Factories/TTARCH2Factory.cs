using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using Katana.IO;
using Katana.Types;
using SCUMMRevLib.Encryption;

namespace SCUMMRevLib.FileFormats.Factories
{
    /// <summary>
    /// File factory for TellTale archive files (version 2)
    /// </summary>
    [FileType("Bundles", "ttarch2")]
    public class TTARCH2Factory : FileFactory
    {
        protected override Stream CreateStream(string path)
        {
            var fileStream = File.OpenRead(path);
            using (BinReader reader = new BinReader(fileStream))
            {
                FourCC fourCC = reader.ReadFourCC();
                switch (fourCC.Name)
                {
                    case "ECTT":
                        // Compressed and encrypted
                        TellTaleFileStructureInfo fileInfo = PrepareFileInfo(reader);
                        byte[] key = FindKey(reader, fileInfo, TellTaleKeyManager.Instance.KeysTTArch2);
                        if (key == null)
                        {
                            throw new ScummRevisitedException("Couldn't determine blowfish key");
                        }
                        // ttarch2 always uses modified blowfish algorithm
                        return new TellTaleBlowfishZlibStream(fileStream, fileInfo, key, true);
                    case "ZCTT":
                        // Compressed and unencrypted
                        fileInfo = PrepareFileInfo(reader);
                        return new TellTaleBlowfishZlibStream(fileStream, fileInfo);
                    case "3ATT":
                    case "NCTT":
                        // Uncompressed and unencrypted
                        return fileStream;
                    default:
                        // not TTARCH2
                        fileStream.Dispose();
                        return null;
                }
            }
        }

        protected override SRFile CreateFromStream(string path, Stream stream)
        {
            if (stream == null)
            {
                return null;
            }
            return new TTArch2File(path, stream);
        }

        private TellTaleFileStructureInfo PrepareFileInfo(BinReader reader)
        {
            var result = new TellTaleFileStructureInfo();
            result.FileVersion = 10;

            // For neatness, we reset to start of file, but skipping the format FourCC.
            reader.Position = 4;

            result.BlockSizeUncompressed = reader.ReadU32LE();
            uint blockCount = reader.ReadU32LE();

            ulong blockOffset = reader.ReadU64LE(); // first block offset

            result.VirtualBlocksOffset = blockOffset;

            for (int i = 0; i < blockCount; i++)
            {
                ulong nextBlockOffset = reader.ReadU64LE();
                result.BlockOffsets.Add(blockOffset);
                result.BlockSizesCompressed.Add((uint)(nextBlockOffset - blockOffset));
                blockOffset = nextBlockOffset;
            }

            return result;
        }

        private byte[] FindKey(BinReader reader, TellTaleFileStructureInfo fileInfo, IEnumerable<TellTaleKeyInfo> keys)
        {
            const int decompressSize = 4;
            const int readSize = 4096;
            byte[] readBytes = new byte[readSize];
            byte[] testBytes = new byte[readSize];
            byte[] deflateBytes = new byte[decompressSize];
            reader.Position = fileInfo.VirtualBlocksOffset;
            reader.Read(readBytes, 0, readSize);

            using (MemoryStream stream = new MemoryStream(testBytes))
            {
                foreach (var info in keys)
                {
                    var testBlowfish = new Blowfish(info.Key, true);
                    testBlowfish.Decipher(readBytes, testBytes, readSize);

                    stream.Position = 0;
                    using (DeflateStream deflateStream = new DeflateStream(stream, CompressionMode.Decompress, true))
                    {
                        try
                        {
                            int bytesRead = 0;
                            // FIXME: Sometimes DeflateStream.Read reads 0 bytes...
                            while (bytesRead == 0)
                            {
                                bytesRead = deflateStream.Read(deflateBytes, 0, decompressSize);
                            }
                        }
                        catch (InvalidDataException)
                        {
                            // Since a wrong key may cause invalid zlib input,
                            // we catch the exception for that, and continue to the next key
                            continue;
                        }
                    }
                    if (deflateBytes[0] == '3' && deflateBytes[1] == 'A' && deflateBytes[2] == 'T' &&
                        deflateBytes[3] == 'T')
                    {
                        return info.Key;
                    }

                }
            }
            return null;
        }
    }
}
