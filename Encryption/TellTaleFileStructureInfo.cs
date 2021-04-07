using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using Katana.IO;

namespace SCUMMRevLib.Encryption
{
    public class TellTaleFileStructureInfo
    {
        public uint FileVersion { get; set; }

        public bool HasInfo { get; set; }
        public bool IsInfoEncrypted { get; set; }
        public bool IsInfoCompressed { get; set; }
        public ulong InfoOffset { get; set; }
        public uint InfoSizeUncompressed { get; set; }
        public uint InfoSizeCompressed { get; set; }

        public ulong HeaderSize
        {
            get { return HasInfo ? InfoOffset : VirtualBlocksOffset; }
        }

        /// <summary>
        /// File offset to first compressed/encrypted block
        /// </summary>
        public ulong VirtualBlocksOffset { get; set; }
        
        /// <summary>
        /// Size of uncompressed blocks
        /// </summary>
        public uint BlockSizeUncompressed { get; set; }

        /// <summary>
        /// File offsets for each compressed/encrypted block
        /// </summary>
        public List<ulong> BlockOffsets { get; private set; }

        /// <summary>
        /// Compressed sizes of each block
        /// </summary>
        public List<uint> BlockSizesCompressed { get; private set; }

        /// <summary>
        /// Number of compressed/encrypted blocks
        /// </summary>
        public int BlockCount { get { return BlockOffsets.Count; } }

        public TellTaleFileStructureInfo()
        {
            BlockOffsets = new List<ulong>();
            BlockSizesCompressed = new List<uint>();
        }

        public byte[] ReadInfoBlock(BinReader reader, Blowfish blowfish)
        {
            byte[] infoBufferDecompressed = new byte[InfoSizeUncompressed];
            byte[] result = new byte[InfoSizeCompressed];
            reader.Position = InfoOffset;
            reader.Read(result, 0, InfoSizeCompressed);
            if (IsInfoCompressed)
            {
                Decompress(result, infoBufferDecompressed, 0, (int)InfoSizeUncompressed, FileVersion < 9); // TODO: Check when exactly zlib header was removed
                result = infoBufferDecompressed;
            }
            if (IsInfoEncrypted && blowfish != null)
            {
                blowfish.Decipher(result, (InfoSizeUncompressed / 8) * 8);
            }
            
            return result;
        }

        private int Decompress(byte[] source, byte[] destination, int destinationOffset, int count, bool hasZlibHeader = false)
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
                    return stream.Read(destination, destinationOffset, count);
                }
            }
        }


    }
}
