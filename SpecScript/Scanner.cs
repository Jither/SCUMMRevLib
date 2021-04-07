using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCUMMRevLib.SpecScript
{
    public class Scanner
    {
        private static readonly char[] OPERATOR_CHARS = { '+', '-', '*', '/', '>', '<', '=', '(', ')', '|', '&', '!', '{', '}', '[', ']', ';', ':' };

        private static readonly char[] HEX_CHARS = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f' };

        private static Dictionary<string, TokenType> KEYWORDS = new Dictionary<string, TokenType> {
                                                        {"true", TokenType.KeywordTrue},
                                                        {"offset", TokenType.KeywordOffset},
                                                        {"output", TokenType.KeywordOutput},
                                                        {"var", TokenType.KeywordVar},

                                                        {"if", TokenType.KeywordIf},
                                                        {"else", TokenType.KeywordElse},
                                                        {"while", TokenType.KeywordWhile},
                                                        
                                                        {"read", TokenType.KeywordRead},

                                                        {"byte", TokenType.KeywordByte},
                                                        {"word", TokenType.KeywordWord},
                                                        {"dword", TokenType.KeywordDword},
                                                        {"qword", TokenType.KeywordQword},
                                                        {"stringz", TokenType.KeywordStringZ},
                                                        {"char", TokenType.KeywordChar},
                                                        {"single", TokenType.KeywordSingle},
                                                        {"double", TokenType.KeywordDouble},

                                                        {"le", TokenType.KeywordLE},
                                                        {"be", TokenType.KeywordBE},
                                                        {"signed", TokenType.KeywordSigned},

                                                        {"string", TokenType.KeywordString},
                                                        {"int", TokenType.KeywordInt},
                                                        {"float", TokenType.KeywordFloat}
        };

        private string text;
        private int position;
        private char currChar;

        public Token CurrToken { get; private set; }
        public TokenType CurrType { get { return CurrToken.Type; } }
        public string CurrValue { get { return CurrToken.Value; } }

        public Scanner(string text)
        {
            this.text = text;
            position = 0;
            Look();
            Scan();
        }

        private void ThrowExpected(string expectation)
        {
            throw new ScannerException("Expected {0}", expectation);
        }

        private void Look()
        {
            currChar = position < text.Length ? text[position++] : '\x00';
        }

        private void SkipWhitespace()
        {
            while (Char.IsWhiteSpace(currChar))
            {
                Look();
            }
        }

        private void SkipLineComment()
        {
            while (currChar != '\x0a')
            {
                Look();
            }
        }

        private void SkipBlockComment()
        {
            int level;
            level = 1;
            do
            {
                switch (currChar)
                {
                    case '/':
                        Look();
                        if (currChar == '*')
                        {
                            level++;
                            Look();
                        }
                        break;
                    case '*':
                        Look();
                        if (currChar == '/')
                        {
                            level--;
                            Look();
                        }
                        break;
                    case '\x00':
                        throw new ScannerException("Unterminated comment");
                    default:
                        Look();
                        break;
                }
            } while (level > 0);
        }

        private Token GetName()
        {
            if (!Char.IsLetter(currChar) && (currChar != '_')) ThrowExpected("name");

            string name = "";
            while (Char.IsLetterOrDigit(currChar) || (currChar == '_'))
            {
                name += currChar;
                Look();
            }
            TokenType type = TokenType.Identifier;
            if (KEYWORDS.ContainsKey(name.ToLowerInvariant()))
            {
                type = KEYWORDS[name.ToLowerInvariant()];
            }
            return new Token(type, name);
        }

        private Token GetNumber()
        {
            if (!Char.IsDigit(currChar)) ThrowExpected("number");

            string number = "";

            // Hex?
            if (currChar == '0')
            {
                number += currChar;
                Look();
                if (currChar == 'x')
                {
                    number += currChar;
                    Look();

                    while (Array.IndexOf(HEX_CHARS, Char.ToLower(currChar)) >= 0)
                    {
                        number += currChar;
                        Look();
                    }

                    return new Token(TokenType.HexNumber, number);
                }
            }

            // Decimal
            while (Char.IsDigit(currChar))
            {
                number += currChar;
                Look();
            }
            return new Token(TokenType.Number, number);
        }

        private Token GetString()
        {
            string value = "";
            if (currChar != '"') ThrowExpected("string literal");

            // Skip the quote:
            Look();

            while (currChar != '"' && currChar != '\x00')
            {
                value += currChar;
                Look();
            }
            if (currChar != '"')
            {
                ThrowExpected("end-of-string");
            }
            Look();
            return new Token(TokenType.String, value);
        }

        private Token GetOperator()
        {
            Token token = new Token(TokenType.Unknown, "");

            switch (currChar)
            {
                case ';':
                    token = new Token(TokenType.Terminator, ";");
                    Look();
                    break;
                case '+':
                    Look();
                    switch (currChar)
                    {
                        case '+': 
                            token = new Token(TokenType.OpInc, "++");
                            Look();
                            break;
                        case '=':
                            token = new Token(TokenType.OpAddAssign, "+=");
                            Look();
                            break;
                        default:
                            token = new Token(TokenType.OpPlus, "+");
                            break;
                    }
                    break;
                case '-':
                    Look();
                    switch (currChar)
                    {
                        case '-':
                            token = new Token(TokenType.OpDec, "--");
                            Look();
                            break;
                        case '=':
                            token = new Token(TokenType.OpSubAssign, "-=");
                            Look();
                            break;
                        default:
                            token = new Token(TokenType.OpMinus, "-");
                            break;
                    }
                    break;
                case '*':
                    Look();
                    switch (currChar)
                    {
                        case '=':
                            token = new Token(TokenType.OpMulAssign, "*=");
                            Look();
                            break;
                        default:
                            token = new Token(TokenType.OpMul, "*");
                            break;
                    }
                    break;
                case '/':
                    Look();
                    switch (currChar)
                    {
                        case '=':
                            token = new Token(TokenType.OpDivAssign, "/=");
                            Look();
                            break;
                        case '/':
                            SkipLineComment();
                            token = new Token(TokenType.Comment);
                            break;
                        case '*':
                            Look();
                            SkipBlockComment();
                            token = new Token(TokenType.Comment);
                            break;
                        default:
                            token = new Token(TokenType.OpDiv, "/");
                            break;
                    }
                    break;
                case '%':
                    Look();
                    switch (currChar)
                    {
                        case '=':
                            token = new Token(TokenType.OpModAssign, "%=");
                            Look();
                            break;
                        default:
                            token = new Token(TokenType.OpMod, "%");
                            break;
                    }
                    break;
                case '|':
                    Look();
                    switch (currChar)
                    {
                        case '|':
                            Look();
                            token = new Token(TokenType.OpOr, "||");
                            break;
                        default:
                            Look();
                            token = new Token(TokenType.OpBitwiseOr);
                            break;
                    }
                    break;
                case '&':
                    Look();
                    switch (currChar)
                    {
                        case '&':
                            Look();
                            token = new Token(TokenType.OpAnd, "&&");
                            break;
                        default:
                            Look();
                            token = new Token(TokenType.OpBitwiseAnd);
                            break;
                    }
                    break;
                case '=':
                    Look();
                    switch (currChar)
                    {
                        case '=':
                            Look();
                            token = new Token(TokenType.OpEquals, "==");
                            break;
                        default:
                            token = new Token(TokenType.OpAssign, "=");
                            break;
                    }
                    break;
                case '(':
                    Look();
                    token = new Token(TokenType.StartParenthesis, "(");
                    break;
                case ')':
                    Look();
                    token = new Token(TokenType.EndParenthesis, ")");
                    break;
                case '{':
                    Look();
                    token = new Token(TokenType.StartBlock, "{");
                    break;
                case '}':
                    Look();
                    token = new Token(TokenType.EndBlock, "}");
                    break;
                case '[':
                    Look();
                    token = new Token(TokenType.StartBracket, "[");
                    break;
                case ']':
                    Look();
                    token = new Token(TokenType.EndBracket, "]");
                    break;
                case ':':
                    Look();
                    token = new Token(TokenType.Colon, ":");
                    break;
            }

            switch (currChar)
            {
                case '>':
                    Look();
                    token = new Token(TokenType.OpGreater, ">");
                    if (currChar == '=')
                    {
                        Look();
                        token = new Token(TokenType.OpGreaterEquals, ">=");
                    }
                    break;
                case '<':
                    Look();
                    token = new Token(TokenType.OpLess, "<");
                    if (currChar == '=')
                    {
                        Look();
                        token = new Token(TokenType.OpLessEquals, "<=");
                    }
                    break;
                case '!':
                    Look();
                    token = new Token(TokenType.OpNot, "!");
                    if (currChar == '=')
                    {
                        Look();
                        token = new Token(TokenType.OpNotEquals, "==");
                    }
                    break;
            }

            if (token.Type == TokenType.Unknown) ThrowExpected("operator");

            return token;
        }

        public void Scan()
        {
            do
            {
                SkipWhitespace();

                if (Char.IsLetter(currChar))
                {
                    CurrToken = GetName();
                }
                else if (Char.IsDigit(currChar))
                {
                    CurrToken = GetNumber();
                }
                else if (Array.IndexOf(OPERATOR_CHARS, currChar) >= 0)
                {
                    CurrToken = GetOperator();
                }
                else if (currChar == '"')
                {
                    CurrToken = GetString();
                }
                else if (currChar == '\x00')
                {
                    CurrToken = new Token(TokenType.EOF);
                }
                else
                {
                    CurrToken = new Token(TokenType.Unknown);
                    Look();
                }

                SkipWhitespace();
            } while (CurrToken.Type == TokenType.Comment); // Don't return comment tokens
        }
    }
}
