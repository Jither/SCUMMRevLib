using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SCUMMRevLib.Decoders.Palettes;
using SCUMMRevLib.FileFormats;
using SCUMMRevLib.Utils;
using SCUMMRevLib.Chunks;

namespace SCUMMRevLib.Decoders.Images
{
    /*
    [DecodesChunks("OBIM")]
    public class MMucusObjectImageDecoder : MMucusImageDecoder
    {
        public override uint GetCount(Chunk chunk)
        {
            try
            {
                ObjectInfo info = ChunkUtils.GetObjectInfo(chunk);
                return info.ImageCount;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public override ImageInfo GetInfo(Chunk chunk, uint index)
        {
            ImageInfo info = new ImageInfo();
            ObjectInfo objInfo = ChunkUtils.GetObjectInfo(chunk);
            if (index > objInfo.ImageCount)
            {
                throw new DecodingException("Invalid image index");
            }

            info.X = objInfo.X;
            info.Y = objInfo.Y;
            info.Width = objInfo.Width;
            info.Height = objInfo.Height;
            info.PixelFormat = PixelDepth.Depth8;

            Chunk parentChunk = chunk.Parent;

            if (parentChunk == null)
            {
                throw new DecodingException("Parent ROOM chunk not found");
            }

            GetPalette(chunk, parentChunk, info);

            return info;
        }

        public override bool CanDecode(Chunk chunk)
        {
            return GetCount(chunk) > 0;
        }
    }
     */
}
