using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace tokenizer
{
    class TokenInfo
    {
        public readonly Regex regex;
        public readonly int token;

        public TokenInfo(Regex _regex, int _token)
        {
            regex = _regex;
            token = _token;
        }
    }
}
