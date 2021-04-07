using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCUMMRevLib.Utils
{
    public class WordBitStream
    {
        private readonly byte[] buffer;
        private int position;

        private uint bitsLeft;

        private ushort bits;
        public WordBitStream(byte[] buffer)
        {
            this.buffer = buffer;
            this.position = 0;
            FillBits();
        }

        private void FillBits()
        {
            bits = (ushort)(buffer[position] | buffer[position+1] <<8);
            bitsLeft = 16;
            position += 2;
        }

        public int GetBit()
        {
            int result = bits & 0x0001;
            bits >>= 1;
            bitsLeft--;
            if (bitsLeft == 0)
            {
                FillBits();
            }
            return result;
        }

        public byte GetByte()
        {
            byte result = buffer[position];
            position++;
            return result;
        }
    }
}
