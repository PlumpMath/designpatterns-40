using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DP_Tokenizer
{
    public enum TokenType
    {
        KeyIdentifier,
        Function,
        Identifier,
        If,
        Else,
        ElseIf,
        ForLoop,
        While,
        Do,
        Return,
        Integer,
        Double,
        Boolean,
        Special,
        EOL,
        OperatorPlus,
        OperatorMinus,
        OperatorDivide,
        OperatorRaised,
        OperatorMultiply,
        UniOperatorPlus,
        UniOperatorMinus,
        GreaterThan,
        LowerThan,
        GreaterOrEqThan,
        LowerOrEqThan,
        Equals,
        NotEqual,
        OpenParenthesis,
        CloseParenthesis,
        OpenCurlyBracket,
        CloseCurlyBracket,
        OpenBracket,
        CloseBracket,
        Comparator,
        Logical,
        Show
    }
}
