using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Katana.IO;
using SCUMMRevLib.Chunks;
using SCUMMRevLib.Decoders.Images;
using SCUMMRevLib.Decoders.Palettes;
using SCUMMRevLib.Utils;

namespace SCUMMRevLib.Decoders.Video
{
    [DecodesChunks("ANIM")]
    public class CMISMUSHDecoder : BaseVideoDecoder
    {
        private ChunkList frmeChunks;
        private int currentFrame;

        private bool storeCurrentFrame;
        private byte[] frameStore;
        private bool frameInStore;
        private VideoInfo info;

        private Palette currentPalette;
        private readonly byte[] paletteBuffer = new byte[768];
        private readonly ushort[] transitionPalette = new ushort[768];
        private readonly byte[] transitionBuffer = new byte[2*768];


        public override VideoInfo GetInfo(Chunk chunk)
        {
            VideoInfo info = new VideoInfo();
            Chunk ahdrChunk = chunk.SelectSingle("AHDR");
            BinReader reader = ahdrChunk.GetReader();
            reader.Position = 10;
            info.FrameCount = reader.ReadU16LE();
            info.Width = 640;
            info.Height = 480;
            info.FrameRate = 12;
            info.PixelFormat = PixelDepth.Depth8;
            
            return info;
        }

        public override void Initialize(Chunk chunk)
        {
            frmeChunks = chunk.Select("FRME");
            currentFrame = 0;
            currentPalette = new Palette(256);

            info = GetInfo(chunk);
            // TODO: Make work for other pixel depths than 8 bit?
            
            frameStore = new byte[info.Width * info.Height];
            frameInStore = false;

            Chunk ahdrChunk = chunk.SelectSingle("AHDR");
            HandleAHDRPalette(ahdrChunk);
        }

        public override bool CanDecode(Chunk chunk)
        {
            return true;
        }

        public override void DecodeFrame(byte[] buffer)
        {
            Chunk frmeChunk = frmeChunks[currentFrame];
            ChunkList subChunks = frmeChunk.Select("*");

            foreach (Chunk subChunk in subChunks)
            {
                switch (subChunk.ChunkTypeId)
                {
                    case "FOBJ":
                        HandleFOBJ(subChunk, buffer);
                        break;
                    case "NPAL":
                        HandleNPAL(subChunk, buffer);
                        break;
                    case "XPAL":
                        HandleXPAL(subChunk, buffer);
                        break;
                    case "STOR":
                        HandleSTOR(subChunk, buffer);
                        break;
                    case "FTCH":
                        HandleFTCH(subChunk, buffer);
                        break;
                }
            }
        }

        private void HandleAHDRPalette(Chunk chunk)
        {
            BinReader reader = chunk.GetReader();
            reader.Position = 14;
            ReadPalette(reader);
        }

        private void HandleFOBJ(Chunk chunk, byte[] buffer)
        {
            BinReader reader = chunk.GetReader();
            reader.Position = 8;
            ushort codec = reader.ReadU16LE();
            ushort x = reader.ReadU16LE();
            ushort y = reader.ReadU16LE();
            ushort width = reader.ReadU16LE();
            ushort height = reader.ReadU16LE();
            ushort unknown1 = reader.ReadU16LE();
            ushort unknown2 = reader.ReadU16LE();

            uint inputSize = chunk.Size - 14;
            byte[] inputBuffer = new byte[inputSize];
            reader.Read(inputBuffer, 0, inputSize);

            switch (codec)
            {
                case 1:
                case 3:
                    DecodeSMUSH_1(inputBuffer, buffer, x, y, width, height);
                    break;
                case 37:
                    DecodeSMUSH_37(inputBuffer, buffer, x, y, width, height);
                    break;
                case 47:
                    DecodeSMUSH_47(inputBuffer, buffer, x, y, width, height);
                    break;
                default:
                    throw new DecodingException("Unknown SMUSH codec: {0}", codec);
            }

            if (storeCurrentFrame)
            {
                buffer.CopyTo(frameStore, 0);
                frameInStore = true;
                storeCurrentFrame = false;
            }
            inputBuffer = null;
        }

        private void HandleNPAL(Chunk chunk, byte[] buffer)
        {
            BinReader reader = chunk.GetReader();
            reader.Position = 8;
            ReadPalette(reader);
        }

        private void HandleXPAL(Chunk chunk, byte[] buffer)
        {
            BinReader reader = chunk.GetReader();
            reader.Position = 8;
            if (chunk.Size == 2316) // 2 * 768 words + 3 * 256 bytes + header
            {
                ushort unknown = reader.ReadU16LE();
                ushort unknown2 = reader.ReadU16LE();
                reader.Read(transitionBuffer, 0, 768*2);
                for (int i = 0; i < 768 * 2; i += 2)
                {
                    transitionPalette[i >> 1] = (ushort)(transitionBuffer[i] | (transitionBuffer[i + 1] << 8));
                }
                ReadPalette(reader);
            }
            else if (chunk.Size == 14)
            {
                // Three unknown words here
                int transitionIndex = 0;
                for (int i = 0; i < 256; i++)
                {
                    byte r = GetTransitionColor(currentPalette[i].R, transitionPalette[transitionIndex++]);
                    byte g = GetTransitionColor(currentPalette[i].G, transitionPalette[transitionIndex++]);
                    byte b = GetTransitionColor(currentPalette[i].B, transitionPalette[transitionIndex++]);
                    currentPalette[i] = new PaletteColor(r,g,b);
                }
            }
        }

        private void HandleSTOR(Chunk chunk, byte[] buffer)
        {
            storeCurrentFrame = true;
        }

        private void HandleFTCH(Chunk chunk, byte[] buffer)
        {
            if (frameInStore)
            {
                frameStore.CopyTo(buffer, 0);
            }
        }

        // Simple run length encoding (keyframes)
        private void DecodeSMUSH_1(byte[] input, byte[] output, ushort x, ushort y, ushort width, ushort height)
        {
            int outputPos = y*width + x;
            int inputPos = 0;

            for (int row = 0; row < height; row++)
            {
                int lineSize = (input[inputPos] | input[inputPos + 1] << 8);
                inputPos += 2;

                while (lineSize > 0)
                {
                    byte code = input[inputPos++];
                    lineSize--;
                    int runLength = (code >> 1) + 1;
                    if ((code & 1) != 0)
                    {
                        // Run length encoding
                        byte val = input[inputPos++];
                        lineSize--;
                        if (val != 0)
                        {
                            while (runLength -- > 0)
                            {
                                output[outputPos++] = val;
                            }
                        }
                    }
                    else
                    {
                        // Uncompressed
                        lineSize -= runLength;
                        while (runLength-- > 0)
                        {
                            byte val = input[inputPos++];
                            output[outputPos++] = val;
                        }
                    }
                }
                outputPos += info.Width - width; // Full screen width - frame width
            }
        }

        private void DecodeSMUSH_37(byte[] input, byte[] output, ushort x, ushort y, ushort width, ushort height)
        {

        }

        private void DecodeSMUSH_47(byte[] input, byte[] output, ushort x, ushort y, ushort width, ushort height)
        {

        }

        private void ReadPalette(BinReader reader)
        {
            reader.Read(paletteBuffer, 0, 768); // 3 * 256 as usual

            int i = 0;
            int paletteIndex = 0;
            while (i < 768)
            {
                byte r = paletteBuffer[i++];
                byte g = paletteBuffer[i++];
                byte b = paletteBuffer[i++];
                PaletteColor color = new PaletteColor(r, g, b);
                currentPalette[paletteIndex++] = color;
            }
        }

        private static byte GetTransitionColor(byte colorComponent, ushort delta)
        {
            int color = (colorComponent * 129 + delta) / 128;
            if (color < 0) color = 0;
            if (color > 255) color = 255;
            return (byte)color;
        }
    }
}
