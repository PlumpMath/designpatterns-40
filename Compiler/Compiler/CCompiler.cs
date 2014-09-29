using System;
using System.Collections.Generic;
using System.Windows.Documents;
using DP_Tokenizer;

namespace Compiler
{
    class CCompiler
    {
        private readonly TokenList<Token> _tokenList;
        private TokenList<Token>.Node<Token> _currentToken;

        private List<string> _compileCommands;

        public CCompiler(TokenList<Token> tokenList)
        {
            _tokenList = tokenList;
            _currentToken = _tokenList.Head;
            _compileCommands = new List<string>();
        }

        private Token GetNext()
        {
            return _currentToken.Next == _tokenList.Head ? null : (_currentToken = _currentToken.Next).Data;
        }

        public void Compile()
        {
            
        }

        private bool ValidateType(TokenType type)
        {
            return type == TokenType.Boolean || type == TokenType.Double || type == TokenType.Integer;
        }

        private void CompileAssignStatement()
        {
            var token = GetNext(); // type or variable-name

            Symbol lValue;
            if (token.Type == TokenType.Identifier)
            {
                lValue = SymbolTable.GetSymbol(token.Value);
                if (lValue == null)
                {
                    SymbolTable.Define(token.Value, TokenType.Integer, SymbolKind.Variable);
                    lValue = SymbolTable.GetSymbol(token.Value);
                }
            }
            else
                throw new Exception("Missing type or identifier");

            token = GetNext();
            if (token.Type != TokenType.Equals)
                throw new Exception("Missing '='");

            CompileExpression();


            token = GetNext();
            if (token.Type != TokenType.EOL)
                throw new Exception("Expected ';'");
        }

        private void CompileExpression()
        {
            
        }

        private void CompileTerm()
        {
            Token token = GetNext();

            if (token.Type == TokenType.Integer)
            {
                int value = Int32.Parse(token.Value);
                // do something
            }
            else if (token.Type == TokenType.Identifier)
            {
                string varName = token.Value;
                var variable = SymbolTable.GetSymbol(varName);
                if (variable == null)
                    throw new Exception("Unkown identifier: " + varName);

                // do something
            }
        }
    }
}
