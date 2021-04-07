using System;
using SCUMMRevLib.FileFormats;

namespace SCUMMRevLib.Chunks
{
    public class LB83Chunk : Chunk
    {
        public LB83Chunk(SRFile file, Chunk parent) : base(file, parent)
        {
            Name = "LB83";
            Offset = file.Position;
            Size = file.Size;
        }

        public override string ChunkTypeId
        {
            get { return "LB83"; }
        }

        public override string Description
        {
            get { return "LucasArts 8.3 Bundle"; }
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

            uint dirOffset = file.ReadU32BE();
            uint fileCount = file.ReadU32BE();
            string date = file.ReadString(12);

            file.Position = dirOffset;

            for (uint i = 0; i < fileCount; i++)
            {
                string name = "";

                byte b;
                for (int index = 0; index <= 11; index++ )
                {
                    b = file.ReadU8();
                    if (b != 0)
                    {
                        if (index == 8)
                        {
                            name += ".";
                        }
                        name += (char)b;
                    }
                } 

                uint offset = file.ReadU32BE();
                uint size = file.ReadU32BE();
                Chunk chunk = new FileChunk(file, this, name, offset, size);
                result.Add(chunk);
            }

            return result;
        }
    }
}
