using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DP_Tokenizer
{
    class TokenDefinition
    {
        public readonly IMatcher matcher;
        public readonly TokenType tokenType;

        public TokenDefinition(string regexString, TokenType type)
        {
            matcher     = new RegexMatcher(regexString);
            tokenType   = type;
        }
    }
}
