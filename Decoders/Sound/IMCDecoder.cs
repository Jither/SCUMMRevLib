using System;
using System.Collections.Generic;
using System.IO;
using Katana.IO;
using SCUMMRevLib.Chunks;

namespace SCUMMRevLib.Decoders.Sound
{
    public class MCMPBlockCodecInfo
    {
        public int Codec { get; set; }
        public uint Offset { get; set; }
        public uint UncompressedSize { get; set; }
        public uint CompressedSize { get; set; }
    }

    [DecodesChunks(".imc")]
    public class IMCDecoder : BaseSoundDecoder
    {
        private int nextBlock;

        // THINK 8192 bytes is the largest input buffer size...
        private readonly byte[] inputBuffer = new byte[8192];
        private byte[] outputBuffer;
        private List<MCMPBlockCodecInfo> codecInfo;
        private List<string> codecNames;

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

            MCMPBlockCodecInfo info = codecInfo[nextBlock];
            nextBlock++;

            BinReader reader = currentChunk.GetReader(); // .IMX file
            reader.Position = info.Offset;

            reader.Read(inputBuffer, 0, info.CompressedSize);
            uint result;

            if (info.Codec >= codecNames.Count)
            {
                throw new DecodingException("Invalid codec index: {0}", info.Codec);
            }

            switch (codecNames[info.Codec])
            {
                case "NULL": // Uncompressed
                    result = DecodeUncompressed(inputBuffer, packet, info);
                    break;
                case "VIMA": //
                    result = DecodeVIMA(inputBuffer, packet, info);
                    break;
                default:
                    throw new DecodingException("Unsupported IMC codec: {0}", codecNames[info.Codec]);
            }
            return result;
        }

        protected uint DecodeUncompressed(byte[] input, byte[] output, MCMPBlockCodecInfo info)
        {
            Array.Copy(input, 0, output, 0, info.UncompressedSize);
            return info.UncompressedSize;
        }

        protected uint DecodeVIMA(byte[] input, byte[] output, MCMPBlockCodecInfo info)
        {
            // A VIMA block in this (GF) version of the format is similar to the previous (CMI) format
            // with a few key differences:
            // - No "uncompressed data length" (the NULL compression block takes care of that)
            // - The unused table value initial state is removed
            // - The output initial state is now stored in a signed word (rather than unsigned dword)
            // - Stereo is indicated by a negated first channel table position initial state (i.e. 0x00 => 0xFF)
            // - The bit stream may include "keyframe" data, i.e. single uncompressed words, indicated by setting all LSB bits on, followed by the uncompressed word
            // - The bit count table and navigation tables have changed.

            int[] startTablePos = new int[2];
            int[] startOutput = new int[2];

            using (MemoryStream stream = new MemoryStream(input))
            {
                using (BinReader reader = new BinReader(stream))
                {
                    reader.Position = 0;

                    startTablePos[0] = reader.ReadU8();
                    startOutput[0] = reader.ReadS16BE();

                    int channels = 1;

                    if ((startTablePos[0] & 0x80) != 0)
                    {
                        // Stereo
                        channels = 2;
                        startTablePos[0] = (byte) (~ startTablePos[0]);
                        startTablePos[1] = reader.ReadU8();
                        startOutput[1] = reader.ReadS16BE();
                    }

                    int sampleCount = (int) info.UncompressedSize/(2*channels); // 16 bit
                    int bits = reader.ReadU16BE();
                    int bitptr = 0;

                    // Decode each channel:
                    for (int channel = 0; channel < channels; channel++)
                    {
                        int tablePos = startTablePos[channel];
                        int outputWord = startOutput[channel];

                        int destPos = 2*channel;

                        for (int i = 0; i < sampleCount; i++)
                        {
                            // Read bits based on current bit table value:
                            int bitCount = IMC_BIT_COUNTS[tablePos];
                            bitptr += bitCount;

                            byte signBitMask = (byte) (1 << (bitCount - 1)); // mask for determining sign
                            byte dataBitMask = (byte) (signBitMask - 1); // mask for determining data

                            byte compressedSample = (byte) ((bits >> (16 - bitptr)) & (signBitMask | dataBitMask));

                            // TODO: Put bitcrap into class
                            if (bitptr > 7) // Read more
                            {
                                bits = ((bits & 0xff) << 8) | reader.ReadU8();
                                bitptr -= 8;
                            }

                            if ((compressedSample & signBitMask) != 0)
                            {
                                compressedSample ^= signBitMask;
                            }
                            else
                            {
                                signBitMask = 0;
                            }

                            // If all bits are set, this is a "keyframe" - read one word of uncompressed data:
                            if (compressedSample == dataBitMask)
                            {
                                outputWord = (int) ((short) (bits << bitptr) & 0xffffff00);
                                bits = ((bits & 0xff) << 8) | reader.ReadU8();
                                outputWord |= ((bits >> (8 - bitptr)) & 0xff);
                                bits = ((bits & 0xff) << 8) | reader.ReadU8();
                            }
                            else
                            {
                                // Read delta from table:
                                int tableIndex = (compressedSample << (7 - bitCount)) | (tablePos << 6);
                                int delta = IMC_VALUE_TABLE[tableIndex];
                                if (compressedSample != 0)
                                {
                                    delta += (IMC_DELTA_TABLE[tablePos] >> (bitCount - 1));
                                }

                                // Apply sign:
                                if (signBitMask != 0)
                                {
                                    delta = -delta;
                                }

                                // Apply delta:
                                outputWord += delta;

                                // Clip to signed 16 bit sample:
                                if (outputWord > 0x7fff) outputWord = 0x7fff;
                                if (outputWord < -0x8000) outputWord = -0x8000;
                            }

                            output[destPos] = (byte) (outputWord & 0xff);
                            output[destPos + 1] = (byte) (outputWord >> 8);

                            destPos += channels*2;

                            tablePos += IMC_NAV_TABLE[bitCount - 2][compressedSample];

                            // Clip table position to table:
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
            return info.UncompressedSize;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="compChunk">MCMP chunk</param>
        /// <returns>Maximum decompressed size</returns>
        protected uint ReadCompressionTable(Chunk compChunk)
        {
            uint currentOffset = compChunk.Size; // Start of compressed (even if NULL-compressed) data

            codecInfo = new List<MCMPBlockCodecInfo>();
            BinReader reader = compChunk.GetReader();
            reader.Position = 4;
            int count = reader.ReadU16BE();
            uint maxUncompressedSize = 0;
            for (int i = 0; i < count; i++)
            {
                MCMPBlockCodecInfo info = new MCMPBlockCodecInfo();
                info.Codec = reader.ReadU8();
                info.UncompressedSize = reader.ReadU32BE();
                maxUncompressedSize = Math.Max(maxUncompressedSize, info.UncompressedSize);
                info.CompressedSize = reader.ReadU32BE();
                info.Offset = currentOffset;
                currentOffset += info.CompressedSize;
                codecInfo.Add(info);
            }

            int nameTableSize = reader.ReadU16BE();
            int readCount = 0;

            codecNames = new List<string>();
            while (readCount < nameTableSize)
            {
                string codecName = reader.ReadStringZ();
                codecNames.Add(codecName);
                readCount += codecName.Length + 1; // Plus 0 terminator
            }

            return maxUncompressedSize;
        }

        public override void Initialize(Chunk chunk)
        {
            base.Initialize(chunk);
            uint bitsPerSample, sampleRate, channels;
            GetFormat(chunk, out bitsPerSample, out sampleRate, out channels);

            Chunk compChunk = chunk.SelectSingle("MCMP");
            uint maxOutputSize = ReadCompressionTable(compChunk);
            outputBuffer = new byte[maxOutputSize];
            nextBlock = 1; // Skip first (header) block
        }

        public override bool CanDecode(Chunk chunk)
        {
            return true;
        }


        static IMCDecoder()
        {
            // Setup lookup table:
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
        private static readonly int[] IMC_BIT_COUNTS = {
            4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4,
            4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4,
            4, 4, 4, 4, 4, 4, 4, 4, 4, 5, 5, 5, 5, 5, 5, 5, 5, 5,
            5, 5, 5, 5, 5, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6,
            6, 6, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7                                                           
        };

        private static readonly int[] IMC_VALUE_TABLE;

        private static readonly int[] IMC_DELTA_TABLE = {
            7, 8, 9, 10, 11, 12, 13, 14, 16, 17,
            19, 21, 23, 25, 28, 31, 34, 37, 41, 45,
            50, 55, 60, 66, 73, 80, 88, 97, 107, 118,
            130, 143, 157, 173, 190, 209, 230, 253, 279, 307,
            337, 371, 408, 449, 494, 544, 598, 658, 724, 796,
            876, 963, 1060, 1166, 1282, 1411, 1552, 1707, 1878, 2066,
            2272, 2499, 2749, 3024, 3327, 3660, 4026, 4428, 4871, 5358,
            5894, 6484, 7132, 7845, 8630, 9493, 10442, 11487, 12635, 13899,
            15289, 16818, 18500, 20350, 22385, 24623, 27086, 29794, 32767
        };

        private static readonly int[][] IMC_NAV_TABLE = new [] {
            new [] { -1, 
                      4 },
            new [] { -1, -1, 
                      2,  6 },
            new [] { -1, -1, -1, -1, 
                      1,  2,  4,  6 },
            new [] { -1, -1, -1, -1, -1, -1, -1, -1, 
                      1,  1,  1,  2,  2,  4,  5,  6, },
            new [] { -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
                      1,  1,  1,  1,  1,  2,  2,  2,  2,  4,  4,  4,  5,  5,  6,  6 },
            new [] { -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
		              1,  1,  1,  1,  1,  1,  1,  1,  1,  1,  2,  2,  2,  2,  2,  2,  2,  2,  4,  4,  4,  4,  4,  4,  5,  5,  5,  5,  6,  6,  6,  6 }
        };
    }
}
