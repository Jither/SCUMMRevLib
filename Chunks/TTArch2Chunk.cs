using System;
using System.IO;
using System.Text;
using Katana.IO;
using Katana.Types;
using SCUMMRevLib.FileFormats;

namespace SCUMMRevLib.Chunks
{
    public class TTARCH2Chunk : Chunk
    {
        public override string ChunkTypeId
        {
            get { return "TTARCH2"; }
        }

        public override string Description
        {
            get { return "TellTale Archive"; }
        }

        public override bool HasChildren
        {
            get { return true; }
        }

        public override ImageIndex ImageIndex
        {
            get { return ImageIndex.Bundle; }
        }

        protected override ChunkList InternalGetChildren()
        {
            // TODO: This should probably be moved to be handled by TellTaleFile
            file.Position = Offset;
            var reader = file;

            FourCC format = reader.ReadFourCC();

            ChunkList result;

            switch (format.Name)
            {
                case "ECTT":
                case "ZCTT":
                    // Skip format, block size and count:
                    reader.Position = 12;

                    ulong att3Offset = reader.ReadU64LE();
                    reader.Position = att3Offset;
                    return ReadTTA3(reader);
                case "NCTT":
                    // Not encrypted, not compressed(?)
                    // Skip data size
                    reader.ReadU64LE();
                    result = ReadTTA3(reader);
                    break;
                case "3ATT":
                    result = ReadTTA3(reader);
                    break;
                default:
                    throw new ScummRevisitedException("Unknown format: {0}", format);
            }

            return result;
        }

        private ChunkList ReadTTA3(BinReader reader)
        {
            ChunkList result = new ChunkList();

            FourCC tta3 = reader.ReadFourCC();
            uint tta3Version = reader.ReadU32LE();
            uint namesSize = reader.ReadU32LE();
            uint fileCount = reader.ReadU32LE();

            uint infoSize = fileCount * (8 + 8 + 4 + 4 + 2 + 2);
            ulong namesOffset = reader.Position + infoSize;
            ulong baseOffset = namesOffset + namesSize;

            byte[] infoTable = new byte[infoSize];
            byte[] namesTable = new byte[namesSize];

            reader.Read(infoTable, 0, infoSize);
            reader.Read(namesTable, 0, namesSize);

            using (MemoryStream infoStream = new MemoryStream(infoTable))
            {
                using (BinReader infoReader = new BinReader(infoStream))
                {
                    for (int i = 0; i < fileCount; i++)
                    {
                        ulong hash = infoReader.ReadU64LE();
                        // In compressed archives, the offset is the offset in the "virtual" uncompressed archive
                        ulong offset = infoReader.ReadU64LE() + baseOffset;
                        uint size = infoReader.ReadU32LE();
                        uint unknown = infoReader.ReadU32LE();
                        ushort nameBlock = infoReader.ReadU16LE();
                        ushort nameOffset = infoReader.ReadU16LE();

                        long namePosition = nameOffset + (long)nameBlock * 0x10000;
                        string name = GetNameFromTable(namesTable, namePosition);

                        Chunk chunk = new TellTaleFileChunk(file, this, name, offset, size);
                        result.Add(chunk);
                    }
                }
            }

            return result;
        }

        private StringBuilder nameBuilder = new StringBuilder();

        private string GetNameFromTable(byte[] nameTable, long namePosition)
        {
            nameBuilder.Clear();
            while (namePosition < nameTable.LongLength)
            {
                byte b = nameTable[namePosition];
                if (b == 0)
                {
                    break;
                }
                nameBuilder.Append((char)b);

                namePosition++;
            }
            return nameBuilder.ToString();
        }

        public TTARCH2Chunk(SRFile file, Chunk parent) : base(file, parent)
        {
            this.Name = "TTARCH2";
            this.Offset = file.Position;
            this.Size = file.Size;
        }
    }
}
