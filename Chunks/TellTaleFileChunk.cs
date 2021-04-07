using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using SCUMMRevLib.FileFormats;

namespace SCUMMRevLib.Chunks
{
    public class TellTaleFileChunk : Chunk
    {
        private FileChunkSpec spec;

        public override string ChunkTypeId
        {
            get { return spec.Extension; }
        }

        public override bool OffsetIsInternal
        {
            get
            {
                // TODO: This is only true when chunk is in a compressed archive
                return true;
            }
        }

        public override string Description
        {
            get { return spec.Description; }
        }

        public override bool HasChildren
        {
            // For now
            get { return false; }
        }

        public override ImageIndex ImageIndex
        {
            get { return spec.ImageIndex; }
        }

        protected override ChunkList InternalGetChildren()
        {
            // For now
            return new ChunkList();
        }

        public TellTaleFileChunk(SRFile file, Chunk parent, string name, ulong offset, uint size) : base(file, parent)
        {
            spec = FileChunkSpecs.GetSpec(Path.GetExtension(name).ToLowerInvariant());
            Name = name;
            Offset = offset;
            Size = size;
        }
    }
}
