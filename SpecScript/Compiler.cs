using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace SCUMMRevLib.SpecScript
{
    public class Compiler
    {
        private Scanner scanner;

        private readonly ChunkSpecifications chunks = new ChunkSpecifications();
        private readonly Dictionary<string, VariableInfo> variables = new Dictionary<string, VariableInfo>();
        private int currVarIndex;

        // TODO: Make this unbounded:
        private readonly byte[] code = new byte[64000];
        private int codePosition;

        private static readonly TokenType[] OP_ASSIGNMENT_TYPES = {
                                                             TokenType.OpAssign, TokenType.OpAddAssign,
                                                             TokenType.OpSubAssign, TokenType.OpMulAssign,
                                                             TokenType.OpDivAssign, TokenType.OpModAssign,
                                                             TokenType.OpInc, TokenType.OpDec
                                                         };

        private static readonly TokenType[] VARIABLE_TYPES = {
                                                        TokenType.KeywordInt, TokenType.KeywordString,
                                                        TokenType.KeywordDouble
                                                    };

        public Compiler()
        {
        }

        public ChunkSpecifications Compile(string text)
        {
            scanner = new Scanner(text);

            do
            {
                GlobalDeclaration();
            } while (scanner.CurrType != TokenType.EOF);

            return chunks;
        }

        private void Expected(string expectation)
        {
            Error("Expected {0} but {1} found", expectation, scanner.CurrToken);
        }

        private void Expect(TokenType tokenType)
        {
            if (scanner.CurrType != tokenType)
            {
                Expected(tokenType.ToString());
            }
        }

        private void Match(TokenType tokenType)
        {
            Expect(tokenType);
            scanner.Scan();
        }

        private void Error(string message, params object[] args)
        {
            throw new CompilerException(message, args);
        }

        private void GlobalDeclaration()
        {
            switch (scanner.CurrType)
            {
                case TokenType.Identifier:
                    ChunkDeclaration();
                    break;
                case TokenType.EOF:
                    return;
                default:
                    Expected("chunk declaration");
                    break;
            }
        }

        private void ChunkDeclaration()
        {
            Expect(TokenType.Identifier);
            string chunkIdentifier = scanner.CurrValue;
            if (chunks.ContainsKey(chunkIdentifier))
            {
                Error("Chunk '{0}' already declared.", chunkIdentifier);
            }
            scanner.Scan();
            
            // Reset variables:
            variables.Clear();
            currVarIndex = 0;
            DeclareVariable("chunksize", InternalType.Integer);

            // Clear code:
            Array.Clear(code, 0, code.Length);
            codePosition = 0;

            Statement();

            EmitOpcode(Opcodes.END);
            byte[] chunkCode = new byte[codePosition];
            Array.Copy(code, chunkCode, codePosition);
            chunks.Add(chunkIdentifier, chunkCode);
        }

        private void Statement()
        {
            switch (scanner.CurrType)
            {
                case TokenType.StartBlock:
                    CompoundStatement();
                    break;
                case TokenType.Identifier:
                    AssignmentOrCall();
                    break;
                case TokenType.KeywordVar:
                    VarDeclaration();
                    break;
                case TokenType.KeywordOffset:
                    OffsetStatement();
                    break;
                case TokenType.KeywordOutput:
                    OutputStatement();
                    break;
                case TokenType.KeywordIf:
                    IfStatement();
                    break;
                case TokenType.KeywordWhile:
                    WhileStatement();
                    break;
                case TokenType.KeywordByte:
                case TokenType.KeywordWord:
                case TokenType.KeywordDword:
                case TokenType.KeywordQword:
                case TokenType.KeywordStringZ:
                case TokenType.KeywordChar:
                case TokenType.KeywordSingle:
                case TokenType.KeywordDouble:
                    ChunkVarDeclaration();
                    break;
                default:
                    Expected("statement");
                    break;
            }
        }

        private void CompoundStatement()
        {
            Match(TokenType.StartBlock);
            while (scanner.CurrType != TokenType.EndBlock)
            {
                Statement();
            }
            Match(TokenType.EndBlock);
        }

        private void AssignmentOrCall()
        {
            Expect(TokenType.Identifier);
            string id = scanner.CurrValue;

            scanner.Scan();

            if (scanner.CurrType == TokenType.StartParenthesis)
            {
                // Chunk call: RMHD();
                if (!chunks.ContainsKey(id))
                {
                    Error("Undeclared chunk in call: {0}", id);
                }

                EmitOpcode(Opcodes.CALL);
                EmitString(id);

                scanner.Scan();
                Match(TokenType.EndParenthesis);
                Match(TokenType.Terminator);
                return;
            }

            VariableInfo variableInfo = GetVariable(id);

            if (Array.IndexOf(OP_ASSIGNMENT_TYPES, scanner.CurrType) < 0)
            {
                Expected("assignment");
            }

            TokenType assignmentType = scanner.CurrType;
            scanner.Scan();

            if (assignmentType != TokenType.OpAssign)
            {
                // Push variable value if not simple assignment:
                EmitOpcode(Opcodes.PUSH_VARIABLE);
                EmitVariable(variableInfo);
            }

            switch (assignmentType)
            {
                case TokenType.OpAssign:
                case TokenType.OpAddAssign:
                case TokenType.OpSubAssign:
                case TokenType.OpMulAssign:
                case TokenType.OpDivAssign:
                case TokenType.OpModAssign:
                    CheckOperands(Expression(), variableInfo.Type);
                    break;
                case TokenType.OpInc:
                case TokenType.OpDec:
                    EmitOpcode(Opcodes.PUSH_INTEGER);
                    EmitInteger(1);
                    break;
            }

            switch (assignmentType)
            {
                case TokenType.OpAssign:
                    break;
                case TokenType.OpAddAssign:
                    EmitOpcode(Opcodes.ADD);
                    break;
                case TokenType.OpSubAssign:
                    EmitOpcode(Opcodes.SUB);
                    break;
                case TokenType.OpMulAssign:
                    EmitOpcode(Opcodes.MUL);
                    break;
                case TokenType.OpDivAssign:
                    EmitOpcode(Opcodes.DIV);
                    break;
                case TokenType.OpModAssign:
                    EmitOpcode(Opcodes.MOD);
                    break;
                case TokenType.OpInc:
                    EmitOpcode(Opcodes.ADD);
                    break;
                case TokenType.OpDec:
                    EmitOpcode(Opcodes.SUB);
                    break;
            }

            EmitOpcode(Opcodes.ASSIGN);
            EmitVariable(variableInfo);

            Match(TokenType.Terminator);
        }

        private void VarDeclaration()
        {
            Match(TokenType.KeywordVar);

            Expect(TokenType.Identifier);
            string id = scanner.CurrValue;

            scanner.Scan();
            Match(TokenType.Colon);
            
            if (Array.IndexOf(VARIABLE_TYPES, scanner.CurrType) < 0)
            {
                Expected("variable type");
            }

            InternalType type = InternalType.Invalid;
            
            switch (scanner.CurrType)
            {
                case TokenType.KeywordInt:
                    type = InternalType.Integer;
                    break;
                case TokenType.KeywordDouble:
                    type = InternalType.Float;
                    break;
                case TokenType.KeywordString:
                    type = InternalType.String;
                    break;
            }

            scanner.Scan();

            VariableInfo info = DeclareVariable(id, type);
            EmitVariableDeclaration(info);

            // We might assign in the same statement:
            if (scanner.CurrType == TokenType.OpAssign)
            {
                Match(TokenType.OpAssign);
                InternalType assignedType = Expression();
                EnsureType(assignedType, type);
                
                EmitOpcode(Opcodes.ASSIGN);
                EmitVariable(info);
            }

            Match(TokenType.Terminator);
        }

        private void OutputStatement()
        {
            Match(TokenType.KeywordOutput);
            Expect(TokenType.String);
            string key = scanner.CurrValue;

            scanner.Scan();

            InternalType exprType = Expression();

            EnsureType(exprType, InternalType.String);
            
            EmitOpcode(Opcodes.OUTPUT);
            EmitString(key);
            
            Match(TokenType.Terminator);
        }

        private void OffsetStatement()
        {
            Match(TokenType.KeywordOffset);
            
            Expression();
            
            EmitOpcode(Opcodes.OFFSET);
            
            Match(TokenType.Terminator);
        }

        private void IfStatement()
        {
            Match(TokenType.KeywordIf);
            Match(TokenType.StartParenthesis);
            InternalType exprType = Expression();
            if (exprType != InternalType.Integer)
            {
                Error("Boolean expression required for if statement");
            }
            Match(TokenType.EndParenthesis);
            
            EmitOpcode(Opcodes.IF_NOT);
            int ifJumpPos = codePosition;
            EmitInteger(0); // Temporary offset
            
            Statement();

            if (scanner.CurrType == TokenType.KeywordElse)
            {
                EmitOpcode(Opcodes.JUMP);
                int elseJumpPos = codePosition;
                EmitInteger(0); // Temporary offset

                EmitIntegerAt(ifJumpPos, codePosition); 
                
                scanner.Scan();
                Statement();
                EmitIntegerAt(elseJumpPos, codePosition);
            }
            else
            {
                EmitIntegerAt(ifJumpPos, codePosition);
            }
        }

        private void WhileStatement()
        {
            Match(TokenType.KeywordWhile);
            Match(TokenType.StartParenthesis);
            int exprPos = codePosition;
            InternalType exprType = Expression();

            if (exprType != InternalType.Integer)
            {
                Error("Boolean expression required for while statement");
            }
            Match(TokenType.EndParenthesis);
            
            EmitOpcode(Opcodes.IF_NOT);
            int ifJumpPos = codePosition;
            EmitInteger(0); // Temporary offset
            scanner.Scan();
            Statement();
            EmitOpcode(Opcodes.JUMP);
            EmitInteger(exprPos);
            EmitIntegerAt(ifJumpPos, codePosition);
        }

        private void ChunkVarDeclaration()
        {
            ReadFromChunk();

            Expect(TokenType.String);
            string key = scanner.CurrValue;
            scanner.Scan();

            EmitOpcode(Opcodes.OUTPUT);
            EmitString(key);

            Match(TokenType.Terminator);
        }

        private InternalType ReadFromChunk()
        {
            InternalType internalType = InternalType.Invalid;
            InputType type = InputType.Invalid;
            switch (scanner.CurrType)
            {
                case TokenType.KeywordByte:
                    internalType = InternalType.Integer;
                    type = InputType.Byte;
                    break;
                case TokenType.KeywordWord:
                    internalType = InternalType.Integer;
                    type = InputType.Word;
                    break;
                case TokenType.KeywordDword:
                    internalType = InternalType.Integer;
                    type = InputType.DWord;
                    break;
                case TokenType.KeywordQword:
                    internalType = InternalType.Integer;
                    type = InputType.QWord;
                    break;
                case TokenType.KeywordStringZ:
                    internalType = InternalType.String;
                    type = InputType.StringZ;
                    break;
                case TokenType.KeywordChar:
                    internalType = InternalType.String;
                    type = InputType.Char;
                    break;
                case TokenType.KeywordSingle:
                    internalType = InternalType.Float;
                    type = InputType.Single;
                    break;
                case TokenType.KeywordDouble:
                    internalType = InternalType.Float;
                    type = InputType.Double;
                    break;
                default:
                    Expected("type");
                    break;
            }

            scanner.Scan();

            if (type == InputType.Char)
            {
                // Char may signify an array of char:
                if (scanner.CurrType == TokenType.StartBracket)
                {
                    scanner.Scan();
                    InternalType exprType = Expression();
                    EnsureType(exprType, InternalType.Integer);
                    Match(TokenType.EndBracket);
                }
                else
                {
                    // Default array size = 1
                    EmitOpcode(Opcodes.PUSH_INTEGER);
                    EmitInteger(1);
                }
            }

            bool signed = false;

            if (scanner.CurrType == TokenType.KeywordSigned)
            {
                signed = true;
                scanner.Scan();
            }

            bool littleEndian = true;

            switch (scanner.CurrType)
            {
                case TokenType.KeywordBE:
                    littleEndian = false;
                    scanner.Scan();
                    break;
                case TokenType.KeywordLE:
                    scanner.Scan();
                    break;
            }

            if (littleEndian)
            {
                EmitOpcode(signed ? Opcodes.READ_LE_S : Opcodes.READ_LE_U);
            }
            else
            {
                EmitOpcode(signed ? Opcodes.READ_BE_S : Opcodes.READ_BE_U);
            }

            EmitByte((byte)type);

            return internalType;
        }

        private InternalType Expression()
        {
            InternalType result = ExpressionLogicalAnd();
            while (scanner.CurrType == TokenType.OpOr)
            {
                scanner.Scan();
                CheckOperands(ExpressionLogicalAnd(), result);
                EnsureType(result, InternalType.Integer);
                EmitOpcode(Opcodes.LOR);
            }
            return result;
        }

        private InternalType ExpressionLogicalAnd()
        {
            InternalType result = ExpressionBitwiseOr();
            while (scanner.CurrType == TokenType.OpAnd)
            {
                scanner.Scan();
                CheckOperands(ExpressionBitwiseOr(), result);
                EnsureType(result, InternalType.Integer);
                EmitOpcode(Opcodes.LAND);
            }
            return result;
        }

        private InternalType ExpressionBitwiseOr()
        {
            InternalType result = ExpressionBitwiseAnd();
            while (scanner.CurrType == TokenType.OpBitwiseOr)
            {
                scanner.Scan();
                CheckOperands(ExpressionBitwiseAnd(), result);
                EnsureType(result, InternalType.Integer);
                EmitOpcode(Opcodes.BOR);
            }
            return result;
        }

        private InternalType ExpressionBitwiseAnd()
        {
            InternalType result = ExpressionEquality();
            while (scanner.CurrType == TokenType.OpBitwiseAnd)
            {
                scanner.Scan();
                CheckOperands(ExpressionEquality(), result);
                EnsureType(result, InternalType.Integer);
                EmitOpcode(Opcodes.BAND);
            }
            return result;
        }

        private InternalType ExpressionEquality()
        {
            InternalType result = ExpressionRelational();

            while (scanner.CurrType == TokenType.OpEquals || scanner.CurrType == TokenType.OpNotEquals)
            {
                TokenType tokenType = scanner.CurrType;

                scanner.Scan();
                CheckOperands(ExpressionEquality(), result);
                switch (tokenType)
                {
                    case TokenType.OpEquals:
                        EmitOpcode(Opcodes.EQ);
                        break;
                    case TokenType.OpNotEquals:
                        EmitOpcode(Opcodes.NEQ);
                        break;
                }
            }
            return result;
        }

        private InternalType ExpressionRelational()
        {
            InternalType result = ExpressionAdditive();
            while (scanner.CurrType == TokenType.OpLess || scanner.CurrType == TokenType.OpGreater || scanner.CurrType == TokenType.OpLessEquals || scanner.CurrType == TokenType.OpGreaterEquals)
            {
                TokenType type = scanner.CurrType;
                scanner.Scan();

                CheckOperands(ExpressionAdditive(), result);
                switch (type)
                {
                    case TokenType.OpLess:
                        EmitOpcode(Opcodes.LT);
                        break;
                    case TokenType.OpGreater:
                        EmitOpcode(Opcodes.GT);
                        break;
                    case TokenType.OpLessEquals:
                        EmitOpcode(Opcodes.LEQ);
                        break;
                    case TokenType.OpGreaterEquals:
                        EmitOpcode(Opcodes.GEQ);
                        break;
                }
            }
            return result;
        }

        private InternalType ExpressionAdditive()
        {
            InternalType result = ExpressionMultiplicative();
            while (scanner.CurrType == TokenType.OpPlus || scanner.CurrType == TokenType.OpMinus)
            {
                TokenType type = scanner.CurrType;
                scanner.Scan();

                CheckOperands(ExpressionMultiplicative(), result);

                if (type == TokenType.OpMinus)
                {
                    EnsureType(result, InternalType.Integer | InternalType.Float);
                }

                switch (scanner.CurrType)
                {
                    case TokenType.OpPlus:
                        EmitOpcode(Opcodes.ADD);
                        break;
                    case TokenType.OpMinus:
                        EmitOpcode(Opcodes.SUB);
                        break;
                }
            }
            return result;
        }

        private InternalType ExpressionMultiplicative()
        {
            InternalType result = ExpressionUnary();
            while (scanner.CurrType == TokenType.OpMul || scanner.CurrType == TokenType.OpDiv || scanner.CurrType == TokenType.OpMod)
            {
                TokenType type = scanner.CurrType;
                scanner.Scan();

                CheckOperands(ExpressionUnary(), result);

                switch (type)
                {
                    case TokenType.OpMul:
                        EnsureType(result, InternalType.Integer | InternalType.Float);
                        EmitOpcode(Opcodes.MUL);
                        break;
                    case TokenType.OpDiv:
                        EnsureType(result, InternalType.Integer | InternalType.Float);
                        EmitOpcode(Opcodes.DIV);
                        break;
                    case TokenType.OpMod:
                        EnsureType(result, InternalType.Integer);
                        EmitOpcode(Opcodes.MOD);
                        break;
                }
            }
            return result;
        }

        private InternalType ExpressionUnary()
        {
            TokenType type = TokenType.Unknown;

            if (scanner.CurrType == TokenType.OpMinus || scanner.CurrType == TokenType.OpNot)
            {
                type = scanner.CurrType;
                scanner.Scan();
                ExpressionUnary();
            }

            InternalType result = Factor();
            switch (type)
            {
                case TokenType.OpNot:
                    EnsureType(result, InternalType.Integer);
                    EmitOpcode(Opcodes.NOT);
                    break;
                case TokenType.OpMinus:
                    EnsureType(result, InternalType.Integer | InternalType.Float);
                    EmitOpcode(Opcodes.PUSH_INTEGER);
                    EmitInteger(0);
                    EmitOpcode(Opcodes.SUB);
                    break;

            }
            return result;
        }

        private InternalType Factor()
        {
            InternalType result = InternalType.Invalid;

            switch (scanner.CurrType)
            {
                case TokenType.Identifier:
                    string id = scanner.CurrValue;
                    VariableInfo varInfo = GetVariable(id);
                    EmitOpcode(Opcodes.PUSH_VARIABLE);
                    EmitVariable(varInfo);
                    result = varInfo.Type;
                    scanner.Scan();
                    break;
                case TokenType.Number:
                    EmitOpcode(Opcodes.PUSH_INTEGER);
                    EmitInteger(Convert.ToInt32(scanner.CurrValue));
                    result = InternalType.Integer;
                    scanner.Scan();
                    break;
                case TokenType.HexNumber:
                    EmitOpcode(Opcodes.PUSH_INTEGER);
                    int value = int.Parse(scanner.CurrValue.Substring(2), NumberStyles.AllowHexSpecifier);
                    EmitInteger(value);
                    result = InternalType.Integer;
                    scanner.Scan();
                    break;
                case TokenType.String:
                    result = InternalType.String;
                    EmitOpcode(Opcodes.PUSH_STRING);
                    EmitString(scanner.CurrValue);
                    scanner.Scan();
                    break;
                case TokenType.KeywordRead:
                    scanner.Scan();
                    result = ReadFromChunk();
                    break;
                case TokenType.StartParenthesis:
                    result = Expression();
                    Match(TokenType.EndParenthesis);
                    break;
                default:
                    Expected("factor");
                    break;
            }
            return result;
        }

        private VariableInfo DeclareVariable(string id, InternalType type)
        {
            if (type == InternalType.Invalid)
            {
                Error("Invalid variable type");
            }
            if (variables.ContainsKey(id))
            {
                Error("Variable {0} already declared");
            }
            VariableInfo info = new VariableInfo() {Type = type, Index = currVarIndex++};
            variables.Add(id, info);
            return info;
        }

        private VariableInfo GetVariable(string id)
        {
            if (!variables.ContainsKey(id))
            {
                Error("Undeclared variable: {0}", id);
            }
            return variables[id];
        }

        private void CheckOperands(InternalType t1, InternalType t2)
        {
            if (t1 != t2)
            {
                Error("Incompatible types: {0} and {1}", t1, t2);
            }
        }

        private void EnsureType(InternalType result, InternalType allowed)
        {
            if ((result & allowed) == 0)
            {
                Error("Operator not applicable to these operands");
            }
        }

        private void EmitOpcode(byte opcode)
        {
            code[codePosition++] = opcode;
        }

        private void EmitString(string value)
        {
            byte[] encoded = Encoding.UTF8.GetBytes(value);
            EmitBytes(encoded);
            EmitByte(0);
        }

        private void EmitVariable(VariableInfo info)
        {
            EmitInteger(info.Index);
        }

        private void EmitVariableDeclaration(VariableInfo info)
        {
            EmitOpcode(Opcodes.DECLARE_VARIABLE);
            EmitInteger(info.Index);
            EmitInteger((int)info.Type);
        }

        private void EmitByte(byte value)
        {
            code[codePosition++] = value;
        }

        private void EmitInteger(int value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            EmitBytes(bytes);
        }

        private void EmitIntegerAt(int position, int value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            Array.Copy(bytes, 0, code, position, bytes.Length);
        }

        private void EmitBytes(byte[] value)
        {
            Array.Copy(value, 0, code, codePosition, value.Length);
            codePosition += value.Length;
        }
    }
}
