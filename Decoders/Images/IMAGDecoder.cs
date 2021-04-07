using System;
using Katana.IO;
using SCUMMRevLib.Chunks;
using SCUMMRevLib.Decoders.Palettes;
using SCUMMRevLib.FileFormats;
using SCUMMRevLib.Utils;

namespace SCUMMRevLib.Decoders.Images
{
    [DecodesChunks("IMAG")]
    public class IMAGDecoder : BaseImageDecoder
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
            Chunk paletteChunk = roomChunk.SelectSingle("PALS/WRAP/APAL");

            if (paletteChunk == null)
            {
                throw new DecodingException("Palette chunk not found");
            }

            // TODO: Check if this should be global
            StandardPaletteDecoder paletteDecoder = new StandardPaletteDecoder();
            info.Palette = paletteDecoder.Decode(paletteChunk);
        }


        public override byte[] Decode(Chunk chunk, uint index)
        {
            if (GetCount(chunk) <= index)
            {
                throw new DecodingException("Invalid image index");
            }

            ImageInfo info = GetInfo(chunk, index);

            bool bomp = false; // TODO: Implement bomp

            ChunkList smapChunks = chunk.Select("WRAP/SMAP");

            Chunk dataChunk = smapChunks[(int)index];

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

        protected byte[] DecodeSmap(Chunk smapChunk, ImageInfo info)
        {
            byte[] buffer = new byte[info.Width*info.Height];

            int stripCount = info.Width / 8;

            Chunk offsChunk = smapChunk.SelectSingle("BSTR/WRAP/OFFS");

            int[] stripOffsets = new int[stripCount];

            BinReader reader = offsChunk.GetReader();
            reader.Position = 8;
            for (int i = 0; i < stripCount; i++)
            {
                int offs = reader.ReadS32LE();
                stripOffsets[i] = (int)(offs - offsChunk.Size);
            }
            uint dataSize = offsChunk.Parent.Size - offsChunk.Size - 8;

            SRFile file = smapChunk.File;
            file.Position = offsChunk.Offset + offsChunk.Size;
            byte[] source;
            file.Read(dataSize, out source);

            return MMucusImageDecoder.DecodeStrips(info, source, buffer, stripOffsets);
        }

        public override bool CanDecode(Chunk chunk)
        {
            return (chunk.Parent != null && chunk.Parent.ChunkTypeId == "ROOM");
        }
    }
}
