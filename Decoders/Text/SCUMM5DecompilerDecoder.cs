using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Katana.IO;
using SCUMMRevLib.Chunks;
using SCUMMRevLib.Decompilers.SCUMM;

namespace SCUMMRevLib.Decoders.Text
{
    [DecodesChunks("SCRP", "ENCD", "EXCD")]
    public class SCUMM5DecompilerDecoder : BaseTextDecoder
    {
        public override string DecodedSyntax
        {
            get { return Syntax.SCUMM; }
        }

        public override string GetOutputDescription(Chunk chunk)
        {
            return "Decompiled SCUMM script";
        }

        public override string Decode(Chunk chunk)
        {
            byte[] code;
            BinReader reader = chunk.GetReader();
            reader.Position = 8;
            reader.Read(chunk.Size - 8, out code);

            SCUMMDecompiler decompiler = new SCUMM5Decompiler();
            try
            {
                return decompiler.Decompile(code);
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
