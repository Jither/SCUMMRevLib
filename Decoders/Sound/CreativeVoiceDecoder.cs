using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Katana.IO;
using Katana.Logging;
using SCUMMRevLib.Chunks;

namespace SCUMMRevLib.Decoders.Sound
{
    [DecodesChunks("Crea")]
    public class CreativeVoiceDecoder : BaseSoundDecoder
    {
        private ulong position;

        public override SoundInfo GetInfo(Chunk chunk)
        {
            BinReader reader = chunk.GetReader();
            reader.Position = 0x14;
            uint fileHeaderSize = reader.ReadU16LE();
            reader.Position = fileHeaderSize;
            uint sampleRate;
            while (true)
            {
                uint header = reader.ReadU32LE();
                byte blockType = (byte)(header & 0xff);
                uint blockSize = header >> 8;
                if (blockType == 0x01)
                {
                    byte frequencyDivisor = reader.ReadU8();
                    sampleRate = (uint)(1000000/(256 - frequencyDivisor));
                    byte codecId = reader.ReadU8();
                    if (codecId != 0)
                    {
                        throw new DecodingException("Unsupported VOC codec: {0}", codecId);
                    }
                    break;
                }
                reader.Position += blockSize;
            }
            SoundInfo info = new SoundInfo(1, sampleRate, 8);
            return info;
        }

        public override bool CanDecode(Chunk chunk)
        {
            return true;
        }

        public override void Initialize(Chunk chunk)
        {
            base.Initialize(chunk);
            position = 0;
        }

        protected override uint DecodePacket(out byte[] packet)
        {
            BinReader reader = currentChunk.GetReader();
            if (position == 0)
            {
                reader.Position = 0x14;
                uint fileHeaderSize = reader.ReadU16LE();
                position = fileHeaderSize;
            }

            while (true)
            {
                reader.Position = position;
                byte blockType = reader.ReadU8();
                uint blockSize = 0;
                
                if (blockType > 0)
                {
                    blockSize = reader.ReadU24LE();
                }

                switch (blockType)
                {
                    case 0x00:
                        packet = null;
                        return 0;
                    case 0x01:
                        ushort info = reader.ReadU16LE();
                        byte[] buffer;
                        uint size = blockSize - 2;
                        reader.Read(size, out buffer);
                        
                        position = reader.Position;
                        
                        packet = buffer;
                        return size;
                    case 0x02:
                    case 0x03:
                    case 0x04:
                    case 0x05:
                    case 0x06:
                    case 0x07:
                    case 0x08:
                    case 0x09:
                        Logger.Warning("Unimplemented VOC block type: {0:x2}. Skipped");
                        position += blockSize;
                        break;
                    default:
                        // Unknown block - writing what we have so far
                        Logger.Warning("Unknown VOC block type: {0:x2}. Decoded audio will not be complete.", blockType);
                        packet = null;
                        return 0;
                }
            }
        }
    }
}
