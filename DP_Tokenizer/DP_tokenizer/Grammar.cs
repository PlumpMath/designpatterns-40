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

            definitions.Add(new TokenDefinition("sin|cos|exp|ln|sqrt", TokenType.Function)); // function
            definitions.Add(new TokenDefinition("\\(", TokenType.Symbol)); // open bracket
            definitions.Add(new TokenDefinition("\\)", TokenType.Symbol)); // close bracket
            definitions.Add(new TokenDefinition("[+-]", TokenType.Operator)); // plus or minus
            definitions.Add(new TokenDefinition("[*/]", TokenType.Operator)); // mult or divide
            definitions.Add(new TokenDefinition("\\^", TokenType.Operator)); // raised
            definitions.Add(new TokenDefinition("[0-9]+", TokenType.Integer)); // integer number
            definitions.Add(new TokenDefinition("[a-zA-Z][a-zA-Z0-9_]*", TokenType.Identifier)); // variable
            definitions.Add(new TokenDefinition("\\{", TokenType.Symbol)); // open curly bracket
            definitions.Add(new TokenDefinition("\\}", TokenType.Symbol)); // close curly bracket
            definitions.Add(new TokenDefinition("\\[", TokenType.Symbol)); // open blokhaak
            definitions.Add(new TokenDefinition("\\]", TokenType.Symbol)); // close blokhaak
            definitions.Add(new TokenDefinition("\\<", TokenType.Symbol)); // lt
            definitions.Add(new TokenDefinition("\\>", TokenType.Symbol)); // gt
            definitions.Add(new TokenDefinition("\\=", TokenType.Symbol)); // equals
            definitions.Add(new TokenDefinition(";", TokenType.Symbol)); // equals

            return definitions;
        }
    }
}
