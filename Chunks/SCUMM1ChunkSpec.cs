using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCUMMRevLib.Chunks
{
    public class SCUMM1ChunkSpec
    {
        public string Id { get; protected set; }
        public bool HasChildren { get; protected set; }
        public string Description { get; protected set; }
        public ImageIndex ImageIndex { get; protected set; }

        public SCUMM1ChunkSpec(string id, string description, bool hasChildren, ImageIndex imageIndex)
        {
            this.Id = id;
            this.Description = description;
            this.HasChildren = hasChildren;
            this.ImageIndex = imageIndex;
        }
    }
}
