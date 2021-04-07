using System;
using System.Diagnostics;
using SCUMMRevLib.FileFormats;

namespace SCUMMRevLib.Chunks
{
    public class LABNChunk : Chunk
    {
        public LABNChunk(SRFile file, Chunk parent) : base(file, parent)
        {
            Name = "LABN";
            Offset = file.Position;
            Size = file.Size;
        }

        public override string ChunkTypeId
        {
            get { return "LABN"; }
        }

        public override string Description
        {
            get { return "LucasArts Bundle"; }
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
            file.Position = Offset + 8;

            uint fileCount = file.ReadU32LE();
            uint nameDirectorySize = file.ReadU32LE();
            uint nameDirectoryOffset = file.ReadU32LE();

            byte nameEncryption = 0;
            if (nameDirectoryOffset == 0) 
            {
                // GF bundle:
                // We actually just read the first file name offset, so retreat:
                file.Position -= 4;
                nameDirectoryOffset = 0x10 + fileCount * 0x10;
            }
            else
            {
                // EMI bundle:
                // Names are encrypted:
                nameEncryption = 0x96;
                // Looks like an attempt to make reverse engineering harder:
                nameDirectoryOffset -= 0x13D0F;
            }

            uint[] nameOffsets = new uint[fileCount];
            uint[] offsets = new uint[fileCount];
            uint[] sizes = new uint[fileCount];

            for (uint i = 0; i < fileCount; i++)
            {
                nameOffsets[i] = file.ReadU32LE();
                offsets[i] = file.ReadU32LE();
                sizes[i] = file.ReadU32LE();
                uint reserved = file.ReadU32LE();
                Debug.Assert(reserved == 0, String.Format("Reserved directory value {0} isn't 0", i));
            }

            for (uint i = 0; i < fileCount; i++)
            {
                file.Position = nameDirectoryOffset + nameOffsets[i];

                string name = nameEncryption == 0 ? file.ReadStringZ() : file.ReadStringZ(nameEncryption);
                Chunk chunk = new FileChunk(file, this, name, offsets[i], sizes[i]);
                result.Add(chunk);
            }
            return result;
        }
    }
}
