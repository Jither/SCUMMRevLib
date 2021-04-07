using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SCUMMRevLib.FileFormats;

namespace SCUMMRevLib.Chunks
{
    public class DataBlockInfo
    {
        public ulong Offset { get; private set; }
        public uint Identifier { get; private set; }
        public uint Size { get; private set; }
        public ulong Checksum { get; set; }
        public ulong Unknown1 { get; set; }
        public ulong Unknown2 { get; set; }
        public uint NextFileIndex { get; set; }
        public uint PreviousFileIndex { get; set; }
        public uint Unknown3 { get; set; }
        public uint Timestamp { get; set; }
        public string Name { get; set; }
        public ulong Unknown4 { get; set; }
        public uint Unknown5 { get; set; }
        public uint Unknown6 { get; set; }

        public DataBlockInfo(ulong offset, uint identifier, uint size)
        {
            Offset = offset;
            Identifier = identifier;
            Size = size;
        }
    }

    public class ForgeChunk : Chunk
    {
        private uint version;
        private ulong indexOffset;
        private ulong unknown1;
        private ulong unknown2;

        public ForgeChunk(SRFile file, Chunk parent) : base(file, parent)
        {
            this.Name = "Forge";
            this.Offset = 0;
            this.Size = file.Size;
        }

        public override string ChunkTypeId
        {
            get { return "Forge"; }
        }

        public override string Description
        {
            get { return "Anvil/AnvilNext (Scimitar) package"; }
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
            ChunkList result = new ChunkList();

            file.Position = 9;
            version = file.ReadU32LE();
            indexOffset = file.ReadU64LE();
            unknown1 = file.ReadU64LE();
            unknown2 = file.ReadU64LE();

            // Index
            file.Position = indexOffset;
            uint fileCount = file.ReadU32LE();
            uint index3Records = file.ReadU32LE();
            
            // Only in AnvilNext?:
            uint unknown3 = file.ReadU32LE();

            ulong unknown4 = file.ReadU64LE();
            ulong unknown5 = file.ReadU64LE();
            uint maxFilesPerFragment = file.ReadU32LE();
            uint fragmentIndex = file.ReadU32LE();
            ulong nextFragmentOffset = file.ReadU64LE();

            while (nextFragmentOffset != 0xffffffffffffffff)
            {
                file.Position = nextFragmentOffset;
                uint fileCountInFragment = file.ReadU32LE();
                uint fragmentIndex3Records = file.ReadU32LE();

                ulong index1Offset = file.ReadU64LE();

                nextFragmentOffset = file.ReadU64LE();
                uint firstFileIndex = file.ReadU32LE();
                uint lastFileIndex = file.ReadU32LE();
                ulong index2Offset = file.ReadU64LE();
                ulong index3Offset = file.ReadU64LE();

                file.Position = index1Offset;
                List<DataBlockInfo> dataBlocks = new List<DataBlockInfo>();
                for (uint fileIndex = 0; fileIndex < fileCountInFragment; fileIndex++)
                {
                    ulong offset = file.ReadU64LE();
                    uint identifier = file.ReadU32LE();
                    uint unknown = file.ReadU32LE();
                    uint size = file.ReadU32LE();

                    dataBlocks.Add(new DataBlockInfo(offset, identifier, size));
                }

                file.Position = index2Offset;
                for (int fileIndex = 0; fileIndex < fileCountInFragment; fileIndex++)
                {
                    DataBlockInfo info = dataBlocks[fileIndex];
                    uint size = file.ReadU32LE();
                    info.Checksum = file.ReadU64LE();
                    info.Unknown1 = file.ReadU64LE();
                    info.Unknown2 = file.ReadU64LE();
                    info.NextFileIndex = file.ReadU32LE();
                    info.PreviousFileIndex = file.ReadU32LE();
                    info.Unknown3 = file.ReadU32LE();
                    info.Timestamp = file.ReadU32LE();
                    info.Name = file.ReadStringZ(128);
                    info.Unknown4 = file.ReadU64LE();
                    info.Unknown5 = file.ReadU32LE();
                    info.Unknown6 = file.ReadU32LE();
                    // AnvilNext only?
                    uint unknown7 = file.ReadU32LE();
                }

                foreach (DataBlockInfo info in dataBlocks)
                {
                    result.Add(new ForgeFileChunk(info, file, this));
                }
            }

            return result;
        }
    }
}
