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
            List<TokenDefinition> definitions = new List<TokenDefinition>
            {
                new TokenDefinition("if", TokenType.If),
                new TokenDefinition("else", TokenType.Else),
                new TokenDefinition("while", TokenType.While),
                new TokenDefinition("do", TokenType.Do),
                new TokenDefinition("for", TokenType.ForLoop),
                new TokenDefinition("show", TokenType.Show),
                new TokenDefinition("(sin|cos|exp|ln|sqrt)\b", TokenType.Function),
                new TokenDefinition("\\+{2}", TokenType.UniOperatorPlus),
                new TokenDefinition("-{2}", TokenType.UniOperatorMinus),
                new TokenDefinition("\\+(?!\\+)", TokenType.OperatorPlus),
                new TokenDefinition("-(?!-)", TokenType.OperatorMinus),
                new TokenDefinition("\\*", TokenType.OperatorMultiply),
                new TokenDefinition("\\^", TokenType.OperatorRaised),
                new TokenDefinition("\\/", TokenType.OperatorDivide),
                new TokenDefinition("[0-9]+", TokenType.Integer),
                new TokenDefinition("[0-9]{1,12}(?:\\.[0-9]{1,12}|(?:e|E)-?[1-9]{1,3})", TokenType.Double),
                new TokenDefinition("true|false", TokenType.Boolean),
                new TokenDefinition("\\(", TokenType.OpenParenthesis),
                new TokenDefinition("\\)", TokenType.CloseParenthesis),
                new TokenDefinition("\\{", TokenType.OpenCurlyBracket),
                new TokenDefinition("\\}", TokenType.CloseCurlyBracket),
                new TokenDefinition("\\[", TokenType.OpenBracket),
                new TokenDefinition("\\]", TokenType.CloseBracket),
                new TokenDefinition("==|!=", TokenType.Comparator),
                new TokenDefinition("<", TokenType.LowerThan),
                new TokenDefinition(">", TokenType.GreaterThan),
                new TokenDefinition("=", TokenType.Equals),
                new TokenDefinition("<=", TokenType.LowerOrEqThan),
                new TokenDefinition(">=", TokenType.GreaterOrEqThan),
                new TokenDefinition("\\|\\||&&", TokenType.Logical),
                new TokenDefinition("\\;", TokenType.EOL),
                new TokenDefinition("\".+\"", TokenType.String),
                new TokenDefinition("[a-zA-Z][a-zA-Z0-9_]*", TokenType.Identifier)
            };

            return definitions;
        }

        public static List<TokenPartner> GetPartners()
        {
            List<TokenPartner> partners = new List<TokenPartner>
            {
                new TokenPartner {Token = TokenType.ElseIf, Partner = TokenType.Else},
                new TokenPartner {Token = TokenType.ElseIf, Partner = TokenType.If},
                new TokenPartner {Token = TokenType.Else, Partner = TokenType.ElseIf},
                new TokenPartner {Token = TokenType.Else, Partner = TokenType.If},
                new TokenPartner {Token = TokenType.While, Partner = TokenType.Do},
                new TokenPartner {Token = TokenType.CloseBracket, Partner = TokenType.OpenBracket},
                new TokenPartner {Token = TokenType.CloseCurlyBracket, Partner = TokenType.OpenCurlyBracket},
                new TokenPartner {Token = TokenType.CloseParenthesis, Partner = TokenType.OpenParenthesis}
            };

            return partners;
        }
    }
}
