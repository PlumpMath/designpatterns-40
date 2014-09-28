using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DP_Tokenizer
{
    class ParseExpression
    {
        private float value = 0.0f;
        public ParseExpression(TokenList<Token> tokenList)
        {
            foreach (Token token in tokenList)
            {
                DoParseExpression(token);
                Console.WriteLine(value);
            }
        }

        public bool DoParseExpression(Token token)
        {
            if (!ParseTerm(token, value)) return false;
            if (TokenType.OperatorPlus == token.Type)
            {
                float other = 0.0f;
                if (!ParseTerm(token, other)) return false;
                value += other;
            }
            else if (TokenType.OperatorMinus == token.Type)
            {
                float other = 0.0f;
                if (!ParseTerm(token, other)) return false;
                value -= other;
            }
            return true;
        }

        public bool ParseTerm(Token token, float other) 
        {
            if (!ParsePrimary(token, value)) return false;

            if (TokenType.OperatorMultiply == token.Type)
            {
                //other = 0.0f;
                if (!ParsePrimary(token, other)) return false;
                value *= other;
            }
            else if (TokenType.OperatorDivide == token.Type)
            {
                //other = 0.0f;
                if (!ParsePrimary(token, other)) return false;
                value /= other;
            }
            return true;
        }

        public bool ParsePrimary(Token token, float other)
        {
            if (TokenType.Integer == token.Type)
            {
                value = float.Parse(token.Value);
                return true;
            }
            //else if (Token::LeftParenthesis == token)
            //{
            //if (!ScanExpression(scanner, value)) return false;
            //if (Token::RightParenthesis != scanner.Read()) return false;
            //return true;
            //}
            //else
            //{
            //return false;
            //}
            return true;
        }
    }
}
