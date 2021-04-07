using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCUMMRevLib.SpecScript
{
    public class ChunkSpecifications : Dictionary<string, byte[]>
    {
        public void Append(ChunkSpecifications specs)
        {
            foreach (string name in specs.Keys)
            {
                if (ContainsKey(name))
                {
                    continue;
                }
                Add(name, specs[name]);
            }
        }
    }
}
