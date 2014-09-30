using System.Collections.Generic;
using System.Linq;
using DP_Tokenizer;

namespace Compiler
{
    public class SymbolTable
    {
        private static readonly Dictionary<string, Symbol> _scope = new Dictionary<string, Symbol>();

        public void Define(string name, TokenType type)
        {
            var symbol = new Symbol
            {
                Name = name,
                Type = type,
                Index = _scope.Count(x => x.Value.Type == type)
            };

            _scope.Add(name, symbol);
        }

        public void Reset()
        {
            _scope.Clear();
        }

        public Symbol GetSymbol(string name)
        {
            return _scope.ContainsKey(name) ? _scope[name] : null;
        }

        public int GetCount(TokenType type)
        {
            return _scope.Count(x => x.Value.Type == type);
        }
    }
}
