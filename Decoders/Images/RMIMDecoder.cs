using System;
using Katana.IO;
using SCUMMRevLib.Chunks;
using SCUMMRevLib.Decoders.Palettes;
using SCUMMRevLib.Utils;

namespace SCUMMRevLib.Decoders.Images
{
    [DecodesChunks("RMIM")]
    public class RMIMDecoder : BaseImageDecoder
    {
        private static readonly string[] PALETTE_PATHS = { "CLUT", "PALS/WRAP/APAL" };

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
            Chunk paletteChunk = null;
            foreach (string path in PALETTE_PATHS)
            {
                paletteChunk = roomChunk.SelectSingle(path);
                if (paletteChunk != null) break;
            }

            if (paletteChunk == null)
            {
                throw new DecodingException("Palette chunk not found");
            }

            // TODO: Check if this should use global decoders
            StandardPaletteDecoder palDecoder = new StandardPaletteDecoder();
            info.Palette = palDecoder.Decode(paletteChunk);
        }


        public override byte[] Decode(Chunk chunk, uint index)
        {
            if (GetCount(chunk) <= index)
            {
                throw new DecodingException("Invalid image index");
            }

            ImageInfo info = GetInfo(chunk, index);

            bool bomp = false;

            string imBlockName = String.Format("IM{0:X2}", index);

            Chunk imChunk = chunk.SelectSingle(imBlockName);

            if (imChunk == null)
            {
                throw new DecodingException("{0} chunk not found", imBlockName);
            }

            Chunk dataChunk = imChunk.SelectSingle("SMAP");
            if (dataChunk == null)
            {
                bomp = true;
                dataChunk = imChunk.SelectSingle("BOMP");
            }

            if (dataChunk == null)
            {
                throw new DecodingException("BOMP or SMAP chunk not found");
            }

            byte[] destBuffer = bomp ? DecodeBomp(dataChunk, info) : DecodeSmap(dataChunk, info);

            return destBuffer;
        }

        protected byte[] DecodeBomp(Chunk dataChunk, ImageInfo info)
        {
            byte[] dest = new byte[info.Width * info.Height];

            BinReader reader = dataChunk.GetReader();

            reader.Position = 18;
            byte[] source;
            reader.Read(dataChunk.Size - 18, out source);

            MMucusImageDecoder.DecodeBomp(info, source, dest);

            return dest;
        }

        protected byte[] DecodeSmap(Chunk dataChunk, ImageInfo info)
        {
            byte[] buffer = new byte[info.Width*info.Height];

            int stripCount = info.Width / 8;

            int[] stripOffsets = new int[stripCount];

            BinReader reader = dataChunk.GetReader();
            reader.Position = 8;
            for (int i = 0; i < stripCount; i++)
            {
                int offs = reader.ReadS32LE();
                stripOffsets[i] = offs - 8 - 4 * stripCount;
            }
            uint dataOffset = (uint)(8 + 4 * stripCount);
            uint dataSize = dataChunk.Size - dataOffset;

            reader.Position = dataOffset;
            byte[] source;
            reader.Read(dataSize, out source);

            return MMucusImageDecoder.DecodeStrips(info, source, buffer, stripOffsets);
        }

        public override bool CanDecode(Chunk chunk)
        {
            return (chunk.Parent != null && chunk.Parent.ChunkTypeId == "ROOM");
        }
    }
}
