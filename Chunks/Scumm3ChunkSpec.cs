using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCUMMRevLib.Chunks
{
    public class SCUMM3ChunkSpec
    {
        public string Id { get; protected set; }
        public bool HasChildren { get; protected set; }
        public string Description { get; protected set; }
        public uint ChildOffset { get; protected set; }
        public ImageIndex ImageIndex { get; protected set; }

        public SCUMM3ChunkSpec(string id, string description, bool hasChildren, uint childOffset, ImageIndex imageIndex)
        {
            this.Id = id;
            this.Description = description;
            this.HasChildren = hasChildren;
            this.ChildOffset = childOffset;
            this.ImageIndex = imageIndex;
        }
    }
}
