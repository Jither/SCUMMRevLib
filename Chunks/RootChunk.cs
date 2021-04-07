using System;
using SCUMMRevLib.FileFormats;

namespace SCUMMRevLib.Chunks
{
    public class RootChunk : FileChunk
    {
        public RootChunk(SRFile file) : base(file, null, "__ROOT__", 0, file.Size)
        {
        }

        public override string ChunkTypeId
        {
            get { return "__ROOT__"; }
        }

        public override string Description
        {
            get { return "__ROOT__"; }
        }

        public override bool HasChildren
        {
            get { return true; }
        }

        protected override ChunkList InternalGetChildren()
        {
            return file.GetRootChunks();
        }
    }
}
