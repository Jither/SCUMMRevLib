using Katana.IO;
using SCUMMRevLib.Chunks;
using SCUMMRevLib.FileFormats;
using SCUMMRevLib.Utils;

namespace SCUMMRevLib.Decoders.Images
{
    [DecodesChunks(".bm")]
    public class GrimeBMImageDecoder : BaseImageDecoder
    {
        private const int LZ_THRESHOLD = 2;

        public override uint GetCount(Chunk chunk)
        {
            BinReader reader = chunk.GetReader();
            reader.Position = 16;
            return reader.ReadU32LE();
        }

        public override ImageInfo GetInfo(Chunk chunk, uint index)
        {
            uint count = GetCount(chunk);
            if (index < 0 || index > count)
            {
                throw new DecodingException("Image index out of range.");
            }

            ImageInfo info = new ImageInfo();

            BinReader reader = chunk.GetReader();
            
            // We need the codec to find the width/height later on:
            reader.Position = 8;
            uint codec = reader.ReadU32LE();

            reader.Position = 20;
            info.X = reader.ReadS32LE();
            info.Y = reader.ReadS32LE();
            
            reader.Position = 36;
            uint bitsPerPixel = reader.ReadU32LE();

            switch (bitsPerPixel)
            {
                case 16:
                    info.PixelFormat = PixelDepth.Depth16;
                    break;
                default:
                    throw new DecodingException("Unsupported bitdepth: {0}", bitsPerPixel);
            }


            uint bytesPerPixel = bitsPerPixel/8;

            uint dimensionsOffset = 128;

            uint width = 0;
            uint height = 0;

            for (uint i = 0; i <= index; i++)
            {
                reader.Position = dimensionsOffset;
                width = reader.ReadU32LE();
                height = reader.ReadU32LE();

                if (codec == 0) // uncompressed
                {
                    // Add image size + size of header (width, height)
                    dimensionsOffset += width*height*bytesPerPixel + 8;
                }
                else
                {
                    // Add read size + size of header (compressed size, width, height)
                    dimensionsOffset += reader.ReadU32LE() + 12;
                }
            }
            info.Width = (int)width;
            info.Height = (int)height;

            return info;
        }

        public override byte[] Decode(Chunk chunk, uint index)
        {
            uint count = GetCount(chunk);
            if (index < 0 || index > count)
            {
                throw new DecodingException("Image index out of range.");
            }

            BinReader reader = chunk.GetReader();

            reader.Position = 36;
            uint bytesPerPixel = reader.ReadU32LE() / 8;

            reader.Position = 8;
            uint codec = reader.ReadU32LE();

            byte[] destBuffer;
            // TODO: Read input buffer here
            switch (codec)
            {
                case 0: // Uncompressed
                    destBuffer = DecodeUncompressed(reader, index, 128, bytesPerPixel);
                    break;
                case 3: // LZSS
                    destBuffer = DecodeLZSS(reader, index, 128, bytesPerPixel);
                    break;
                default:
                    throw new DecodingException("Unsupported codec: {0}", codec);
            }
            return destBuffer;
        }

        private byte[] DecodeLZSS(BinReader reader, uint index, uint pos, uint bytesPerPixel)
        {
            uint width = 0;
            uint height = 0;
            uint compressedSize = 0;

            // Find image data at index:
            for (uint n = 0; n <= index; n++)
            {
                reader.Position = pos;
                width = reader.ReadU32LE();
                height = reader.ReadU32LE();
                compressedSize = reader.ReadU32LE();
                pos += compressedSize + 12;
            }

            int buffersize = (int)(width * height * bytesPerPixel);
            byte[] destBuffer = new byte[buffersize];

            byte[] sourceBuffer;
            reader.Read(compressedSize, out sourceBuffer);

            WordBitStream bits = new WordBitStream(sourceBuffer);
            //ByteBitStream bits = new ByteBitStream(sourceBuffer);
            int lzSpan = 0;
            int lzLength = 0;
            int destPos = 0;
            while (true)
            {
                if (bits.GetBit() == 1)
                {
                    destBuffer[destPos++] = bits.GetByte();
                }
                else
                {
                    if (bits.GetBit() == 0)
                    {
                        lzLength = bits.GetBit() << 1;
                        lzLength = (lzLength | bits.GetBit()) + LZ_THRESHOLD + 1;
                        lzSpan = (int) (bits.GetByte() | 0xffffff00);
                    }
                    else
                    {
                        lzSpan = bits.GetByte();
                        lzLength = bits.GetByte();
                        lzSpan |= (int)(0xfffff000 + ((lzLength & 0xf0) << 4));
                        lzLength = (lzLength & 0x0f) + LZ_THRESHOLD + 1;

                        if (lzLength == LZ_THRESHOLD + 1)
                        {
                            lzLength = bits.GetByte() + 1;
                            if (lzLength == 1) break;
                        }
                    }
                }

                int copyPos = destPos + lzSpan;
                while (lzLength > 0)
                {
                    destBuffer[destPos++] = destBuffer[copyPos++];
                    lzLength--;
                }
            }
            return destBuffer;
        }

        private byte[] DecodeUncompressed(BinReader reader, uint index, uint pos, uint bytesPerPixel)
        {
            uint width = 0;
            uint height = 0;

            // Find image data at index:
            for (uint n = 0; n <= index; n++)
            {
                reader.Position = pos;
                width = reader.ReadU32LE();
                height = reader.ReadU32LE();
                pos += width*height*bytesPerPixel + 8;
            }
            uint buffersize = width*height*bytesPerPixel;
            
            byte[] buffer;
            reader.Read(buffersize, out buffer);
            
            return buffer;
        }

        public override bool CanDecode(Chunk chunk)
        {
            SRFile file = chunk.File;
            file.Position = chunk.Offset;
            return file.ReadFourCC() == "BM  ";
        }
    }
}
