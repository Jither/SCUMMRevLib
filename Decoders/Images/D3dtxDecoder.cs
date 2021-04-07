using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SCUMMRevLib.FileFormats;
using SCUMMRevLib.Utils;

namespace SCUMMRevLib.Decoders.Images
{
    [DecodesChunks(".d3dtx")]
    public class D3dtxDecoder : BaseImageDecoder
    {
        public override uint GetCount(Chunks.Chunk chunk)
        {
            using (var stream = chunk.GetStream())
            {
                var ddsImage = new DDSImage(stream);
                return (uint)ddsImage.MipMapCount;
            }
        }

        public override ImageInfo GetInfo(Chunks.Chunk chunk, uint index)
        {
            using (var stream = chunk.GetStream())
            {
                var ddsImage = new DDSImage(stream);
                return new ImageInfo
                {
                    X = 0,
                    Y = 0,
                    Height = ddsImage.Height,
                    Width = ddsImage.Width,
                    PixelFormat = PixelDepth.Depth32
                };
            }
        }

        public override byte[] Decode(Chunks.Chunk chunk, uint index)
        {
            using (var stream = chunk.GetStream())
            {
                var ddsImage = new DDSImage(stream);
                ddsImage.Decode(stream);
                return ddsImage.images[0].Bitmap;
            }
        }

        public override bool CanDecode(Chunks.Chunk chunk)
        {
            SRFile file = chunk.File;
            file.Position = chunk.Offset;
            return (file.ReadFourCC() == "5VSM");
        }
    }
}
