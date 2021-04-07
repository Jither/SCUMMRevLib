using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Katana.IO;
using SCUMMRevLib.FileFormats;
using SCUMMRevLib.Chunks;

namespace SCUMMRevLib.SpecScript
{
    public class ChunkSpecValue
    {
        public string Name { get; protected set; }
        public string Value { get; protected set; }

        public ChunkSpecValue(string name, string value)
        {
            Name = name;
            Value = value;
        }
    }

    public class Interpreter
    {
        private static Regex rxReplaceSpecialChars = new Regex("[^a-zA-Z0-9_]", RegexOptions.Compiled);

        private Dictionary<int, Action> opcodes;
        private ChunkSpecifications specs;

        private byte[] code;
        private int codePos;
        private BinReader reader;
        private Stack<object> stack = new Stack<object>();
        private Dictionary<int, VariableInfo> variables = new Dictionary<int, VariableInfo>();
        private List<ChunkSpecValue> specValues = new List<ChunkSpecValue>();

        public Interpreter(ChunkSpecifications specs)
        {
            SetupOpcodes();
            this.specs = specs;
        }

        public List<ChunkSpecValue> Interpret(Chunk chunk)
        {
            string id = rxReplaceSpecialChars.Replace(chunk.ChunkTypeId, "_");

            specValues.Clear();

            if (!specs.ContainsKey(id))
            {
                return specValues;
            }

            variables.Clear();

            AddVariable(0, InternalType.Integer, (long)(chunk.Size));

            stack.Clear();

            code = specs[id];
            codePos = 0;

            using (ChunkStream chunkStream = chunk.GetStream())
            {
                using (reader = new BinReader(chunkStream))
                {
                    byte opcode;
                    do
                    {
                        opcode = code[codePos++];

                        Debug.Assert(opcodes.ContainsKey(opcode));

                        opcodes[opcode]();
                        //Trace.TraceInformation("Opcode {0}", opcodes[opcode].Method.Name);
                    } while (opcode != Opcodes.END);
                }
            }
            return specValues;
        }

        private void SetupOpcodes()
        {
            opcodes = new Dictionary<int, Action>
                          {
                              {Opcodes.ADD, Add},
                              {Opcodes.ASSIGN, Assign},
                              {Opcodes.BAND, BitwiseAnd},
                              {Opcodes.BOR, BitwiseOr},
                              {Opcodes.CALL, Call},
                              {Opcodes.DECLARE_VARIABLE, DeclareVariable},
                              {Opcodes.DIV, Divide},
                              {Opcodes.END, End},
                              {Opcodes.EQ, Equal},
                              {Opcodes.GEQ, GreaterThanOrEqual},
                              {Opcodes.GT, GreaterThan},
                              {Opcodes.IF_NOT, IfNot},
                              {Opcodes.JUMP, Jump},
                              {Opcodes.LAND, LogicalAnd},
                              {Opcodes.LEQ, LessThanOrEqual},
                              {Opcodes.LOR, LogicalOr},
                              {Opcodes.LT, LessThan},
                              {Opcodes.MOD, Modulus},
                              {Opcodes.MUL, Multiply},
                              {Opcodes.NEQ, NotEqual},
                              {Opcodes.NOT, Not},
                              {Opcodes.OFFSET, Offset},
                              {Opcodes.OUTPUT, Output},
                              {Opcodes.PUSH_INTEGER, PushIntegerConstant},
                              {Opcodes.PUSH_STRING, PushStringConstant},
                              {Opcodes.PUSH_VARIABLE, PushVariable},
                              {Opcodes.READ_BE_S, ReadBES},
                              {Opcodes.READ_BE_U, ReadBEU},
                              {Opcodes.READ_LE_S, ReadLES},
                              {Opcodes.READ_LE_U, ReadLEU},
                              {Opcodes.SUB, Subtract}
                          };
        }

        private void Add()
        {
            long a = PopInteger();
            long b = PopInteger();
            PushInteger(a + b);
        }

        private void Assign()
        {
            int index = FetchInteger();
            VariableInfo info = variables[index];
            switch (info.Type)
            {
                case InternalType.Integer:
                    info.Value = PopInteger();
                    break;
                case InternalType.String:
                    info.Value = PopString();
                    break;
                case InternalType.Float:
                    info.Value = PopFloat();
                    break;
            }
        }

        private void BitwiseAnd()
        {
            long b = PopInteger();
            long a = PopInteger();
            PushInteger(b & a);
        }

        private void BitwiseOr()
        {
            long b = PopInteger();
            long a = PopInteger();
            PushInteger(b | a);
        }

        private void Call()
        {
            throw new NotImplementedException();
        }

        private void DeclareVariable()
        {
            int index = FetchInteger();
            InternalType type = (InternalType)FetchInteger();
            AddVariable(index, type);
        }

        private void AddVariable(int index, InternalType type, object value = null)
        {
            VariableInfo info = new VariableInfo {Index = index, Type = type, Value = value};
            variables.Add(index, info);
        }

        private void Divide()
        {
            long b = PopInteger();
            long a = PopInteger();
            PushInteger(a / b);
        }

        private void End()
        {
            // Do nothing for now
        }

        private void Equal()
        {
            // TODO: Strings
            long b = PopInteger();
            long a = PopInteger();
            PushInteger(a == b ? 1 : 0);
        }

        private void GreaterThanOrEqual()
        {
            long b = PopInteger();
            long a = PopInteger();
            PushInteger(a >= b ? 1 : 0);
        }

        private void GreaterThan()
        {
            long b = PopInteger();
            long a = PopInteger();
            PushInteger(a > b ? 1 : 0);
        }

        private void IfNot()
        {
            long condition = PopInteger();
            if (condition == 0)
            {
                Jump();
            }
            else
            {
                FetchInteger(); // Read jump pos regardless
            }
        }

        private void Jump()
        {
            codePos = FetchInteger();
        }

        private void LogicalAnd()
        {
            long b = PopInteger();
            long a = PopInteger();
            PushInteger((a != 0) && (b != 0) ? 1 : 0);
        }

        private void LogicalOr()
        {
            long b = PopInteger();
            long a = PopInteger();
            PushInteger((a != 0) || (b != 0) ? 1 : 0);
        }

        private void LessThanOrEqual()
        {
            long b = PopInteger();
            long a = PopInteger();
            PushInteger(a <= b ? 1 : 0);
        }

        private void LessThan()
        {
            long b = PopInteger();
            long a = PopInteger();
            PushInteger(a < b ? 1 : 0);
        }

        private void Modulus()
        {
            long b = PopInteger();
            long a = PopInteger();
            PushInteger(a % b);
        }

        private void Multiply()
        {
            long b = PopInteger();
            long a = PopInteger();
            PushInteger(a * b);
        }

        private void NotEqual()
        {
            // TODO: Strings
            long b = PopInteger();
            long a = PopInteger();
            PushInteger(a != b ? 1 : 0);
        }

        private void Not()
        {
            long a = PopInteger();
            PushInteger(a == 0 ? 1 : 0);
        }

        private void Offset()
        {
            reader.Position = (uint)PopInteger();
        }

        private void Output()
        {
            string name = FetchString();
            string value = PopAny().ToString();
            specValues.Add(new ChunkSpecValue(name, value));
        }

        private void PushIntegerConstant()
        {
            PushInteger(FetchInteger());
        }

        private void PushStringConstant()
        {
            PushString(FetchString());
        }

        private void PushVariable()
        {
            int index = FetchInteger();
            VariableInfo info = variables[index];
            switch (info.Type)
            {
                case InternalType.Integer:
                    PushInteger((long)info.Value);
                    break;
                case InternalType.String:
                    PushString((string)info.Value);
                    break;
                case InternalType.Float:
                    PushFloat((double)info.Value);
                    break;
            }
        }

        private void Read(bool bigendian, bool signed)
        {
            InputType type = (InputType)code[codePos++];
            switch (type)
            {
                case InputType.Byte:
                    if (signed)
                    {
                        PushInteger(reader.ReadS8());
                    }
                    else
                    {
                        PushInteger(reader.ReadU8());
                    }
                    break;
                case InputType.Word:
                    if (signed)
                    {
                        PushInteger(bigendian ? reader.ReadS16BE() : reader.ReadS16LE());
                    }
                    else
                    {
                        PushInteger(bigendian ? reader.ReadU16BE() : reader.ReadU16LE());
                    }
                    break;
                case InputType.DWord:
                    if (signed)
                    {
                        PushInteger(bigendian ? reader.ReadS32BE() : reader.ReadS32LE());
                    }
                    else
                    {
                        PushInteger(bigendian ? reader.ReadU32BE() : reader.ReadU32LE());
                    }
                    break;
                case InputType.QWord:
                    if (signed)
                    {
                        PushInteger(bigendian ? reader.ReadS64BE() : reader.ReadS64LE());
                    }
                    else
                    {
                        PushInteger(bigendian ? reader.ReadU64BE() : reader.ReadU64LE());
                    }
                    break;
                case InputType.StringZ:
                    PushString(reader.ReadStringZ());
                    break;
                case InputType.Char:
                    long length = PopInteger();
                    PushString(reader.ReadString((uint)length));
                    break;
                case InputType.Single:
                    throw new NotImplementedException("Single reading is not implemented.");
                case InputType.Double:
                    throw new NotImplementedException("Double reading is not implemented.");
            }
        }

        private void ReadBES()
        {
            Read(true, true);
        }

        private void ReadBEU()
        {
            Read(true, false);
        }

        private void ReadLES()
        {
            Read(false, true);
        }

        private void ReadLEU()
        {
            Read(false, false);
        }

        private void Subtract()
        {
            long b = PopInteger();
            long a = PopInteger();
            PushInteger(a - b);
        }

        // Code stream functions
        private string FetchString()
        {
            int pos = codePos;
            byte b;
            int length = -1;
            do
            {
                b = code[pos++];
                length++;
            }
            while (b != 0);
            
            string result = Encoding.UTF8.GetString(code, codePos, length);
            codePos += length + 1; // Plus zero terminator
            return result;
        }

        private int FetchInteger()
        {
            int result = BitConverter.ToInt32(code, codePos);
            codePos += 4;
            return result;
        }


        // Stack functions

        void PushInteger(long value)
        {
            stack.Push(value);
        }

        void PushInteger(ulong value)
        {
            stack.Push(value);
        }

        void PushString(string value)
        {
            stack.Push(value);
        }

        void PushFloat(double value)
        {
            stack.Push(value);
        }

        long PopInteger()
        {
            if (stack.Count == 0)
            {
                Error("Stack overflow");
            }
            return (long)stack.Pop();
        }

        string PopString()
        {
            if (stack.Count == 0)
            {
                Error("Stack overflow");
            }
            return (string) stack.Pop();
        }

        double PopFloat()
        {
            if (stack.Count == 0)
            {
                Error("Stack overflow");
            }
            return (double) stack.Pop();
        }

        object PopAny()
        {
            if (stack.Count == 0)
            {
                Error("Stack overflow");
            }
            return stack.Pop();
        }

        private void Error(string message, params object[] args)
        {
            throw new InterpreterException(message, args);
        }
    }
}
