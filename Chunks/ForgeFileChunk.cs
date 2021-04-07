using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SCUMMRevLib.FileFormats;

namespace SCUMMRevLib.Chunks
{
    public class ForgeFileChunk : Chunk
    {
        public ForgeFileChunk(DataBlockInfo info, SRFile file, Chunk parent) : base(file, parent)
        {
            Offset = info.Offset;
            Size = info.Size;
            Name = info.Name;
        }

        public override string ChunkTypeId
        {
            get { return "ForgeFile"; }
        }

        public override string Description
        {
            get { return "Forge File"; }
        }

        public override bool HasChildren
        {
            get { return false; }
        }

        public override ImageIndex ImageIndex
        {
            get { return ImageIndex.Unknown; }
        }

        protected override ChunkList InternalGetChildren()
        {
            throw new NotImplementedException();
        }
    }
}
