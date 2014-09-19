using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DP_Tokenizer
{
    class Tokenizer
    {
        private readonly TextReader _reader;
        private readonly List<TokenDefinition> _tokenDefinitions;
        private Dictionary<String, String> _tokenPartners;

        private TokenList<Token> tokenList;

        private int _lineNumber  = 0;
        private int _linePostion = 1;
        private int _level       = 1;
        private string _lineRemaining;

        public Tokenizer(TextReader reader, List<TokenDefinition> tokenDefinitions, Dictionary<string, string> tokenPartners)
        {
            _reader = reader;
            _tokenDefinitions = tokenDefinitions;
            _tokenPartners = tokenPartners;
            nextLine();
        }

        public void Tokenize()
        {
            tokenList = new TokenList<Token>();

            while (_lineRemaining != null)
            {
                _lineRemaining = _lineRemaining.Trim();
                bool match = false;
    
                foreach(TokenDefinition tokenDefinition in _tokenDefinitions)
                {
                    int matched = tokenDefinition.matcher.Match(_lineRemaining);
                    if (matched > 0)
                    {
                        match = true;

                        string tokenValue = _lineRemaining.Substring(0, matched);

                        if (tokenValue == "(" || tokenValue == "{" || tokenValue == "[")
                            _level++;

                        Token partner = null;
                        if (tokenDefinition.tokenType == TokenType.Keyword || tokenDefinition.tokenType == TokenType.CloseBracket
                            || tokenDefinition.tokenType == TokenType.CloseCurlyBracket || tokenDefinition.tokenType == TokenType.EndOfIndex)
                            partner = findPartner(tokenValue, _level);

                        Token token = new Token(_lineNumber, _linePostion, _level, tokenValue, tokenDefinition.tokenType, partner);
                        tokenList.AddLast(token);

                        if (tokenValue == ")" || tokenValue == "}" || tokenValue == "]")
                            _level--;

                        _linePostion  += matched; 
                        _lineRemaining = _lineRemaining.Substring(matched);
                        if (_lineRemaining.Length == 0)
                            nextLine();

                        break;
                    }
                }

                if (!match)
                {
                    throw new ParserException("Unexpected character '" + _lineRemaining + "' at line " + _lineNumber + " position" + _linePostion);
                }
            } 
        }

        private Token findPartner(string tokenValue, int level)
        {
            Token token = null;

            if (_tokenPartners.ContainsKey(tokenValue))
            {
                string partner = _tokenPartners[tokenValue];

                foreach (Token lToken in tokenList)
                {
                    if (lToken.Value == partner && lToken.Level == level && lToken.Partner == null)
                        return lToken;
                }
            }

            return token;
        }

        private void nextLine()
        {
            do
            {
                _lineRemaining = _reader.ReadLine();
                ++_lineNumber;
                _linePostion = 1;
            }
            while (_lineRemaining != null && _lineRemaining.Length == 0);
        }

        public TokenList<Token> GetTokenList()
        {
            return tokenList;
        }
    }
}
