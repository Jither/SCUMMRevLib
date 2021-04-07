using System;
using System.IO;

namespace SCUMMRevLib.Encryption
{
    public class XorStream : FileStream
    {
        public override bool CanWrite
        {
            get { return false; }
        }

        public byte Encryption { get; set; }

        public XorStream(string path) : base(path, FileMode.Open, FileAccess.Read, FileShare.Read)
        {
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            int result = base.Read(buffer, offset, count);

            if (Encryption != 0)
            {
                for (int i = offset; i < offset + result; i++)
                {
                    buffer[i] ^= Encryption;
                }
            }

            return result;
        }

        public override void Flush()
        {
            // Do nothing
        }

        public override void SetLength(long value)
        {
            throw new NotImplementedException();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotImplementedException();
        }
    }
}
