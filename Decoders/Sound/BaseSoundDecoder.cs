using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Katana.Logging;
using SCUMMRevLib.Chunks;

namespace SCUMMRevLib.Decoders.Sound
{
    public abstract class BaseSoundDecoder : BaseDecoder
    {
        protected Chunk currentChunk;
        protected byte[] currentPacket;
        protected uint packetSize;
        protected uint packetPosition;
        protected bool moreData;

        public override DecoderFormat Format
        {
            get { return DecoderFormat.Sound; }
        }

        public override string GetOutputDescription(Chunk chunk)
        {
            return "Sound";
        }

        public abstract SoundInfo GetInfo(Chunk chunk);

        public uint Decode(byte[] output, uint outputSize)
        {
            if (!moreData)
            {
                return 0;
            }

            uint copied = 0;
            // Copy any remainder of last packet
            if (currentPacket != null) 
            {
                uint packetRemainder = packetSize - packetPosition;
                uint length = Math.Min(outputSize, packetRemainder);
                Array.Copy(currentPacket, packetPosition, output, 0, length);
                copied += length;
                packetPosition += length;
            }

            // Read more packets if needed
            while (copied < outputSize)
            {
                packetSize = DecodePacket(out currentPacket);
                packetPosition = 0;
                
                if (currentPacket == null)
                {
                    // No more data
                    moreData = false;
                    return copied;
                }

                uint length = Math.Min(outputSize - copied, packetSize);
                Array.Copy(currentPacket, 0, output, copied, length);
                copied += length;
                packetPosition += length;
                if (packetPosition == packetSize)
                {
                    currentPacket = null;
                }
            }

            return copied;
        }

        public virtual void Initialize(Chunk chunk)
        {
            currentChunk = chunk;
            currentPacket = null;
            packetPosition = 0;
            moreData = true;
        }

        /// <summary>
        /// Decodes a single packet of data.
        /// Output buffer may be reused by decoder implementation, so do all work you need to do on it
        /// (or copy the buffer elsewhere) before calling DecodePacket again.
        /// </summary>
        /// <param name="buffer">Decoded packet</param>
        /// <returns>Size of decoded data</returns>
        protected abstract uint DecodePacket(out byte[] buffer);
    }
}
