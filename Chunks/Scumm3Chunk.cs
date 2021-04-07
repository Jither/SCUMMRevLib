using System;
using Katana.Types;
using SCUMMRevLib.FileFormats;

namespace SCUMMRevLib.Chunks
{
    public class SCUMM3Chunk : Chunk
    {
        private readonly SCUMM3ChunkSpec spec;

        public SCUMM3Chunk(SRFile file, Chunk parent, string name, ulong offset, uint size) : base(file, parent)
        {
            Offset = offset;
            Name = name;
            spec = SCUMM3ChunkSpecs.GetSpec(name, parent);
            Size = size;
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

        // TODO: Try to get rid of this static function
        // (needed in order for file to read root chunks without having a parent chunk)
        public static Chunk ReadChunk(SRFile file, Chunk parent)
        {
            ulong offset = file.Position;
            uint size = file.ReadU32LE();
            TwoCC fourCC = file.ReadTwoCC();
            if (!fourCC.IsValid)
            {
                return new UnknownChunk(file, parent, offset, (uint)(parent.Offset + parent.Size - offset));
            }

            return new SCUMM3Chunk(file, parent, fourCC.Name, offset, size);
        }

        private bool CheckChildren()
        {
            file.Position = Offset + spec.ChildOffset;
            uint size = file.ReadU32LE();
            if (size >= Offset + Size) return false;
            TwoCC twoCC = file.ReadTwoCC();
            if (!twoCC.IsValid) return false;
            return true;
        }

        protected override ChunkList InternalGetChildren()
        {
            ChunkList result = new ChunkList();
            file.Position = Offset + spec.ChildOffset;

            ulong maxPosition = Offset + Size;

            while (file.Position < maxPosition)
            {
                Chunk chunk = ReadChunk(file, this);
                result.Add(chunk);
                file.Position = chunk.Offset + chunk.Size;
            }
            return result;
        }

        public override ImageIndex ImageIndex
        {
            get { return spec.ImageIndex; }
        }
    }
}
