using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SCUMMRevLib.FileFormats;

namespace SCUMMRevLib.Chunks
{
    public class BNDTChunk : Chunk
    {
        public BNDTChunk(SRFile file, Chunk parent, ulong offset, uint size) : base(file, parent)
        {
            Name = "BNDT";
            Offset = offset;
            Size = size;
        }

        public override string ChunkTypeId
        {
            get { return "BNDT"; }
        }

        public override string Description
        {
            get { return "BtM Bundle Data"; }
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
            var bnhdChunk = SelectSingle("../BNHD");
            if (bnhdChunk == null)
            {
                throw new ScummRevisitedException("BNHD chunk is required to read contents of BNDT.");
            }

            ChunkList result = new ChunkList();
            using (var reader = bnhdChunk.GetReader())
            {
                reader.Position = 0x1002a; // header (0x2a) + some table (0x10000)
                while (reader.Position < reader.Size)
                {
                    string name = reader.ReadStringZ(0xaa, true);
                    reader.Position += (ulong)(200 - (name.Length + 1));
                    uint size = reader.ReadU32LE();
                    ulong offset = reader.ReadU32LE() + this.Offset + 8; // Offset is relative to start of BNDT's content (after FourCC and size)
                    var chunk = new FileChunk(file, this, name, offset, size);

                    result.Add(chunk);

                    reader.Position += 44;
                }
            }
            return result;
        }
    }
}
