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
    [DecodesChunks("SC", "EN", "EX")]
    public class SCUMM3DisassemblerDecoder : BaseTextDecoder
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
            reader.Position = 6;
            reader.Read(chunk.Size - 6, out code);

            Disassembler disassembler;

            try
            {
                int version = -1;
                var file = chunk.File as SCUMM3File;
                if (file != null)
                {
                    version = file.FileVersion;
                }
                switch (version)
                {
                    case 3: disassembler = new DisassemblerSCUMM3(); break;
                    case 4: disassembler = new DisassemblerSCUMM4(); break;
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
