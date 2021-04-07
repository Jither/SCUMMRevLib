using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Katana.IO;

namespace SCUMMRevLib.Decompilers.SCUMM
{
    public abstract class Disassembler
    {
        protected Dictionary<int, Action<int>> opcodeHandlers = new Dictionary<int, Action<int>>();

        protected List<SCUMMCommand> commands = new List<SCUMMCommand>();

        private ulong currentOffset;
        protected BinReader reader;

        protected Func<SCUMMParameter> GetString { get; set; }
        protected Func<SCUMMParameter> GetVar { get; set; }

        protected Disassembler()
        {
            GetString = GetStringNew;
            GetVar = GetVarNew;
        }

        public string Decompile(byte[] code)
        {
            if (!opcodeHandlers.Any())
            {
                InitOpcodes();
            }

            try
            {
                using (MemoryStream stream = new MemoryStream(code))
                {
                    using (reader = new BinReader(stream))
                    {
                        while (reader.Position < reader.Size)
                        {
                            currentOffset = reader.Position;
                            byte opcode = reader.ReadU8();

                            Action<int> handler;
                            if (!opcodeHandlers.TryGetValue(opcode, out handler))
                            {
                                throw new SCUMMDecompilerException("Unexpected opcode: {0}", opcode);
                            }
                            handler(opcode);
                        }

                    }
                }
            }
            catch (SCUMMDecompilerException ex)
            {
                return String.Format(
                    "{0}{1}{2}", 
                    String.Join(Environment.NewLine, commands), Environment.NewLine,
                    ex.Message
                );
            }
            return String.Join(Environment.NewLine, commands);
        }

        protected abstract void InitOpcodes();

        protected void AddOpcode(int opcode, Action<int> handler)
        {
            opcodeHandlers[opcode] =  handler;
        }

        protected void Add(SCUMMOpcode opcode, params SCUMMParameter[] parameters)
        {
            commands.Add(new SCUMMCommand(opcode, parameters) { Offset = currentOffset });
        }

        protected void SanityCheck(bool condition, string message)
        {
            if (!condition)
            {
                throw new SCUMMDecompilerException("Sanity check failed: {0}", message);
            }
        }

        protected SCUMMDecompilerException UnknownSubOpcode(string type, byte opcode)
        {
            throw new SCUMMDecompilerException("Unknown {0} subopcode: 0x{1:x2}", type, opcode);
        }

        protected SCUMMParameter GetByte()
        {
            return new SCUMMParameter(SCUMMParameterType.Number, reader.ReadU8());
        }

        protected virtual SCUMMParameter GetVarOld()
        {
            return new SCUMMParameter(SCUMMParameterType.Var, reader.ReadU8());
        }

        protected virtual SCUMMParameter GetVarNew()
        {
            return MakeVar(reader.ReadU16LE());
        }

        protected virtual SCUMMParameter GetNumber()
        {
            return new SCUMMParameter(SCUMMParameterType.Number, reader.ReadU16LE());
        }

        protected virtual SCUMMParameter GetNumberSigned()
        {
            return new SCUMMParameter(SCUMMParameterType.Number, reader.ReadS16LE());
        }

        protected SCUMMParameter GetByteVar()
        {
            return new SCUMMParameter(SCUMMParameterType.Var, reader.ReadU8());
        }

        protected SCUMMParameter GetVarOrByte(int opcode, int mask)
        {
            if ((opcode & mask) != 0)
            {
                // Variable
                return GetVar();
            }
            return new SCUMMParameter(SCUMMParameterType.Number, reader.ReadU8());
        }

        protected SCUMMParameter GetVarOrWord(int opcode, int mask)
        {
            if ((opcode & mask) != 0)
            {
                // Variable
                return GetVar();
            }
            return new SCUMMParameter(SCUMMParameterType.Number, reader.ReadU16LE());
        }

        protected SCUMMParameter GetStringOld()
        {
            StringBuilder builder = new StringBuilder();
            bool addSpace = false;
            while (true)
            {
                byte b = reader.ReadU8();
                if (b == 0)
                {
                    break;
                }
                if ((b & 0x80) != 0)
                {
                    addSpace = true;
                }

                var c = b & 0x7f;

                if (c < 8)
                {
                    switch (c)
                    {
                        case 1: // newline
                            builder.Append("\",\"");
                            break;
                        case 3: // wait
                            builder.Append("\":\"");
                            break;
                        case 4: // variable
                            builder.AppendFormat("%N{0}%", GetVar());
                            break;
                        default:
                            builder.AppendFormat("\\{0}", c);
                            break;
                    }
                }
                else
                {
                    builder.Append((char) c);
                }

                if (addSpace)
                {
                    builder.Append(" ");
                    addSpace = false;
                }
            }
            return new SCUMMParameter(SCUMMParameterType.String, builder.ToString());
        }

        protected SCUMMParameter GetStringNew()
        {
            StringBuilder builder = new StringBuilder();
            while (true)
            {
                byte b = reader.ReadU8();
                if (b == 0)
                {
                    break;
                }
                if (b == 0xff || b == 0xfe)
                {
                    byte msg = reader.ReadU8();
                    switch (msg)
                    {
                        case 0x00: // MSGS_end
                            builder.Append("<end>");
                            break;
                        case 0x01: // MSGS_nextline
                            builder.Append("\",\"");
                            break;
                        case 0x02: // MSGS_no_crlf
                            builder.Append("\"+"); // TODO: Make this property outside the string
                            break;
                        case 0x03: // MSGS_wait
                            builder.Append("\":\"");
                            break;
                        case 0x04: // MSGS_variable
                            builder.AppendFormat("%N{0}%", GetVar());
                            break;
                        case 0x05: // MSGS_verb
                            builder.AppendFormat("%V{0}%", GetNumber());
                            break;
                        case 0x06: // MSGS_actor_object
                            builder.AppendFormat("%A{0}%", GetNumber());
                            break;
                        case 0x07: // MSGS_string
                            builder.AppendFormat("%S{0}%", GetNumber());
                            break;
                        case 0x08: // MSGS_verbnextline
                            builder.Append("<verbnextline>");
                            break;
                        case 0x09: // MSGS_actor_animation
                            builder.AppendFormat("%a{0}%", GetNumber());
                            break;
                        case 0x0a: // FOA: Talkie info
                            builder.AppendFormat("%t{0},{1},{2},{3},{4},{5},{6}%", GetNumber(), GetNumber(), GetNumber(), GetNumber(), GetNumber(), GetNumber(), GetNumber());
                            break;
                        case 0x0c: // FOA: Color change
                            builder.AppendFormat("%c{0}%", GetNumber());
                            break;
                        case 0x0d:
                            builder.AppendFormat("<unknown>{0}", GetNumber());
                            break;
                        case 0x0e: // FOA: Charset change
                            builder.AppendFormat("%f{0}%", GetNumber());
                            break;
                    }
                }
                else
                {
                    builder.Append((char)b);
                }
            }
            return new SCUMMParameter(SCUMMParameterType.String, builder.ToString());
        }


        private SCUMMParameter MakeVar(int value)
        {
            SCUMMParameter index = null;

            if ((value & 0x2000) != 0)
            {
                int i = reader.ReadU16LE();
                index = (i & 0x2000) != 0 ? MakeVar(i & 0xdfff) : new SCUMMParameter(SCUMMParameterType.Number, i & 0x0fff);

                value = value & 0xdfff;
            }

            if ((value & 0x8000) != 0)
            {
                return new SCUMMParameter(SCUMMParameterType.BitVar, value & 0x7fff, index);
            }

            if ((value & 0x4000) != 0)
            {
                return new SCUMMParameter(SCUMMParameterType.LocVar, value & 0x0fff, index);
            }

            return new SCUMMParameter(SCUMMParameterType.Var, value);
        }
    }
}
