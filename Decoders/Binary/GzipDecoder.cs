using System;
using System.IO;
using System.IO.Compression;
using Katana.IO;
using SCUMMRevLib.Chunks;

namespace SCUMMRevLib.Decoders.Binary
{
    [DecodesChunks(".snm", ".tga", ".til")]
    public class GzipDecoder : BaseBinaryDecoder
    {
        public override string GetFileExtension(Chunk chunk)
        {
            return "*" + chunk.ChunkTypeId;
        }

        public override void Decode(Chunk chunk, Stream destination)
        {
            using (ChunkStream source = chunk.GetStream())
            {
                using (GZipStream gzipStream = new GZipStream(source, CompressionMode.Decompress))
                {
                    gzipStream.CopyTo(destination);
                }
            }
        }

        public override bool CanDecode(Chunk chunk)
        {
            BinReader reader = chunk.GetReader();
            ushort magic = reader.ReadU16BE();
            return (magic == 0x1f8b);
        }

        public override string GetOutputDescription(Chunk chunk)
        {
            return "Uncompressed file";
        }
    }
}
