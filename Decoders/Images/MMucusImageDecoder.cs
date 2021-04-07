using System;
using SCUMMRevLib.Utils;

namespace SCUMMRevLib.Decoders.Images
{
    public class MMucusImageDecoder
    {
        public static void DecodeBomp(ImageInfo info, byte[] source, byte[] dest)
        {
            uint nextOffset = 0;
            uint destOffset = 0;
            int linesLeft = info.Height;

            while (linesLeft > 0)
            {
                uint sourceOffset = nextOffset;
                nextOffset = sourceOffset + (uint)(source[sourceOffset] | source[sourceOffset + 1] << 8) + 2;
                sourceOffset += 2;

                DecodeBompLine(source, dest, sourceOffset, destOffset, info.Width);

                destOffset += (uint)info.Width;
                linesLeft--;
            }
        }

        private static void DecodeBompLine(byte[] source, byte[] dest, uint sourceOffset, uint destOffset, int count)
        {
            while (count > 0)
            {
                byte code = source[sourceOffset++];
                int runLength = (code >> 1) + 1;
                if (runLength > count) runLength = count;
                count -= runLength;
                if ((code & 1) != 0)
                {
                    byte currPen = source[sourceOffset++];
                    while (runLength > 0)
                    {
                        dest[destOffset++] = currPen;
                        runLength--;
                    }
                }
                else
                {
                    while (runLength > 0)
                    {
                        dest[destOffset++] = source[sourceOffset++];
                        runLength--;
                    }
                }
            }
        }

        public static byte[] DecodeStrips(ImageInfo info, byte[] source, byte[] dest, int[] stripOffsets)
        {
            int stripCount = stripOffsets.Length;
            for (int strip = 0; strip < stripCount; strip++ )
            {
                int pos = stripOffsets[strip];
                int codec = source[pos++];
                if (codec <= 10)
                {
                    switch (codec)
                    {
                        case 1: // Uncompressed
                            DecodeUncompressed(dest, info.Width, info.Height, strip*8, source, pos, false);
                            break;
                        case 2: // Simple run length encoding
                            DecodeRLEV(dest, info.Width, info.Height, strip*8, source, pos, false);
                            break;
                        case 3: // Bitstream - Run length encoding with palette rows
                            DecodeRLEPaletteRowsV(dest, info.Width, info.Height, strip*8, source, pos, false);
                            break;
                        case 4: // Bitstream - Run length encoding with local palette
                            DecodeRLELocalPaletteV(dest, info.Width, info.Height, strip*8, source, pos, false);
                            break;
                        case 7: // Bitstream - Old zigzag codec
                            DecodeOldZigZagV(dest, info.Width, info.Height, strip*8, source, pos, false);
                            break;
                        case 10: // Amiga/EGA compression
                            // TODO: Implement this
                            break;
                    }
                }
                else
                {
                    int indexSize = codec % 10;

                    codec = codec - indexSize;
                    
                    switch (codec)
                    {
                            // Zigzag (Huffman with subtraction variable):
                        case 10: // 0x0e-0x12
                            DecodeZigzagV(dest, info.Width, info.Height, strip * 8, source, pos, indexSize, false);
                            break;
                        case 20: // 0x18-0x1c
                            DecodeZigzagH(dest, info.Width, info.Height, strip * 8, source, pos, indexSize, false);
                            break;
                        case 30: // 0x22-0x26
                            DecodeZigzagV(dest, info.Width, info.Height, strip * 8, source, pos, indexSize, true);
                            break;
                        case 40: // 0x2c-0x30
                            DecodeZigzagH(dest, info.Width, info.Height, strip * 8, source, pos, indexSize, true);
                            break;
                            // MajMin (Huffman deltas + RLE):
                        case 60: // 0x40-0x44
                            DecodeMajMinH(dest, info.Width, info.Height, strip * 8, source, pos, indexSize, false);
                            break;
                        case 80: // 0x54-0x58
                            DecodeMajMinH(dest, info.Width, info.Height, strip * 8, source, pos, indexSize, true);
                            break;
                        case 100: // 0x68-0x6c
                            DecodeMajMinH(dest, info.Width, info.Height, strip * 8, source, pos, indexSize, true);
                            break;
                        case 120: // 0x7c-0x80
                            DecodeMajMinH(dest, info.Width, info.Height, strip * 8, source, pos, indexSize, false);
                            break;
                        default:
                            throw new DecodingException("Unknown codec (this should never happen): {0}", codec);
                    }
                }
            }

            return dest;
        }

        // Uncompressed byte color indices
        // Vertical pixel order
        private static void DecodeUncompressed(byte[] buffer, int width, int height, int stripX, byte[] source, int sourcePos, bool transparent)
        {
            int x = stripX;
            int y = 0;
            while (x < stripX + 8)
            {
                while (y < height)
                {
                    buffer[y*width + x] = source[sourcePos++];
                    y++;
                }
                y = 0;
                x++;
            }
        }

        // Simple run length encoding.
        // 1 byte run length followed by 1 byte palette index
        // Vertical pixel order
        private static void DecodeRLEV(byte[] buffer, int width, int height, int stripX, byte[] source, int sourcePos, bool transparent)
        {
            int x = stripX;
            int y = 0;
            int runlength = 0;
            byte color = 0;
            while (x < stripX + 8)
            {
                while (y < height)
                {
                    if (runlength > 0)
                    {
                        runlength--;
                        buffer[y*width + x] = color;
                        y++;
                        continue;
                    }
                    runlength = source[sourcePos++];
                    color = source[sourcePos++];
                }
                y = 0;
                x++;
            }
        }

        // Palette indices point to a current palette row (containing 16 indices) and are 4 bits
        // 2 bits: run length
        // 2 bits: mode:
        //      0: color index = 4 bits
        //         run length = run length + 2
        //      1: read new color index (4 bits) each iteration
        //         run length = run length + 1
        //      2: change paletteRow = 4 bits
        //
        // Vertical pixel order
        private static void DecodeRLEPaletteRowsV(byte[] buffer, int width, int height, int stripX, byte[] source, int sourcePos, bool transparent)
        {
            int x = stripX;
            int y = 0;
            int code = -1;
            byte color = 0;
            int paletteRow = 0;
            int runlength = 0;

            LEBitStream sourceStream = new LEBitStream(source, sourcePos);
            while (x < stripX + 8)
            {
                while (y < height)
                {
                    if (runlength > 0)
                    {
                        if (code >> 2 == 1)
                        {
                            color = (byte)sourceStream.GetBits(4);
                        }
                        runlength--;
                        buffer[y * width + x] = (byte)(paletteRow * 16 + color);
                        y++;
                        continue;
                    }
                    code = sourceStream.GetBits(4);
                    switch (code >> 2)
                    {
                        case 0:
                            color = (byte)sourceStream.GetBits(4);
                            runlength = ((code & 3) + 2);
                            break;
                        case 1:
                            runlength = ((code & 3) + 1);
                            break;
                        case 2:
                            paletteRow = sourceStream.GetBits(4);
                            break;
                    }
                }
                y = 0;
                x++;
            }
        }


        // 1 byte: local palette size (n)
        // n bytes: palette indices for local palette
        //
        // 1 byte: mode:
        //      a: Within local palette range: set pixel
        //      b: run length = mode - local palette size + 1
        //         1 byte: global palette index to write in [run length] pixels
        //
        // Vertical pixel order
        private static void DecodeRLELocalPaletteV(byte[] buffer, int width, int height, int stripX, byte[] source, int sourcePos, bool transparent)
        {
            int x = stripX;
            int y = 0;
            byte color = 0;
            int runlength = 0;

            byte[] localPalette = new byte[256];
            int numColors = source[sourcePos++];
            for (int i = 0; i < numColors; i++)
            {
                localPalette[i] = source[sourcePos++];
            }

            while (x < stripX + 8)
            {
                while (y < height)
                {
                    if (runlength > 0)
                    {
                        buffer[y*width + x] = color;
                        y++;
                        runlength--;
                        continue;
                    }

                    color = source[sourcePos++];
                    if (color < numColors)
                    {
                        buffer[y*width + x] = localPalette[color];
                        y++;
                    }
                    else
                    {
                        runlength = color - numColors + 1;
                        color = source[sourcePos++];
                    }
                }
                y = 0;
                x++;
            }
        }

        // Predecessor of ZigZag codec
        // Vertical pixel order
        private static void DecodeOldZigZagV(byte[] buffer, int width, int height, int stripX, byte[] source, int sourcePos, bool transparent)
        {
            int x = stripX;
            int y = 0;
            int sub = 1;

            byte color = source[sourcePos++];

            LEBitStream sourceStream = new LEBitStream(source, sourcePos);
            while (x < stripX + 8)
            {
                while (y < height)
                {
                    buffer[y*width + x] = color;
                    y++;

                    if (sourceStream.GetBit() == 0) // 0
                    {
                        // do nothing (draw pixel, same color)
                    }
                    else if (sourceStream.GetBit() == 0) // 10
                    {
                        sub = -sub;
                        color = (byte) (color - sub);
                    }
                    else if (sourceStream.GetBit() == 0) // 110
                    {
                        color = (byte) (color - sub);
                    }
                    else // 111
                    {
                        sub = 1;
                        color = (byte)sourceStream.GetBits(8);
                    }
                }
                y = 0;
                x++;
            }
        }

        private static void DecodeZigzagV(byte[] buffer, int width, int height, int stripX, byte[] source, int sourcePos, int indexSize, bool transparent)
        {
            int sub = 1;
            LEBitStream sourceStream = new LEBitStream(source, sourcePos);
            byte color = sourceStream.GetByte();
            int x = stripX;
            int y = 0;

            while (x < stripX + 8)
            {
                while (y < height)
                {
                    buffer[y*width + x] = color;

                    if (sourceStream.GetBit() == 0) // 0
                    {
                        // Do nothing (draw pixel, same color)
                    }
                    else if (sourceStream.GetBit() == 0) // 10
                    {
                        color = (byte)(sourceStream.GetBits(indexSize));
                        sub = 1;
                    }
                    else if (sourceStream.GetBit() == 0) // 110
                    {
                        color = (byte)(color - sub);
                    }
                    else // 111
                    {
                        sub = -sub;
                        color = (byte)(color - sub);
                    }
                    y++;
                }
                y = 0;
                x++;
            }
        }

        private static void DecodeZigzagH(byte[] buffer, int width, int height, int stripX, byte[] source, int sourcePos, int indexSize, bool transparent)
        {
            int sub = 1;
            LEBitStream sourceStream = new LEBitStream(source, sourcePos);
            byte color = sourceStream.GetByte();
            int x = stripX;
            int y = 0;

            while (y < height)
            {
                while (x < stripX + 8)
                {
                    buffer[y * width + x] = color;

                    if (sourceStream.GetBit() == 0) // 0
                    {
                        // Do nothing (draw pixel, same color)
                    }
                    else if (sourceStream.GetBit() == 0) // 10
                    {
                        color = (byte)(sourceStream.GetBits(indexSize));
                        sub = 1;
                    }
                    else if (sourceStream.GetBit() == 0) // 110
                    {
                        color = (byte) (color - sub);
                    }
                    else // 111
                    {
                        sub = -sub;
                        color = (byte)(color - sub);
                    }
                    x++;
                }
                x = stripX;
                y++;
            }
        }

        private static void DecodeMajMinH(byte[] buffer, int width, int height, int stripX, byte[] source, int sourcePos, int indexSize, bool transparent)
        {
            int runlength = 0;
            LEBitStream sourceStream = new LEBitStream(source, sourcePos);
            byte color = sourceStream.GetByte();
            int x = stripX;
            int y = 0;

            while (y < height)
            {
                while (x < stripX + 8)
                {
                    buffer[y * width + x] = color;
                    x++;
                    if (runlength > 0)
                    {
                        runlength--;
                        continue;
                    }

                    if (sourceStream.GetBit() == 0) // 0
                    {
                        
                    }
                    else if (sourceStream.GetBit() == 0) // 10
                    {
                        color = (byte)(sourceStream.GetBits(indexSize));
                    }
                    else // 11
                    {
                        int code = sourceStream.GetBits(3) - 4;
                        if (code != 0)
                        {
                            color = (byte)(color + code);
                        }
                        else
                        {
                            runlength = sourceStream.GetBits(8) - 1;
                        }
                    }
                }
                x = stripX;
                y++;
            }
        }
    }
}
