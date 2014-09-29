using System.Collections.Generic;
using System.Linq;
using DP_Tokenizer;

namespace Compiler
{
    public class SymbolTable
    {
        private static readonly Dictionary<string, Symbol> _scope = new Dictionary<string, Symbol>();

        public static void Define(string name, TokenType type, SymbolKind kind)
        {
            var symbol = new Symbol
            {
                Name = name,
                Type = type,
                Kind = kind,
                Index = _scope.Count(x => x.Value.Kind == kind)
            };

            _scope.Add(name, symbol);
        }

        public static void Reset()
        {
            _scope.Clear();
        }

        public static Symbol GetSymbol(string name)
        {
            return _scope.ContainsKey(name) ? _scope[name] : null;
        }

        public static int GetCount(SymbolKind kind)
        {
            return _scope.Count(x => x.Value.Kind == kind);
        }
    }
}
