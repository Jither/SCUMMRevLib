using System;
using System.IO;
using System.IO.Compression;
using Katana.IO;

namespace SCUMMRevLib.Encryption
{
    /// <summary>
    /// Wrapper stream for blowfish-encrypted zlib compressed archive files.
    /// This provides a stream where contained files can be accessed by their position in a "virtual"
    /// uncompressed archive
    /// </summary>
    public class TellTaleBlowfishZlibStream : Stream
    {
        protected readonly BinReader reader;
        private readonly TellTaleFileStructureInfo fileInfo;

        // Used for seeking through Zlib block.
        private readonly byte[] tempBuffer = new byte[2000];

        private readonly byte[] readBuffer;

        // Preloaded info table (for ttarch1):
        private readonly byte[] infoBuffer;

        private readonly Blowfish blowfish;

        // Current position in "virtual" (uncompressed) file that we're reading from
        private ulong virtualPosition;

        public TellTaleBlowfishZlibStream(Stream stream, TellTaleFileStructureInfo fileInfo, byte[] key = null, bool useModifiedBlowfish = false)
        {
            reader = new BinReader(stream);
            this.fileInfo = fileInfo;

            virtualPosition = 0;

            readBuffer = new byte[fileInfo.BlockSizeUncompressed];

            if (key != null)
            {
                // Blowfish is stateless decryption per block (8 bytes), so we can 
                // create a single decrypter based on key/version specifics here:
                blowfish = new Blowfish(key, useModifiedBlowfish);
            }

            // ttarch1: Preload the info block
            if (fileInfo.HasInfo)
            {
                infoBuffer = fileInfo.ReadInfoBlock(reader, blowfish);
            }
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            switch (origin)
            {
                case SeekOrigin.Begin:
                    virtualPosition = (ulong)offset; // + blocksBaseOffset;
                    break;
                case SeekOrigin.Current:
                    // We cannot simply add (negative) offset, because ulong += long is not allowed
                    // So we cast to ulong, and add/subtract appropriately
                    if (offset > 0)
                    {
                        virtualPosition += (ulong) offset;
                    }
                    else
                    {
                        virtualPosition -= (ulong) (Math.Abs(offset));
                    }
                    break;
                case SeekOrigin.End:
                    virtualPosition = (ulong)(Length - offset);
                    break;
            }
            return (long)virtualPosition;
        }

        public override int Read(byte[] destination, int offset, int count)
        {
            int totalBytesRead = 0;

            // First read any uncompressed bytes in the header
            if (virtualPosition < fileInfo.HeaderSize)
            {
                var bytesRead = ReadUncoded(destination, offset, count);
                totalBytesRead += bytesRead;

                count -= bytesRead;
                offset += bytesRead;

                // Done?
                if (count == 0)
                {
                    return totalBytesRead;
                }
            }

            // ttarch1: Read info
            if (fileInfo.HasInfo && virtualPosition < fileInfo.VirtualBlocksOffset)
            {
                // The info table was preloaded in the constructor
                int bytesToRead = Math.Min(count, (int) fileInfo.InfoSizeUncompressed);
                Array.Copy(infoBuffer, (int)(virtualPosition - fileInfo.InfoOffset), destination, offset, bytesToRead);
                virtualPosition += (ulong)bytesToRead;

                totalBytesRead += bytesToRead;

                count -= bytesToRead;
                offset += bytesToRead;

                // Done?
                if (count == 0)
                {
                    return totalBytesRead;
                }
            }

            // Now read from compressed/encrypted blocks:
            if (fileInfo.BlockCount > 0)
            {
                totalBytesRead += ReadCoded(destination, offset, count, true);
            }
            else
            {
                totalBytesRead += ReadUncoded(destination, offset, count);
            }
            return totalBytesRead;
        }

        private int ReadUncoded(byte[] destination, int offset, int count)
        {
            reader.Position = virtualPosition;

            // Either read <count> bytes or all remaining bytes in uncompressed area
            uint bytesLeft = (uint)(fileInfo.HeaderSize - virtualPosition);
            uint bytesToRead = Math.Min((uint)count, bytesLeft);

            int bytesRead = reader.Read(destination, (uint)offset, bytesToRead);

            virtualPosition += (uint)bytesRead;

            return bytesRead;
        }

        private int ReadCoded(byte[] destination, int offset, int count, bool decrypt)
        {
            int totalBytesRead = 0;

            // Find block corresponding to current position:
            ulong uncompressedOffset = virtualPosition - fileInfo.VirtualBlocksOffset;
            int blockIndex = (int) (uncompressedOffset/fileInfo.BlockSizeUncompressed);

            while (count > 0)
            {
                // Find offset in file:
                ulong fileOffset = fileInfo.BlockOffsets[blockIndex];

                uint blockSize = fileInfo.BlockSizesCompressed[blockIndex];

                reader.Position = fileOffset;
                reader.Read(readBuffer, 0, blockSize);

                // Decrypt if we need to:
                if (blowfish != null && decrypt)
                {
                    // Only full blocks (8 bytes) are encrypted:
                    blowfish.Decipher(readBuffer, (blockSize/8)*8);
                }

                // Find the requested read offset relative to the start of the block:
                ulong blockReadOffset = (virtualPosition - fileInfo.VirtualBlocksOffset) - (ulong) (blockIndex*fileInfo.BlockSizeUncompressed);
                
                // Either decompress <count> bytes or entire remaining block:
                int bytesToRead = Math.Min(count, (int)(fileInfo.BlockSizeUncompressed - blockReadOffset));
                
                int bytesRead = Decompress(readBuffer, destination, blockReadOffset, offset, bytesToRead, fileInfo.FileVersion < 9);
                
                virtualPosition += (uint)bytesRead;
                
                count -= bytesRead;
                offset += bytesRead;
                totalBytesRead += bytesRead;

                // Move to next block
                blockIndex++;
            }
            return totalBytesRead;
        }

        private int Decompress(byte[] source, byte[] destination, ulong sourceOffsetDecompressed, int destinationOffset, int count, bool hasZlibHeader = false)
        {
            using (MemoryStream input = new MemoryStream(source))
            {
                if (hasZlibHeader)
                {
                    // Skip header
                    input.ReadByte();
                    input.ReadByte();
                }
                using (DeflateStream stream = new DeflateStream(input, CompressionMode.Decompress))
                {
                    // Seek to the actual offset inside the block
                    // - we can't actually seek, so we read and discard:
                    while (sourceOffsetDecompressed > 0)
                    {
                        int copyCount = sourceOffsetDecompressed > (ulong) tempBuffer.Length ? tempBuffer.Length : (int) sourceOffsetDecompressed;
                        sourceOffsetDecompressed -= (uint)stream.Read(tempBuffer, 0, copyCount);
                    }

                    // ... and read either the requested bytes,
                    // or the remaining bytes in the block
                    return stream.Read(destination, destinationOffset, count);
                }
            }
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

        public override long Length
        {
            get { return fileInfo.BlockCount > 0 ? (long)fileInfo.VirtualBlocksOffset + fileInfo.BlockCount* fileInfo.BlockSizeUncompressed : reader.Size; }
        }

        public override long Position
        {
            get { return (long)virtualPosition; }
            set { Seek(value, SeekOrigin.Begin); }
        }

        public override void Flush()
        {
            throw new NotSupportedException(String.Format("{0} is read-only", GetType().Name));
        }

        public override void SetLength(long value)
        {
            throw new NotSupportedException(String.Format("{0} is read-only", GetType().Name));
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotSupportedException(String.Format("{0} is read-only", GetType().Name));
        }
    }
}
