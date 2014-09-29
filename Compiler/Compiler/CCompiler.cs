using System;
using System.Linq;
using DP_Tokenizer;

namespace Compiler
{
    class CCompiler
    {
        private readonly TokenList<Token> _tokenList;
        private TokenList<Token>.Node<Token> _currentToken;

        private readonly TokenList<object[]> _compileCommands;

        // Label stacks
        private LabelStack _endIfLabels;
        private LabelStack _elseLabels;
        private LabelStack _beginWhileLabels;
        private LabelStack _endWhileLabels;

        public CCompiler(TokenList<Token> tokenList)
        {
            _tokenList = tokenList;
            _compileCommands = new TokenList<object[]>();
            
            // Label stacks
            _endIfLabels = new LabelStack("ENDIF_");
            _elseLabels = new LabelStack("ELSE_");
            _beginWhileLabels = new LabelStack("BEGINWHILE_");
            _endWhileLabels = new LabelStack("ENDWHILE_");
        }

        private Token GetNext()
        {
            return _currentToken == null ? (_currentToken = _tokenList.Head).Data : (_currentToken.Next != null ? (_currentToken = _currentToken.Next).Data : null);
        }

        private Token PeakNext()
        {
            return _currentToken == null ? _tokenList.Head.Data : (_currentToken.Next != null ? _currentToken.Next.Data : null);
        }

        public void Compile()
        {
            while (PeakNext() != null)
            {
                ParseStatement();
            }
        }

        public TokenList<object[]> GetCompilerTokens()
        {
            return _compileCommands;
        }

        private void Match(TokenType token)
        {
            if (PeakNext() == null)
                throw new Exception("Expected " + token.ToString());

            Token actual = GetNext();
            if (actual.Type != token)
            {
                throw new Exception("Expected " + token.ToString());
            }
        }

        private void ParseStatement()
        {
            switch (PeakNext().Type)
            {
                case TokenType.If: 
                    ParseIfStatement();
                    break;
                case TokenType.Identifier:
                    ParseAssignStatement();
                    break;
                case TokenType.While:
                    ParseWhileStatement();
                    break;
                default:
                    throw new Exception("Expected statement identifier");
            }
        }

        private void ParseIfStatement()
        {
            var token = GetNext();
            if (token.Type == TokenType.If)
            {
                if (token.Partner != null)
                {
                    ParseIfElse();
                    return;
                }
            }
            else
                throw new Exception("Expected If identifier");

            Match(TokenType.OpenParenthesis);

            object condition = ParseExpression();

            Match(TokenType.CloseParenthesis);

            _compileCommands.AddLast(new [] {"$if", condition, new[] {"$goto", _endIfLabels.Push()} });

            bool hasBrackets = false;
            if (PeakNext().Type == TokenType.OpenCurlyBracket)
            {
                GetNext();
                hasBrackets = true;
            }

            ParseStatement();

            if (hasBrackets)
                Match(TokenType.CloseCurlyBracket);

            _compileCommands.AddLast(new[] { _endIfLabels.Pop() });
        }

        private void ParseIfElse()
        {
            Match(TokenType.OpenParenthesis);

            object condition = ParseExpression();

            Match(TokenType.CloseParenthesis);

            bool hasBrackets = false;
            if (PeakNext().Type == TokenType.OpenCurlyBracket)
            {
                GetNext();
                hasBrackets = true;
            }

            ParseStatement();

            if (hasBrackets)
                Match(TokenType.CloseParenthesis);

            _compileCommands.AddLast(new[] { "$ifElse", condition, new[] {"$goto", _elseLabels.Push()} });

            Match(TokenType.Else);
            bool elseHasBrackets = false;
            if (PeakNext().Type == TokenType.OpenCurlyBracket)
            {
                GetNext();
                elseHasBrackets = true;
            }

            ParseStatement();

            if (elseHasBrackets)
                Match(TokenType.CloseCurlyBracket);

            _compileCommands.AddLast(new []{ _elseLabels.Pop() });
        }


        private void ParseWhileStatement()
        {
            Match(TokenType.While);
            Match(TokenType.OpenParenthesis);

            _compileCommands.AddLast(new [] {_beginWhileLabels.Push()} );

            object condition = ParseExpression();

            Match(TokenType.CloseParenthesis);

            _compileCommands.AddLast(new [] {"$while", condition, new []{"$goto", _endWhileLabels.Push()} });

            bool hasBrackets = false;
            if (PeakNext().Type == TokenType.OpenCurlyBracket)
            {
                GetNext();
                hasBrackets = true;
            }

            ParseStatement();

            if (hasBrackets)
                Match(TokenType.CloseCurlyBracket);

            _compileCommands.AddLast(new[] { "$goto", _beginWhileLabels.Pop() });
            _compileCommands.AddLast(new[] { _endWhileLabels.Pop() });
        }

        private void ParseAssignStatement()
        {
            var token = GetNext(); // type or variable-name

            Symbol lValue;
            if (token.Type == TokenType.Identifier)
            {
                lValue = SymbolTable.GetSymbol(token.Value);
                if (lValue == null)
                {
                    SymbolTable.Define(token.Value, TokenType.Integer);
                    lValue = SymbolTable.GetSymbol(token.Value);
                }
            }
            else
                throw new Exception("Missing identifier");

            Match(TokenType.Equals);

            object compileExpression = ParseExpression();

            _compileCommands.AddLast(new []{ "$assignment", lValue.Name, compileExpression });

            Match(TokenType.EOL);
        }

        private object ParseTerm()
        {
            Token token = GetNext();

            if (token.Type == TokenType.Integer)
            {
                int value = Int32.Parse(token.Value);
                return value;
            }
            else if (token.Type == TokenType.Identifier)
            {
                string varName = token.Value;
                var variable = SymbolTable.GetSymbol(varName);
                if (variable == null)
                    throw new Exception("Unkown identifier: " + varName);

                return new[] {"$getVariable", varName};
            }

            return null;
        }

        private object ParseExpression()
        {
            object parsedExpr = ParseRelationalExpression();
            while (IsNextTokenLogicalOp())
            {
                Token logicalOp = GetNext();
                object secondParsedExpr = ParseRelationalExpression();
                switch (logicalOp.Value)
                {
                    case "&&":
                        parsedExpr = new[] {"$and", parsedExpr, secondParsedExpr};
                        break;
                    case "||": 
                        parsedExpr = new[] {"$or", parsedExpr, secondParsedExpr};
                        break;
                }
            }
            return parsedExpr;
        }

        private object ParseRelationalExpression()
        {
            object parsedExpr = ParseAddExpression();
            while (IsNextTokenRelationalOp())
            {
                Token relOp = GetNext();
                object secondParsedExpr = ParseAddExpression();

                switch (relOp.Type)
                {
                    case TokenType.LowerThan:
                        parsedExpr = new[] {"$less", parsedExpr, secondParsedExpr};
                        break;
                    case TokenType.LowerOrEqThan:
                        parsedExpr = new[] {"$lessOrEq", parsedExpr, secondParsedExpr};
                        break;
                    case TokenType.GreaterThan:
                        parsedExpr = new[] { "$greater", parsedExpr, secondParsedExpr };
                        break;
                    case TokenType.GreaterOrEqThan:
                        parsedExpr = new[] { "$greaterOrEq", parsedExpr, secondParsedExpr };
                        break;
                    case TokenType.Comparator:
                        parsedExpr = relOp.Value == "==" ? new[] { "$equals", parsedExpr, secondParsedExpr } : new[] { "$notEquals", parsedExpr, secondParsedExpr };
                        break;
                }
            }

            return parsedExpr;
        }

        private object ParseAddExpression()
        {
            object parsedExpr = ParseMulExpression();
            while (IsNextTokenAddOp())
            {
                Token addOp = GetNext();
                object secondParsedExpr = ParseMulExpression();
                switch (addOp.Type)
                {
                    case TokenType.OperatorPlus:
                        parsedExpr = new[] { "$add", parsedExpr, secondParsedExpr };
                        break;
                    case TokenType.OperatorMinus:
                        parsedExpr = new[] { "$min", parsedExpr, secondParsedExpr };
                        break;
                }
            }

            return parsedExpr;
        }

        private object ParseMulExpression()
        {
            object term = ParseTerm();

            while (IsNextTokenMulOp())
            {
                Token mullOp = GetNext();
                switch (mullOp.Type)
                {
                    case TokenType.OperatorMultiply:
                        term = new[] { "$mul", term, ParseTerm() };
                        break;
                    case TokenType.OperatorDivide:
                        term = new[] { "$div", term, ParseTerm() };
                        break;
                    case TokenType.OperatorRaised:
                        term = new[] { "$raise", term, ParseTerm() };
                        break;
                }
            }

            return term;
        }


        private bool IsNextTokenMulOp()
        {
            return
                new[] {TokenType.OperatorMultiply, TokenType.OperatorRaised, TokenType.OperatorDivide}.Contains(
                    PeakNext().Type);
        }

        private bool IsNextTokenAddOp()
        {
            return
                new[] { TokenType.OperatorPlus, TokenType.OperatorMinus }.Contains(
                    PeakNext().Type);
        }

        private bool IsNextTokenRelationalOp()
        {
            return
                new[]
                {
                    TokenType.Comparator, TokenType.GreaterOrEqThan, TokenType.GreaterThan, TokenType.LowerOrEqThan,
                    TokenType.LowerThan
                }.Contains(PeakNext().Type);
        }

        private bool IsNextTokenLogicalOp()
        {
            return new[] {TokenType.Logical}.Contains(PeakNext().Type);
        }
    }
}
