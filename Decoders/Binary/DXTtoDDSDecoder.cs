using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Katana.IO;
using Katana.Types;
using SCUMMRevLib.Chunks;
using SCUMMRevLib.FileFormats;

namespace SCUMMRevLib.Decoders.Binary
{
    [Flags]
    public enum DDSFlags : uint
    {
        Caps = 0x00000001,
        Height = 0x00000002,
        Width = 0x00000004,
        Pitch = 0x00000008,
        PixelFormat = 0x00001000,
        MipmapCount = 0x00020000,
        LinearSize = 0x00080000,
        Depth = 0x00800000
    }

    public enum DDSPixelFormat : uint
    {
        AlphaPixels = 0x00000001,
        FourCC = 0x00000004,
        RGB = 0x00000040
    }

    [Flags]
    public enum DDSCaps1 : uint
    {
        Complex = 0x00000008,
        Texture = 0x00001000,
        Mipmap = 0x00400000
    }

    [Flags]
    public enum DDSCaps2 : uint
    {
        Cubemap = 0x00000200,
        CubemapPositiveX = 0x00000400,
        CubemapNegativeX = 0x00000800,
        CubemapPositiveY = 0x00001000,
        CubemapNegativeY = 0x00002000,
        CubemapPositiveZ = 0x00004000,
        CubemapNegativeZ = 0x00008000,
        CubemapVolume = 0x00200000
    }

    [DecodesChunks(".dxt")]
    public class DXTtoDDSDecoder : BaseBinaryDecoder
    {
        public override DecoderFormat Format
        {
            get { return DecoderFormat.Binary; }
        }

        public override bool CanDecode(Chunk chunk)
        {
            return true;
        }

        public override string GetOutputDescription(Chunk chunk)
        {
            return "DDS texture";
        }

        public override void Decode(Chunk chunk, Stream destination)
        {
            BinReader reader = chunk.GetReader();
            reader.Position = 0;
            FourCC fourCC = reader.ReadFourCC();
            uint width = reader.ReadU32LE();
            uint height = reader.ReadU32LE();

            BinWriter writer = new BinWriter(destination);
            writer.WriteFourCC("DDS ");
            writer.WriteU32LE(124);
            writer.WriteU32LE((uint)(DDSFlags.Caps | DDSFlags.PixelFormat | DDSFlags.Width | DDSFlags.Height | DDSFlags.LinearSize));
            writer.WriteU32LE(height);
            writer.WriteU32LE(width);
            writer.WriteU32LE(width * height * 4);
            writer.WriteU32LE(0);
            writer.WriteU32LE(0);
            for (int i = 0; i < 11; i++)
            {
                writer.WriteU32LE(0);
            }

            writer.WriteU32LE(32);
            writer.WriteU32LE((uint)DDSPixelFormat.FourCC);
            writer.WriteFourCC(fourCC);
            writer.WriteU32LE(0);
            writer.WriteU32LE(0);
            writer.WriteU32LE(0);
            writer.WriteU32LE(0);
            writer.WriteU32LE(0);

            writer.WriteU32LE((uint)DDSCaps1.Texture);
            writer.WriteU32LE(0);
            writer.WriteU32LE(0);
            writer.WriteU32LE(0);

            writer.WriteU32LE(0);

            byte[] buffer = new byte[chunk.Size - 12];
            ChunkStream chunkStream = chunk.GetStream();
            chunkStream.Position = 12;
            chunkStream.Read(buffer, 0, (int)chunk.Size - 12);
            writer.Write((uint)buffer.Length, buffer);
        }

        public override string GetFileExtension(Chunk chunk)
        {
            return "dds";
        }
    }
}
