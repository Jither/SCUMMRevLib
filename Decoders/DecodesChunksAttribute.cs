using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCUMMRevLib.Decoders
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class DecodesChunksAttribute : Attribute
    {
        public string[] ChunkIds { get; protected set; }

        public DecodesChunksAttribute(params string[] chunkIds)
        {
            this.ChunkIds = chunkIds;
        }
    }
}
