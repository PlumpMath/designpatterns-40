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

            definitions.Add(new TokenDefinition("if", TokenType.If));
            definitions.Add(new TokenDefinition("else", TokenType.Else));
            definitions.Add(new TokenDefinition("while", TokenType.While));
            definitions.Add(new TokenDefinition("do", TokenType.Do));
            definitions.Add(new TokenDefinition("for", TokenType.ForLoop));

            definitions.Add(new TokenDefinition("(sin|cos|exp|ln|sqrt)\b", TokenType.Function)); // function
            
            definitions.Add(new TokenDefinition("\\+(?!\\+)", TokenType.OperatorPlus)); // plus
            definitions.Add(new TokenDefinition("-(?!-)", TokenType.OperatorMinus));
            definitions.Add(new TokenDefinition("\\+{2}", TokenType.UniOperatorPlus)); // plus or minus
            definitions.Add(new TokenDefinition("-{2}", TokenType.UniOperatorMinus)); // plus or minus
            definitions.Add(new TokenDefinition("\\*", TokenType.OperatorMultiply)); // mult or divide
            definitions.Add(new TokenDefinition("\\^", TokenType.OperatorRaised)); // raised
            definitions.Add(new TokenDefinition("\\/", TokenType.OperatorDivide)); // raised
            
            definitions.Add(new TokenDefinition("[0-9]+", TokenType.Integer)); // integer number
            definitions.Add(new TokenDefinition("[0-9]{1,12}(?:\\.[0-9]{1,12}|(?:e|E)-?[1-9]{1,3})", TokenType.Double));
            definitions.Add(new TokenDefinition("true|false", TokenType.Boolean));

            definitions.Add(new TokenDefinition("\\(", TokenType.OpenParenthesis)); // open bracket
            definitions.Add(new TokenDefinition("\\)", TokenType.CloseParenthesis)); // close bracket
            definitions.Add(new TokenDefinition("\\{", TokenType.OpenCurlyBracket)); // open curly bracket
            definitions.Add(new TokenDefinition("\\}", TokenType.CloseCurlyBracket)); // close curly bracket
            definitions.Add(new TokenDefinition("\\[", TokenType.OpenBracket)); // open blokhaak
            definitions.Add(new TokenDefinition("\\]", TokenType.CloseBracket)); // close blokhaak
            definitions.Add(new TokenDefinition("<", TokenType.LowerThan)); // lt
            definitions.Add(new TokenDefinition(">", TokenType.GreaterThan)); // gt
            definitions.Add(new TokenDefinition("=", TokenType.Equals)); // equals
            definitions.Add(new TokenDefinition("<=", TokenType.LowerOrEqThan));
            definitions.Add(new TokenDefinition(">=", TokenType.GreaterOrEqThan));
            definitions.Add(new TokenDefinition("==|!=", TokenType.Comparator));
            definitions.Add(new TokenDefinition("\\|\\||&&", TokenType.Comparator));
            definitions.Add(new TokenDefinition("\\;", TokenType.EOL)); // end line

            definitions.Add(new TokenDefinition("[a-zA-Z][a-zA-Z0-9_]*", TokenType.Identifier)); // variable

            return definitions;
        }

        public static List<TokenPartner> GetPartners()
        {
            List<TokenPartner> partners = new List<TokenPartner>();

            partners.Add(new TokenPartner { Token = TokenType.ElseIf, Partner = TokenType.Else });
            partners.Add(new TokenPartner { Token = TokenType.ElseIf, Partner = TokenType.If });
            partners.Add(new TokenPartner { Token = TokenType.Else, Partner = TokenType.ElseIf });
            partners.Add(new TokenPartner { Token = TokenType.Else, Partner = TokenType.If });
            partners.Add(new TokenPartner { Token = TokenType.While, Partner = TokenType.Do });
            partners.Add(new TokenPartner { Token = TokenType.CloseBracket, Partner = TokenType.OpenBracket });
            partners.Add(new TokenPartner { Token = TokenType.CloseCurlyBracket, Partner = TokenType.OpenCurlyBracket });
            partners.Add(new TokenPartner { Token = TokenType.CloseParenthesis, Partner = TokenType.OpenParenthesis });

            return partners;
        }
    }
}
