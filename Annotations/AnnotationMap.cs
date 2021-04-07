using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCUMMRevLib.Annotations
{
    public class AnnotationMap : Dictionary<string, string>
    {
        private static string GetChunkKey(string chunkTypeId, ulong offset)
        {
            return String.Format("{0}_{1}", chunkTypeId, offset);
        }

        public void Set(string chunkTypeId, ulong offset, string annotation)
        {
            string chunkKey = GetChunkKey(chunkTypeId, offset);
            if (ContainsKey(chunkKey))
            {
                this[chunkKey] = annotation;
            }
            else
            {
                this.Add(chunkKey, annotation);
            }
        }

        public string Get(string chunkTypeId, ulong offset)
        {
            string chunkKey = GetChunkKey(chunkTypeId, offset);
            if (!ContainsKey(chunkKey))
            {
                return "";
            }
            return this[chunkKey];
        }
    }
}
