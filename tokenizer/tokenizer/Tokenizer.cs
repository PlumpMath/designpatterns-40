using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace tokenizer
{
    class Tokenizer
    {
        // Linkedlist from c# need to be modified to own class (tokenlist)
        private LinkedList<TokenInfo> tokenInfos;
        private LinkedList<Token> tokens;
        private int linePosition = 1;
        private int level = 1;

        public Tokenizer()
        {
            tokenInfos = new LinkedList<TokenInfo>();
            tokens = new LinkedList<Token>();
        }

        // Add token info to the list
        public void Add(String regex, int token)
        {
            // New regex to get the regex
            tokenInfos.AddLast(new TokenInfo(new Regex("^(" + regex + ")"), token));
        }

        // The tokenize method who is doing the tokenizing
        public void Tokenize(String str)
        {
            // First trim the spaces from the begin and the end
            String tokenizingText = str.Trim();
            tokens.Clear();

            // Check if string is not empty
            while (!tokenizingText.Equals(""))
            {
                Boolean isMatch = false;
                // loop through linkedlist with tokeninfos
                foreach (TokenInfo info in tokenInfos)
                {
                    MatchCollection matches = info.regex.Matches(tokenizingText);
                    if (matches.Count > 0)
                    {
                        // Set match to true and get first match
                        isMatch = true;
                        Match match = matches[0];

                        // Get group collection of match
                        String specificToken = match.Groups[0].ToString();
                        if (specificToken == "(" || specificToken == "{" || specificToken == "[")
                        {
                            level++;
                        }
                        else if (specificToken == ")" || specificToken == "}" || specificToken == "]")
                        {
                            level--;
                        }
                        // Add token to linkedlist
                        tokens.AddLast(new Token(info.token, 1, linePosition, specificToken, level));
                        tokenizingText = tokenizingText.Substring(specificToken.Length).Trim();

                        linePosition += specificToken.Length;

                        break;
                    }
                }
                if (!isMatch)
                {
                    throw new ParserException("Unexpected character in input: " + tokenizingText);
                }
            }
        }

        public LinkedList<Token> GetTokenList()
        {
            return tokens;
        }
    }
}
