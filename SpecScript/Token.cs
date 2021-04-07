using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCUMMRevLib.SpecScript
{
    public struct Token
    {
        public TokenType Type;
        public string Value;

        public Token(TokenType type)
        {
            Type = type;
            Value = null;
        }

        public Token(TokenType type, string value)
        {
            Type = type;
            Value = value;
        }

        public override string ToString()
        {
            return String.Format("{0} '{1}'", Type, Value);
        }
    }
}
