using System;
using Katana.IO;
using SCUMMRevLib.Chunks;
using SCUMMRevLib.Decoders.Palettes;
using SCUMMRevLib.Utils;

namespace SCUMMRevLib.Decoders.Images
{
    [DecodesChunks("BM")]
    public class BMDecoder : BaseImageDecoder
    {
        public override uint GetCount(Chunk chunk)
        {
            return 1;
        }

        public override ImageInfo GetInfo(Chunk chunk, uint index)
        {
            Chunk parentChunk = chunk.Parent;

            ImageInfo info = new ImageInfo();
            info.X = 0;
            info.Y = 0;
            Size dims = SCUMMUtils.GetRoomDimensions(parentChunk);
            info.Width = dims.Width;
            info.Height = dims.Height;
            info.PixelFormat = PixelDepth.Depth8;

            GetPalette(chunk, parentChunk, info);

            return info;
        }

        protected void GetPalette(Chunk decodeChunk, Chunk roomChunk, ImageInfo info)
        {
            Chunk paletteChunk = roomChunk.SelectSingle("PA");

            if (paletteChunk == null)
            {
                // TODO: Handle EGA
                throw new DecodingException("Palette chunk not found");
            }

            // TODO: Check if this should be global
            PADecoder paletteDecoder = new PADecoder();
            info.Palette = paletteDecoder.Decode(paletteChunk);
        }


        public override byte[] Decode(Chunk chunk, uint index)
        {
            if (GetCount(chunk) <= index)
            {
                throw new DecodingException("Invalid image index");
            }

            ImageInfo info = GetInfo(chunk, index);

            byte[] destBuffer = DecodeSmap(chunk, info);

            return destBuffer;
        }

        protected byte[] DecodeSmap(Chunk smapChunk, ImageInfo info)
        {
            byte[] buffer = new byte[info.Width*info.Height];

            int stripCount = info.Width / 8;

            int[] stripOffsets = new int[stripCount];

            BinReader reader = smapChunk.GetReader();
            reader.Position = 10;
            for (int i = 0; i < stripCount; i++)
            {
                int offs = reader.ReadS32LE();
                stripOffsets[i] = offs - stripCount * 4 - 4; // Strip offsets are relative to offset 6
            }
            uint dataSize = (uint)(smapChunk.Size - stripCount * 4 - 10);

            byte[] source;
            reader.Read(dataSize, out source);

            return MMucusImageDecoder.DecodeStrips(info, source, buffer, stripOffsets);
        }

        public override bool CanDecode(Chunk chunk)
        {
            return (chunk.Parent != null && chunk.Parent.ChunkTypeId == "RO");
        }
    }
}
