using System;
using System.IO;

namespace SCUMMRevLib.Chunks
{
    public class ChunkStream : Stream
    {
        private readonly Stream baseStream;
        private readonly ulong fileOffset;
        private readonly long size;
        private long position;

        public ChunkStream(Stream stream, ulong fileOffset, long size)
        {
            baseStream = stream;
            this.fileOffset = fileOffset;
            this.size = size;
        }

        public override bool CanRead
        {
            get { return true; }
        }

        public override bool CanSeek
        {
            get { return true; }
        }

        public override bool CanWrite
        {
            get { return false; }
        }

        public override void Flush()
        {
            // Do nothing
        }

        public override long Length
        {
            get { return size; }
        }

        public override long Position
        {
            get
            {
                return position;
            }
            set
            {
                position = value;
            }
        }

        public override int Read(byte[] buffer, int start, int count)
        {
            baseStream.Position = (long)fileOffset + position;
            count = (int)Math.Min(count, size - position);
            int read = baseStream.Read(buffer, start, count);
            position += read;

            return read;
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            switch (origin)
            {
                case SeekOrigin.Begin:
                    position = Math.Min(offset, size - 1);
                    break;
                case SeekOrigin.End:
                    position = Math.Max(size - offset, 0);
                    break;
                case SeekOrigin.Current:
                    position += offset;
                    if (position < 0) position = 0;
                    if (position > size - 1) position = size - 1;
                    break;
            }
            return position;
        }

        public override void SetLength(long value)
        {
            throw new NotSupportedException();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotSupportedException();
        }
    }
}
