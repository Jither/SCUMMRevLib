using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Katana.IO;
using SCUMMRevLib.Chunks;

namespace SCUMMRevLib.Decompilers.SCUMM
{
    public abstract class SCUMMDecompiler
    {
        private byte[] code;
        protected int pos;
        protected BinReader reader;
        protected Dictionary<int, Func<int, SCUMMCommand>> opcodeHandlers;

        public string Decompile(byte[] code)
        {
            SCUMMLineBuilder lineBuilder = new SCUMMLineBuilder();
            InitOpcodes();

            this.code = code;

            StringBuilder builder = new StringBuilder();

            using (Stream stream = new MemoryStream(code))
            using (reader = new BinReader(stream))
            {
                while (reader.Position < reader.Size)
                {
                    ulong offset = reader.Position;
                    byte opcode = reader.ReadU8();
                    Func<int, SCUMMCommand> handler;
                    if (!opcodeHandlers.TryGetValue(opcode, out handler))
                    {
                        throw new SCUMMDecompilerException("Unexpected opcode: {0}", opcode);
                    }
                    SCUMMCommand cmd = handler(opcode);
                    cmd.Offset = offset;
                    builder.AppendLine(lineBuilder.GetLine(cmd));
                }
            }

            return builder.ToString();
        }

        protected void SanityCheck(bool condition, string message)
        {
            if (!condition)
            {
                throw new SCUMMDecompilerException("Sanity check failed: {0}", message);
            }
        }

        protected abstract void InitOpcodes();
    }
}
