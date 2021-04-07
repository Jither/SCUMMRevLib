using System;
using System.IO;
using System.Text;
using Katana.IO;
using Katana.Types;
using SCUMMRevLib.FileFormats;

namespace SCUMMRevLib.Chunks
{
    public class TTARCHChunk : Chunk
    {
        public override string ChunkTypeId
        {
            get { return "TTARCH"; }
        }

        public override string Description
        {
            get { return "TellTale Archive"; }
        }

        public override bool HasChildren
        {
            get { return true; }
        }

        public override ImageIndex ImageIndex
        {
            get { return ImageIndex.Bundle; }
        }

        protected override ChunkList InternalGetChildren()
        {
            // TODO: This should probably be moved to be handled by TellTaleFile
            file.Position = Offset;
            var reader = file; //.GetChunkReader(this);

            var fileInfo = TTArchFile.PrepareFileInfo(reader);

            var result = new ChunkList();

            reader.Position = fileInfo.InfoOffset;
            uint folderCount = reader.ReadU32LE();

            for (uint i = 0; i < folderCount; i++)
            {
                uint nameSize = reader.ReadU32LE();
                string name = reader.ReadString(nameSize);
                result.Add(new TellTaleFileChunk(this.File, this, name, 0, 0));
            }

            uint fileCount = reader.ReadU32LE();

            for (uint i = 0; i < fileCount; i++)
            {
                uint nameSize = reader.ReadU32LE();
                string name = reader.ReadString(nameSize);

                uint zero = reader.ReadU32LE();
                ulong offset = reader.ReadU32LE() + fileInfo.VirtualBlocksOffset;
                uint size = reader.ReadU32LE();

                result.Add(new TellTaleFileChunk(this.File, this, name, offset, size));
            }

            return result;
        }

        public TTARCHChunk(SRFile file, Chunk parent) : base(file, parent)
        {
            this.Name = "TTARCH";
            this.Offset = file.Position;
            this.Size = file.Size;
        }
    }
}
