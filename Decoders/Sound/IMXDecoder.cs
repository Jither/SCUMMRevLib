using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Katana.IO;
using SCUMMRevLib.Chunks;

namespace SCUMMRevLib.Decoders.Sound
{
    internal class BlockCodecInfo
    {
        public uint Offset { get; set; }
        public uint CompressedSize { get; set; }
        public int Codec { get; set; }
    }

    [DecodesChunks(".imx")]
    public class IMXDecoder : BaseSoundDecoder
    {
        private int nextBlock;
        private uint lastBlockSize;
        private readonly byte[] inputBuffer = new byte[8192];
        private readonly byte[] outputBuffer = new byte[8192];
        private List<BlockCodecInfo> codecInfo;

        public override SoundInfo GetInfo(Chunk chunk)
        {
            uint bits, sampleRate, channels;
            GetFormat(chunk, out bits, out sampleRate, out channels);
            if (bits == 12)
            {
                bits = 16;
            }
            SoundInfo info = new SoundInfo(channels, sampleRate, bits);
            return info;
        }

        protected void GetFormat(Chunk chunk, out uint bits, out uint sampleRate, out uint channels)
        {
            Chunk frmtChunk = chunk.SelectSingle("iMUS/MAP /FRMT");
            BinReader reader = frmtChunk.GetReader();
            reader.Position = 16;
            bits = reader.ReadU32BE();
            sampleRate = reader.ReadU32BE();
            channels = reader.ReadU32BE();
        }

        protected override uint DecodePacket(out byte[] packet)
        {
            if (nextBlock >= codecInfo.Count)
            {
                // No more data
                packet = null;
                return 0;
            }

            packet = outputBuffer;
            BlockCodecInfo info = codecInfo[nextBlock];
            nextBlock++;

            BinReader reader = currentChunk.GetReader(); // .IMX file
            reader.Position = info.Offset;

            reader.Read(inputBuffer, 0, info.CompressedSize);
            uint result;
            switch (info.Codec)
            {
                case 0x00: // Uncompressed
                    result = DecodeUncompressed(inputBuffer, packet, info.CompressedSize);
                    break;
                case 0x01:
                case 0x02: // 
                case 0x03: // 
                case 0x0A: //
                case 0x0B: //
                case 0x0C: //
                    packet = null;
                    result = 0;
                    break;
                case 0x0D: //
                    result = DecodeVIMA(inputBuffer, packet, info.CompressedSize, 1);
                    break;
                case 0x0F: //
                    result = DecodeVIMA(inputBuffer, packet, info.CompressedSize, 2);
                    break;
                default:
                    throw new DecodingException("Unsupported IMC codec: {0}", info.Codec);
            }
            if (nextBlock >= codecInfo.Count)
            {
                result = lastBlockSize;
            }
            return result;
        }

        protected uint DecodeUncompressed(byte[] input, byte[] output, uint size)
        {
            Array.Copy(input, 0, output, 0, size);
            return size;
        }

        protected uint DecodeVIMA(byte[] input, byte[] output, uint size, int channels)
        {
            int[] startTablePos = {0, 0};
            uint[] startTableEntry = {7, 7};
            uint[] startOutput = {0, 0};

            int samplesLeft = 0x1000;

            int sourceOffs = 0;

            ushort uncompressedDataLength;

            using (MemoryStream stream = new MemoryStream(input))
            {
                using (BinReader reader = new BinReader(stream))
                {
                    uncompressedDataLength = reader.ReadU16BE();
                    sourceOffs += 2;
                    if (uncompressedDataLength != 0)
                    {
                        // Just ignore the uncompressed data - it's the iMUS header
                        //reader.Read(output, 0, uncompressedDataLength);
                        samplesLeft -= uncompressedDataLength/2;
                        //destOffs += uncompressedDataLength;
                        sourceOffs += uncompressedDataLength;
                    }
                    else
                    {
                        // Read seed values (i.e. end values from last block):
                        for (int i = 0; i < channels; i++)
                        {
                            startTablePos[i] = reader.ReadU8();
                            startTableEntry[i] = reader.ReadU32BE();
                            startOutput[i] = reader.ReadU32BE();
                            sourceOffs += 9;
                        }
                    }

                    // Decode each channel:
                    int totalBitOffset = 0;
                    for (int channel = 0; channel < channels; channel++)
                    {
                        int tablePos = startTablePos[channel];
                        int outputWord = (int) startOutput[channel];

                        int destPos = 2*channel;

                        int sampleCount = (channels == 1)
                            ? samplesLeft
                            : (channel == 0) ? (samplesLeft + 1)/2 : samplesLeft/2;

                        for (int i = 0; i < sampleCount; i++)
                        {
                            // Read bits based on current bit table value:
                            int bitCount = IMC_BIT_COUNTS[tablePos];
                            reader.Position = (uint) (sourceOffs + (totalBitOffset >> 3));
                            ushort readWord = (ushort) (reader.ReadU16BE() << (totalBitOffset & 0x07));
                            byte compressedSample = (byte) (readWord >> (16 - bitCount));

                            totalBitOffset += bitCount;

                            byte signBitMask = (byte) (1 << (bitCount - 1)); // mask for determining sign
                            byte dataBitMask = (byte) (signBitMask - 1); // mask for determining data

                            byte data = (byte) (compressedSample & dataBitMask);

                            // Read delta from table:
                            //int delta = (IMC_DELTA_TABLE[tablePos]*(2*data + 1)) >> (bitCount - 1);
                            int tableIndex = (data << (7 - bitCount)) + (tablePos << 6);
                            int delta = (IMC_DELTA_TABLE[tablePos] >> (bitCount - 1)) + IMC_VALUE_TABLE[tableIndex];

                            // Apply sign:
                            if ((signBitMask & compressedSample) != 0)
                            {
                                delta = -delta;
                            }

                            // Apply delta:
                            outputWord += delta;

                            // Clip to signed 16 bit sample:
                            if (outputWord > 0x7fff) outputWord = 0x7fff;
                            if (outputWord < -0x8000) outputWord = -0x8000;

                            output[destPos] = (byte) (outputWord & 0xff);
                            output[destPos + 1] = (byte) (outputWord >> 8);

                            destPos += channels*2;

                            tablePos += IMC_NAV_TABLE[bitCount - 2][data];
                            if (tablePos < 0)
                            {
                                tablePos = 0;
                            }
                            else if (tablePos >= IMC_DELTA_TABLE.Length)
                            {
                                tablePos = IMC_DELTA_TABLE.Length - 1;
                            }
                        }
                    }
                }
            }
            return (uint)(8192 - uncompressedDataLength); // The uncompressed data is solely used for the iMUS header, which we want to skip here
        }

        protected void ReadCompressionTable(Chunk compChunk)
        {
            codecInfo = new List<BlockCodecInfo>();
            BinReader reader = compChunk.GetReader();
            reader.Position = 4;
            uint count = reader.ReadU32BE();
            reader.Position = 12;
            lastBlockSize = reader.ReadU32BE();
            reader.Position = 16;
            for (int i = 0; i < count; i++)
            {
                BlockCodecInfo info = new BlockCodecInfo();
                info.Offset = reader.ReadU32BE();
                info.CompressedSize = reader.ReadU32BE();
                info.Codec = reader.ReadS32BE();
                uint skip = reader.ReadU32BE();
                Debug.Assert(skip == 0, "Found unknown codec info != 0");
                codecInfo.Add(info);
            }
        }

        public override void Initialize(Chunk chunk)
        {
            base.Initialize(chunk);
            uint bitsPerSample, sampleRate, channels;
            GetFormat(chunk, out bitsPerSample, out sampleRate, out channels);
            Chunk compChunk = chunk.SelectSingle("COMP");
            ReadCompressionTable(compChunk);

            nextBlock = 0;
        }

        public override bool CanDecode(Chunk chunk)
        {
            return true;
        }


        static IMXDecoder()
        {
            // Initialize bit count table:
            IMC_BIT_COUNTS = new int[IMC_DELTA_TABLE.Length];

            for (int i = 0; i < IMC_DELTA_TABLE.Length; i++)
            {
                byte value = 0;
                int tableValue = ((IMC_DELTA_TABLE[i] * 4) / 7) / 2;
                while (tableValue != 0)
                {
                    tableValue /= 2;
                    value++;
                }
                if (value < 2)
                {
                    value = 2;
                }
                if (value > 7)
                {
                    value = 7;
                }
                IMC_BIT_COUNTS[i] = value;
            }

            IMC_VALUE_TABLE = new int[0x40 * IMC_DELTA_TABLE.Length];
            for (int n = 0; n <= 0x3f; n++)
            {
                int destTablePos = n;
                foreach (int t in IMC_DELTA_TABLE)
                {
                    int mask = 32;
                    int put = 0;
                    int tableValue = t;
                    do
                    {
                        if ((mask & n) != 0)
                        {
                            put += tableValue;
                        }
                        mask >>= 1;
                        tableValue >>= 1;
                    } while (mask != 0);
                    IMC_VALUE_TABLE[destTablePos] = put;
                    destTablePos += 0x40;
                }
            }
        }

        // VIMA IMC tables:
        private static readonly int[] IMC_BIT_COUNTS;

        private static readonly int[] IMC_VALUE_TABLE;

        private static readonly int[] IMC_DELTA_TABLE = {
                                                   7, 8, 9, 10, 11, 12, 13, 14, 16, 17,
                                                   19, 21, 23, 25, 28, 31, 34, 37, 41, 45,
                                                   50, 55, 60, 66, 73, 80, 88, 97, 107, 118,
                                                   130, 143, 157, 173, 190, 209, 230, 253, 279, 307, 
                                                   337, 371, 408, 449, 494, 544, 598, 658, 724, 796,
                                                   876, 963, 1060, 1166, 1282, 1411, 1552, 1707, 1878, 2066,
                                                   2272, 2499, 2749, 3024,3327, 3660, 4026, 4428, 4871, 5358,
                                                   5894, 6484, 7132, 7845, 8630, 9493, 10442, 11487, 12635, 13899,
                                                   15289, 16818, 18500, 20350, 22385, 24623, 27086, 29794, 32767,
                                               };

        private static readonly int[][] IMC_NAV_TABLE = new [] {
            new [] { -1, 
                      4 },
            new [] { -1, -1, 
                      2,  8 },
            new [] { -1, -1, -1, -1, 
                      1,  2,  4,  6 },
            new [] { -1, -1, -1, -1, -1, -1, -1, -1, 
                      1,  2,  4,  6,  8, 12, 16, 32 },
            new [] { -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
                      1,  2,  4,  6,  8, 10, 12, 14, 16, 18, 20, 22, 24, 26, 28, 32},
            new [] { -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
		              1,  2,  3,  4,  5,  6,  7,  8,  9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32}
        };
    }
}
