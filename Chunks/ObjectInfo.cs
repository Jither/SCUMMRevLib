using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCUMMRevLib.Chunks
{
    public class ObjectInfo
    {
        public int Version { get; set; }
        public string Name { get; set; }
        public int Id { get; set; }
        public uint ImageCount { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
    }
}
