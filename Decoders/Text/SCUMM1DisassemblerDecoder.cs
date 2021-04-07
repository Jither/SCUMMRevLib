using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Katana.IO;
using SCUMMRevLib.Chunks;
using SCUMMRevLib.Decompilers.SCUMM;
using SCUMMRevLib.FileFormats;

namespace SCUMMRevLib.Decoders.Text
{
    [DecodesChunks("SCRPv1", "ENCDv1", "EXCDv1")]
    public class SCUMM1DisassemblerDecoder : BaseTextDecoder
    {
        public override string DecodedSyntax
        {
            get { return Syntax.SCUMM; }
        }

        public override string GetOutputDescription(Chunk chunk)
        {
            return "SCUMM disassembly";
        }

        public override string Decode(Chunk chunk)
        {
            uint codeOffset = 0;
            switch (chunk.ChunkTypeId)
            {
                case "SCRPv1":
                    codeOffset = 4;
                    break;
            }

            byte[] code;
            BinReader reader = chunk.GetReader();
            reader.Position = codeOffset;
            reader.Read(chunk.Size - codeOffset, out code);

            Disassembler disassembler;

            try
            {
                int version = -1;
                var file = chunk.File as SCUMM1File;
                if (file != null)
                {
                    version = file.FileVersion;
                }
                switch (version)
                {
                    case 1: disassembler = new DisassemblerSCUMM1(); break;
                    case 2: disassembler = new DisassemblerSCUMM1(); break;
                    default:
                        throw new SCUMMDecompilerException("Could not determine SCUMM version");
                }

                return disassembler.Decompile(code);
            }
            catch (SCUMMDecompilerException ex)
            {
                return ex.Message;
            }
        }

        public override bool CanDecode(Chunk chunk)
        {
            return true;
        }
    }
}
