using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DP_Tokenizer
{
    enum TokenType
    {
        OpenBracket,
        CloseBracket,
        OpenCurlyBracket,
        CloseCurlyBracket,
        StartIndex,
        EndOfIndex,
        Equals,
        EndOfStatement,
        Integer,
        Double,
        GreaterThan,
        LowerThan,
        OperatorPlus,
        OperatorMinus,
        OperatorDivide,
        OperatorRaised,
        OperatorMultiply,
        Keyword,
        Identifier,
        Function
    }
}
