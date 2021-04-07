using System;
using Katana.Types;
using SCUMMRevLib.FileFormats;

namespace SCUMMRevLib.Chunks
{
    public class SCUMM5Chunk : Chunk
    {
        private readonly SCUMM5ChunkSpec spec;

        public SCUMM5Chunk(SRFile file, Chunk parent, string name, ulong offset, uint size) : base(file, parent)
        {
            Offset = offset;
            Name = name;
            spec = SCUMM5ChunkSpecs.GetSpec(name, parent);

            CalculateSize(parent, size);
        }

        private void CalculateSize(Chunk parent, uint sizeRead)
        {
            switch (spec.SizeFormat)
            {
                case SizeFormat.Standard:
                    Size = sizeRead;
                    break;
                case SizeFormat.BasedOnParent:
                    Size = parent != null ? parent.Size - (uint)(Offset - parent.Offset) : file.Size;
                    break;
                case SizeFormat.WithoutHeader:
                    Size = sizeRead + 8;
                    break;
                case SizeFormat.WithoutHeaderPadded:
                    Size = sizeRead + 8 + (sizeRead%2);
                    break;
                case SizeFormat.Creative:
                    file.Position = Offset + 0x14;
                    ulong nextOffset = Offset + file.ReadU16LE();
                    byte blockType;
                    do
                    {
                        file.Position = nextOffset;
                        blockType = file.ReadU8();
                        nextOffset++;
                        switch (blockType)
                        {
                            case 1:
                            case 2:
                            case 5:
                                nextOffset += file.ReadU24LE() + 3;
                                break;
                            case 3:
                                nextOffset += 6;
                                break;
                            case 4:
                                nextOffset += 5;
                                break;
                            case 6:
                                nextOffset += 5;
                                break;
                            case 8:
                                nextOffset += 7;
                                break;
                        }
                    } 
                    while (blockType != 0);
                    Size = (uint)(nextOffset - Offset);
                    break;
                case SizeFormat.COMP:
                    // Based on entry count (U32):
                    Size = sizeRead*16 + 18;
                    break;
                case SizeFormat.MCMP:
                    // Based on entry count (U16):
                    file.Position = Offset + 4;
                    Size = (uint)file.ReadU16BE()*9 + 18;
                    break;
            }
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
            uint size;
            FourCC fourCC = file.ReadFourCC();
            if (!fourCC.IsValid)
            {
                size = (uint)(parent != null ? parent.Offset + parent.Size - offset : file.Size - offset);
                return new UnknownChunk(file, parent, offset, size);
            }
            size = file.ReadU32BE();

            return new SCUMM5Chunk(file, parent, fourCC.Name, offset, size);
        }

        private bool CheckChildren()
        {
            file.Position = Offset + 8;
            var fourCC = file.ReadFourCC();
            if (!fourCC.IsValid) return false;

            // TODO: Proper size check
            // For now, just ensuring we're not dealing with Crea
            if (fourCC.Name != "Crea" && file.ReadU32BE() >= Offset + Size) return false;
            return true;
        }

        protected override ChunkList InternalGetChildren()
        {
            ChunkList result = new ChunkList();
            file.Position = Offset + 8;

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
