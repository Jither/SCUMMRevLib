using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCUMMRevLib.Utils
{
    public class LEBitStream
    {
        private readonly byte[] buffer;
        private int position;

        private uint bitsLeft;

        private byte bits;

        public LEBitStream(byte[] buffer) : this(buffer, 0)
        {
        }

        public LEBitStream(byte[] buffer, int pos) 
        {
            this.buffer = buffer;
            this.position = pos;
            FillBits();
        }

        private void FillBits()
        {
            if (position >= buffer.Length)
            {
                bits = 0;
                bitsLeft = 8;
                return;
            }
            bits = buffer[position];
            bitsLeft = 8;
            position++;
        }

        public int GetBit()
        {
            int result = bits & 0x01;
            bits >>= 1;
            bitsLeft--;
            if (bitsLeft == 0)
            {
                FillBits();
            }
            return result;
        }

        public int GetBits(int count)
        {
            int result = 0;
            for (int i = 0; i < count; i++)
            {
                result |= GetBit() << i;
            }
            return result;
        }

        public byte GetByte()
        {
            byte result = (byte)GetBits(8);
            return result;
        }

        public ushort GetWord()
        {
            int result = GetByte() | GetByte() << 8;
            return (ushort) result;
        }

        public uint GetDWord()
        {
            int result = GetWord() | GetWord() << 16;
            return (uint)result;
        }

        public void Seek(int position)
        {
            this.position = position;
            FillBits();
        }
    }
}
