using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DP_Tokenizer;

namespace Compiler
{
    public class Symbol
    {
        public string Name { get; set; }
        public TokenType Type { get; set; }
        public int Index { get; set; }
        public dynamic Value { get; set; }
    }

    public enum SymbolKind
    {
        Argument,
        Variable
    }
}
