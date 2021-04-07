using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using SCUMMRevLib.FileFormats;

namespace SCUMMRevLib.Chunks
{
    public class KAPLChunk : Chunk
    {
        public KAPLChunk(SRFile file, Chunk parent)
            : base(file, parent)
        {
            this.Name = "KAPL";
            this.Offset = file.Position;
            this.Size = file.Size;
        }

        public override string ChunkTypeId
        {
            get { return "KAPL"; }
        }

        public override string Description
        {
            get { return "LucasArts Package"; }
        }

        public override ImageIndex ImageIndex
        {
            get { return ImageIndex.Bundle; }
        }

        public override bool HasChildren
        {
            get { return true; }
        }

        protected override ChunkList InternalGetChildren()
        {
            ChunkList result = new ChunkList();
            file.Position = Offset + 4;

            float version = file.ReadU32LE();
            uint crcOffset = file.ReadU32LE();
            uint dirOffset = file.ReadU32LE();
            uint nameTableOffset = file.ReadU32LE();
            uint dataOffset = file.ReadU32LE();
            uint crcSize = file.ReadU32LE();
            uint dirSize = file.ReadU32LE();
            uint nameTableSize = file.ReadU32LE();
            uint dataSize = file.ReadU32LE();

            uint dirPosition = dirOffset;
            while (dirPosition < nameTableOffset)
            {
                file.Position = dirPosition;
                uint offset = dataOffset + file.ReadU32LE();
                uint nameOffset = nameTableOffset + file.ReadU32LE();
                uint size = file.ReadU32LE();
                uint otherSize = file.ReadU32LE();
                uint flags = file.ReadU32LE();

                file.Position = nameOffset;
                string name = file.ReadStringZ();

                Chunk chunk = new FileChunk(file, this, name, offset, size);
                result.Add(chunk);

                dirPosition += 20;
            }

            return result;
        }
    }
}
