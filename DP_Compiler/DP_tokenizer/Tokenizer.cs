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
        private readonly List<TokenPartner> _tokenPartners;

        private TokenList<Token> tokenList;

        private int _lineNumber  = 0;
        private int _linePostion = 1;
        private int _level       = 1;
        private string _lineRemaining;

        public Tokenizer(TextReader reader, List<TokenDefinition> tokenDefinitions, List<TokenPartner> tokenPartners)
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
                        if (tokenDefinition.tokenType == TokenType.Else || tokenDefinition.tokenType == TokenType.ElseIf ||
                            tokenDefinition.tokenType == TokenType.CloseBracket || tokenDefinition.tokenType == TokenType.While ||
                            tokenDefinition.tokenType == TokenType.CloseCurlyBracket || tokenDefinition.tokenType == TokenType.CloseParenthesis)
                            partner = findPartner(tokenDefinition.tokenType, _level);

                        Token token = new Token(_lineNumber, _linePostion, _level, tokenValue, tokenDefinition.tokenType, partner);
                        tokenList.AddLast(token);

                        // Add token to the found partner
                        if (partner != null)
                            partner.Partner = token;

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

        private Token findPartner(TokenType tokenType, int level)
        {
            Token token = null;

            foreach (TokenPartner tokenPartner in _tokenPartners)
            {

                if (tokenPartner.Token == tokenType)
                {
                    foreach (Token lToken in tokenList)
                    {
                        if (lToken.Type == tokenPartner.Partner && lToken.Level == level && lToken.Partner == null)
                            return lToken;
                    }
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
