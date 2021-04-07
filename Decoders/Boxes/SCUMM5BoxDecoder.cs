using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Katana.IO;
using SCUMMRevLib.Chunks;
using SCUMMRevLib.Utils;

namespace SCUMMRevLib.Decoders.Boxes
{
    [DecodesChunks("BOXD")]
    public class SCUMM5BoxDecoder : BaseBoxDecoder
    {
        public override uint GetCount(Chunk chunk)
        {
            BinReader reader = chunk.GetReader();
            reader.Position = 8;
            // This works for CMI (dword size) too, as long as there are less than 65536 boxes...
            return reader.ReadU16LE();
        }

        public override List<ScummBox> Decode(Chunk chunk)
        {
            BinReader reader = chunk.GetReader();
            reader.Position = 8;
            uint count = reader.ReadU16LE();

            List<ScummBox> boxes = new List<ScummBox>();

            if (count * 20 + 10 < chunk.Size)
            {
                // CMI (SCUMM 8)
                reader.Position = 12;
                for (uint i = 0; i < count; i++)
                {
                    Point topLeft = new Point(reader.ReadS32LE(), reader.ReadS32LE());
                    Point topRight = new Point(reader.ReadS32LE(), reader.ReadS32LE());
                    Point bottomRight = new Point(reader.ReadS32LE(), reader.ReadS32LE());
                    Point bottomLeft = new Point(reader.ReadS32LE(), reader.ReadS32LE());

                    uint zPlane = reader.ReadU32LE();
                    uint intFlags = reader.ReadU32LE();
                    uint scaleSlot = reader.ReadU32LE();
                    uint scale = reader.ReadU32LE();

                    BoxFlags flags = (BoxFlags)intFlags;

                    uint unknown1 = reader.ReadU32LE(); // TODO: Figure out what these are

                    ScummBox box = new ScummBox(topLeft, topRight, bottomRight, bottomLeft, zPlane, flags, scale, scaleSlot);
                    boxes.Add(box);
                }
            }
            else
            {
                // Pre v8 SCUMM
                reader.Position = 10;
                for (uint i = 0; i < count; i++)
                {
                    Point topLeft = new Point(reader.ReadS16LE(), reader.ReadS16LE());
                    Point topRight = new Point(reader.ReadS16LE(), reader.ReadS16LE());
                    Point bottomRight = new Point(reader.ReadS16LE(), reader.ReadS16LE());
                    Point bottomLeft = new Point(reader.ReadS16LE(), reader.ReadS16LE());

                    uint zPlane = reader.ReadU8();
                    uint intFlags = reader.ReadU8();
                    uint scale = reader.ReadU16LE();
                    uint scaleSlot = 0;

                    BoxFlags flags = (BoxFlags) intFlags;

                    if ((scale & 0x8000) != 0)
                    {
                        scaleSlot = scale & 0x7fff;
                        scale = 0;
                    }

                    ScummBox box = new ScummBox(topLeft, topRight, bottomRight, bottomLeft, zPlane, flags, scale, scaleSlot);
                    boxes.Add(box);
                }
            }

            return boxes;
        }

        public override bool CanDecode(Chunk chunk)
        {
            return (chunk.Parent != null && chunk.Parent.ChunkTypeId == "ROOM");
        }
    }
}
