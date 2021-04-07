using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Katana.IO;
using SCUMMRevLib.Chunks;
using SCUMMRevLib.Decompilers.SCUMM;

namespace SCUMMRevLib.Decoders.Text
{
    [DecodesChunks("SC", "EN", "EX")]
    public class SCUMM3DecompilerDecoder : BaseTextDecoder
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
            reader.Position = 6;
            reader.Read(chunk.Size - 6, out code);
            SCUMM3Decompiler decompiler = new SCUMM3Decompiler();
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
