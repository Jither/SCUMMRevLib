using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCUMMRevLib.SpecScript
{
    public enum TokenType
    {
        Identifier,
        Number,
        HexNumber,
        String,
        EOF,

        OpPlus,
        OpMinus,
        OpMul,
        OpDiv,
        OpMod,
        OpInc,
        OpDec,
        OpNot,
        OpOr,
        OpAnd,
        OpBitwiseOr,
        OpBitwiseAnd,
        
        OpAssign,
        OpAddAssign,
        OpSubAssign,
        OpMulAssign,
        OpDivAssign,
        OpModAssign,

        OpGreater,
        OpGreaterEquals,
        OpLess,
        OpLessEquals,
        OpEquals,
        OpNotEquals,

        StartBlock,
        EndBlock,
        StartParenthesis,
        EndParenthesis,
        StartBracket,
        EndBracket,

        Colon,
        Terminator,

        KeywordTrue,
        KeywordOffset,
        KeywordOutput,
        KeywordVar,
        KeywordIf,
        KeywordElse,
        KeywordWhile,
        KeywordRead,
        
        KeywordByte,
        KeywordWord,
        KeywordDword,
        KeywordQword,
        KeywordStringZ,
        KeywordChar,
        KeywordSingle,
        KeywordDouble,
        
        KeywordLE,
        KeywordBE,
        KeywordSigned,
        
        KeywordInt,
        KeywordString,
        KeywordFloat,

        Comment,
        Unknown,
    }
}
