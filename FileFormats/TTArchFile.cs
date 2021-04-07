using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using Katana.IO;
using Katana.Logging;
using Katana.Types;
using SCUMMRevLib.Chunks;
using SCUMMRevLib.Encryption;
using SCUMMRevLib.Utils;

namespace SCUMMRevLib.FileFormats
{
    public class TTArchFile : SRFile
    {
        public TTArchFile(string path, Stream stream) : base(path, stream)
        {

        }

        public override ChunkList GetRootChunks()
        {
            Position = 0;

            ChunkList result = new ChunkList();
            result.Add(new TTARCHChunk(this, RootChunk));
            return result;
        }

        public static TellTaleFileStructureInfo PrepareFileInfo(BinReader reader)
        {
            var result = new TellTaleFileStructureInfo();
            // For neatness, we reset to start of file, but skipping the format FourCC.
            reader.Position = 0;

            result.FileVersion = reader.ReadU32LE();
            uint infoEncrypted = reader.ReadU32LE();

            if (result.FileVersion > 9 || infoEncrypted > 1)
            {
                // Not a ttarch file, or unsupported
                return null;
            }

            uint type3 = reader.ReadU32LE();

            uint filesFormat = 0;
            uint blockCount = 0;
            uint dataSize = 0;
            uint unknown1 = 0;
            uint unknown2 = 0;
            uint unknown3 = 0;
            uint unknown4 = 0;
            byte unknown5 = 0;
            uint unknown6 = 0;

            ulong[] blockOffsets = null;

            if (result.FileVersion >= 3)
            {
                filesFormat = reader.ReadU32LE();
                blockCount = reader.ReadU32LE();

                blockOffsets = new ulong[blockCount];
                if (blockCount > 0)
                {
                    ulong blockOffset = 0;
                    for (int i = 0; i < blockCount; i++)
                    {
                        uint size = reader.ReadU32LE();
                        result.BlockSizesCompressed.Add(size);
                        blockOffsets[i] = blockOffset;
                        blockOffset += size;
                    }
                }

                dataSize = reader.ReadU32LE();
            }

            if (result.FileVersion >= 4)
            {
                unknown1 = reader.ReadU32LE();
                unknown2 = reader.ReadU32LE();
            }
            if (result.FileVersion >= 7)
            {
                unknown3 = reader.ReadU32LE();
                unknown4 = reader.ReadU32LE();
                result.BlockSizeUncompressed = reader.ReadU32LE() * 1024;
            }
            if (result.FileVersion >= 8)
            {
                unknown5 = reader.ReadU8();
            }
            if (result.FileVersion >= 9)
            {
                unknown6 = reader.ReadU32LE();
            }

            result.HasInfo = true;
            result.IsInfoEncrypted = infoEncrypted == 1;

            result.InfoSizeUncompressed = reader.ReadU32LE();

            if (result.FileVersion >= 7 && filesFormat == 2)
            {
                result.IsInfoCompressed = true;
                result.InfoSizeCompressed = reader.ReadU32LE();
            }
            else
            {
                result.InfoSizeCompressed = result.InfoSizeUncompressed;
            }

            result.InfoOffset = reader.Position;
            result.VirtualBlocksOffset = reader.Position + result.InfoSizeUncompressed;
            ulong blocksOffset = reader.Position + result.InfoSizeCompressed;

            for (var i = 0; i < blockCount; i++)
            {
                result.BlockOffsets.Add(blockOffsets[i] + blocksOffset);
            }

            return result;
        }
    }
}
