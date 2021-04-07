using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SCUMMRevLib.Utils;

namespace SCUMMRevLib.Decoders.Boxes
{
    public class ScummBox
    {
        public Point TopLeft { get; protected set; }
        public Point TopRight { get; protected set; }
        public Point BottomRight { get; protected set; }
        public Point BottomLeft { get; protected set; }
        public uint ZPlane { get; protected set; }
        public BoxFlags Flags { get; protected set; }
        public uint Scale { get; protected set; }
        public uint ScaleSlot { get; protected set; }

        public ScummBox(Point topLeft, Point topRight, Point bottomRight, Point bottomLeft, uint zPlane, BoxFlags flags, uint scale, uint scaleSlot)
        {
            TopLeft = topLeft;
            TopRight = topRight;
            BottomRight = bottomRight;
            BottomLeft = bottomLeft;
            ZPlane = zPlane;
            Flags = flags;
            Scale = scale;
            ScaleSlot = scaleSlot;
        }
    }
}
