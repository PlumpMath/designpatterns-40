using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DP_Tokenizer
{
    static class Grammar
    {
        public static List<TokenDefinition> GetDefinitions()
        {
            List<TokenDefinition> definitions = new List<TokenDefinition>();

            definitions.Add(new TokenDefinition("if", TokenType.Keyword));
            definitions.Add(new TokenDefinition("else", TokenType.Keyword));
            definitions.Add(new TokenDefinition("[a-zA-Z][a-zA-Z0-9_]*", TokenType.Identifier)); // variable
            definitions.Add(new TokenDefinition("sin|cos|exp|ln|sqrt", TokenType.Function)); // function
            definitions.Add(new TokenDefinition("\\(", TokenType.OpenBracket)); // open bracket
            definitions.Add(new TokenDefinition("\\)", TokenType.CloseBracket)); // close bracket
            definitions.Add(new TokenDefinition("[+-](?![+-])", TokenType.OperatorPlus)); // plus or minus
            definitions.Add(new TokenDefinition("\\+{2}", TokenType.OperatorPlus)); // plus or minus
            definitions.Add(new TokenDefinition("-{2}", TokenType.OperatorMinus)); // plus or minus
            definitions.Add(new TokenDefinition("[*/]", TokenType.OperatorMultiply)); // mult or divide
            definitions.Add(new TokenDefinition("\\^", TokenType.OperatorRaised)); // raised
            definitions.Add(new TokenDefinition("[0-9]+", TokenType.Integer)); // integer number
            
            definitions.Add(new TokenDefinition("\\{", TokenType.OpenCurlyBracket)); // open curly bracket
            definitions.Add(new TokenDefinition("\\}", TokenType.CloseCurlyBracket)); // close curly bracket
            definitions.Add(new TokenDefinition("\\[", TokenType.StartIndex)); // open blokhaak
            definitions.Add(new TokenDefinition("\\]", TokenType.EndOfIndex)); // close blokhaak
            definitions.Add(new TokenDefinition("\\<", TokenType.LowerThan)); // lt
            definitions.Add(new TokenDefinition("\\>", TokenType.GreaterThan)); // gt
            definitions.Add(new TokenDefinition("\\=", TokenType.Equals)); // equals
            definitions.Add(new TokenDefinition("\\;", TokenType.EndOfStatement)); // end line

            return definitions;
        }
    }
}
