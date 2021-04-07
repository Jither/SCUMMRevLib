using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Katana.Logging;
using Katana.Types;
using SCUMMRevLib.FileFormats;

namespace SCUMMRevLib.Chunks
{
    public class SCUMM1Chunk : Chunk
    {
        private readonly SCUMM1ChunkSpec spec;

        public SCUMM1Chunk(SRFile file, Chunk parent, string name, ulong offset, uint size) : base(file, parent)
        {
            Offset = offset;
            Size = size;
            Name = name;
            spec = SCUMM1ChunkSpecs.GetSpec(name, parent);
        }

        public override string ChunkTypeId
        {
            get { return spec.Id; }
        }

        public override string Description
        {
            get { return spec.Description; }
        }

        public override bool HasChildren
        {
            get { return spec.HasChildren && CheckChildren(); }
        }

        private bool CheckChildren()
        {
            // TODO: Improve this?
            return true;
        }

        protected override ChunkList InternalGetChildren()
        {
            if (ChunkTypeId == SCUMM1ChunkSpecs.RoomName)
            {
                return GetRoomChildren();
            }
            ChunkList result = new ChunkList();
            return result;
        }

        private ChunkList GetRoomChildren()
        {
            var result = new ChunkList();

            var reader = GetReader();
            reader.Position = 10;

            ushort imageCharsOffset = reader.ReadU16LE();
            ushort imageOffset = reader.ReadU16LE();
            ushort paletteOffset = reader.ReadU16LE();
            ushort zplaneOffset = reader.ReadU16LE();
            ushort zplaneCharsOffset = reader.ReadU16LE();
            byte objectCount = reader.ReadU8(); // or U16LE?
            byte unknown1 = reader.ReadU8();
            byte soundCount = reader.ReadU8();
            byte scriptCount = reader.ReadU8();
            ushort excdOffset = reader.ReadU16LE();
            ushort encdOffset = reader.ReadU16LE();

            ushort afterObjectsOffset = excdOffset > 0 ? excdOffset // If there's an EXCD block, use that
                                      : encdOffset > 0 ? encdOffset // Or ENCD block
                                      : (ushort)Size;               // If none of them, use the room block size

            List<ushort> obimOffsets = new List<ushort>();
            List<ushort> obcdOffsets = new List<ushort>();

            for (int i = 0; i < objectCount; i++)
            {
                obimOffsets.Add(reader.ReadU16LE());
            }
            for (int i = 0; i < objectCount; i++)
            {
                obcdOffsets.Add(reader.ReadU16LE());
            }

            obimOffsets.Sort();
            obcdOffsets.Sort();

            // RMHD = everything we read up until now
            var rmhdChunk = new SCUMM1Chunk(file, this, "RMHDv1", Offset + 2, (uint) reader.Position - 2);
            result.Add(rmhdChunk);

            // BOXD
            ulong boxdOffset = reader.Position;
            byte boxCount = reader.ReadU8();
            uint boxdSize = (uint) (boxCount*8 + 1);
            var boxdChunk = new SCUMM1Chunk(file, this, "BOXDv1", boxdOffset, boxdSize);
            result.Add(boxdChunk);

            reader.Position = boxdOffset + boxdSize;
            
            // BOXM
            ulong boxmOffset = reader.Position;
            uint boxmSize = (uint)(boxCount + boxCount*boxCount);
            var boxmChunk = new SCUMM1Chunk(file, this, "BOXMv1", boxmOffset, boxmSize);
            result.Add(boxmChunk);

            // There's not supposed to be a gap here, but in case there is...
            if (reader.Position < imageCharsOffset)
            {
                var unknownChunk = new SCUMM1Chunk(file, this, "????v1", reader.Position, (uint) (imageCharsOffset - reader.Position));
                result.Add(unknownChunk);

                reader.Position = imageCharsOffset;
            }

            // RMCH - room image characters
            var rmchChunk = new SCUMM1Chunk(file, this, "RMCHv1", imageCharsOffset, (uint)(imageOffset - imageCharsOffset));
            result.Add(rmchChunk);

            // RMIM
            var rmimChunk = new SCUMM1Chunk(file, this, "RMIMv1", imageOffset, (uint) (paletteOffset - imageOffset));
            result.Add(rmimChunk);

            // CLUT - palette
            var clutChunk = new SCUMM1Chunk(file, this, "CLUTv1", paletteOffset, (uint) (zplaneOffset - paletteOffset));
            result.Add(clutChunk);

            // ZPLN
            var zplnChunk = new SCUMM1Chunk(file, this, "ZPLNv1", zplaneOffset, (uint) (zplaneCharsOffset - zplaneOffset));
            result.Add(zplnChunk);

            // ZPCH - zplane characters
            // Calculation of size depends on whether we have objects or not
            ushort zpchSize;
            if (objectCount > 0)
            {
                zpchSize = (ushort) (obimOffsets[0] - zplaneCharsOffset);
            }
            else
            {
                // If no objects, use offset to after-objects:
                zpchSize = (ushort)(afterObjectsOffset - zplaneCharsOffset);
            }

            var zpchChunk = new SCUMM1Chunk(file, this, "ZPCHv1", zplaneCharsOffset, zpchSize);
            result.Add(zpchChunk);

            // OBIM
            for (int i = 0; i < objectCount; i++)
            {
                uint offset = obimOffsets[i];
                uint size = ((i < objectCount - 1) ? obimOffsets[i + 1] : obcdOffsets[0]) - offset;
                var chunk = new SCUMM1Chunk(file, this, "OBIMv1", offset, size);
                result.Add(chunk);
            }

            // OBCD
            for (int i = 0; i < objectCount; i++)
            {
                uint offset = obcdOffsets[i];
                reader.Position = offset;
                uint calculatedSize = ((i < objectCount - 1) ? obcdOffsets[i + 1] : afterObjectsOffset) - offset;
                uint readSize = reader.ReadU16LE();
                if (calculatedSize != readSize)
                {
                    Logger.Warning("OBIM size differs between calculated ({0}) and read ({1})", calculatedSize, readSize);
                }
                var chunk = new SCUMM1Chunk(file, this, "OBCDv1", offset, calculatedSize);
                result.Add(chunk);
            }

            // EXCD
            if (excdOffset > 0)
            {
                uint excdSize = (encdOffset > 0 ? encdOffset : Size) - excdOffset;
                var excdChunk = new SCUMM1Chunk(file, this, "EXCDv1", excdOffset, excdSize);
                result.Add(excdChunk);
            }
            if (encdOffset > 0)
            {
                uint encdSize = Size - encdOffset;
                var encdChunk = new SCUMM1Chunk(file, this, "ENCDv1", encdOffset, encdSize);
                result.Add(encdChunk);
            }

            return result;
        }

        public override ImageIndex ImageIndex
        {
            get { return spec.ImageIndex; }
        }
    }
}
