using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DP_Tokenizer
{
    class RegexMatcher : IMatcher
    {
        private readonly Regex regex;

        public RegexMatcher(string regexString)
        {
            regex = new Regex(string.Format("^({0})", regexString));
        }

        public int Match(string text)
        {
            Match match = regex.Match(text);
            return match.Success ? match.Length : 0;
        }

        public override string ToString()
        {
            return regex.ToString();
        }
    }
}
