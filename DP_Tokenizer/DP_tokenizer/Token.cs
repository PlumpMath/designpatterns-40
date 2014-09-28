using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DP_Tokenizer
{
    public class Token
    {
        public int LineNumber { get; private set; }
        public int LinePosition { get; private set; }
        public int Level { get; private set; }
        public string Value { get; private set; }
        public TokenType Type { get; private set; }
        public String PartnerValue { get { return (Partner != null) ? Partner.Value : ""; } set { Partner.Value = value; } }
        public Token Partner { get; set; }

        public Token(int lineNumber, int linePosition, int level, string value, TokenType type, Token partner)
        {
            LineNumber      = lineNumber;
            LinePosition    = linePosition;
            Level           = level;
            Value           = value;
            Type            = type;
            Partner         = partner;
        }
    }
}
