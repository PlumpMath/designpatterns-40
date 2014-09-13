using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tokenizer
{
    class Token
    {
        //Info for the token
        public readonly int token;
        public readonly int lineNumber;
        public readonly int linePosition;
        public readonly String text;
        public readonly int level;

        public Token(int _token, int _lineNumber, int _linePosition, String _text, int _level)
        {
            token = _token;
            lineNumber = _lineNumber;
            linePosition = _linePosition;
            text = _text;
            level = _level;
        }
    }
}
