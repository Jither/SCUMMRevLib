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
    [DecodesChunks("SCRP", "ENCD", "EXCD")]
    public class SCUMM5DisassemblerDecoder : BaseTextDecoder
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
            byte[] code;
            BinReader reader = chunk.GetReader();
            reader.Position = 8;
            reader.Read(chunk.Size - 8, out code);

            Disassembler disassembler;

            try
            {
                int version = -1;
                var file = chunk.File as SCUMM5File;
                if (file != null)
                {
                    version = file.FileVersion;
                }
                switch (version)
                {
                    case 5: disassembler = new DisassemblerSCUMM5(); break;
                    case 6:
                    case 7: disassembler = new DisassemblerSCUMM6(); break;
                    case 8: disassembler = new DisassemblerSCUMM8(); break;
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
