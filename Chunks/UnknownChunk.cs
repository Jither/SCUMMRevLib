using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SCUMMRevLib.FileFormats;

namespace SCUMMRevLib.Chunks
{
    public class UnknownChunk : Chunk
    {
        public UnknownChunk(SRFile file, Chunk parent, ulong offset, uint size) : base(file, parent)
        {
            this.Offset = offset;
            this.Size = size;
            this.Name = "????";
        }

        public override string ChunkTypeId
        {
            get { return "UNKNOWN"; }
        }

        public override string Description
        {
            get { return "Unknown Chunk"; }
        }

        public override ImageIndex ImageIndex
        {
            get { return ImageIndex.Unknown; }
        }

        public override bool HasChildren
        {
            get { return false; }
        }

        protected override ChunkList InternalGetChildren()
        {
            throw new NotImplementedException();
        }
    }
}
