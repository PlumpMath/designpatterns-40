using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using DP_Tokenizer;

namespace Compiler
{
    class CCompiler
    {
        private readonly TokenList<Token> _tokenList;
        private TokenList<Token>.Node<Token> _currentToken;

        private readonly TokenList<object[]> _compileCommands;

        public CCompiler(TokenList<Token> tokenList)
        {
            _tokenList = tokenList;
            _compileCommands = new TokenList<object[]>();
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
            Token token;
            while (PeakNext() != null)
            {
                token = PeakNext();
                switch (token.Type)
                {
                    case TokenType.Identifier: CompileAssignStatement();
                        break;
                    case TokenType.If: CompileIfStatement();
                        break;
                    case TokenType.While: CompileWhileStatement();
                        break;
                } 
            }
        }

        private void CompileIfStatement()
        {

        }

        private void CompileWhileStatement()
        {
            var token = GetNext();

            if (token.Type == TokenType.While)
            {
                token = GetNext();
                // After while find open bracket
                if (token.Type == TokenType.OpenParenthesis)
                {
                    token = GetNext();
                    // after open bracket find ID to compile the condition
                    if (token.Type == TokenType.Identifier)
                    {
                        // Compile specific condition
                        object compileCondition = CompileCondition(token);
                        token = GetNext();
                        if (token.Type == TokenType.CloseParenthesis)
                        {
                            token = GetNext();
                            if (token.Type == TokenType.OpenCurlyBracket)
                            {
                                object compileStatement = CompileStatement(token);
                            }
                        }
                    }
                }
            }
        }

        private object CompileCondition(Token token)
        {
            if (IsNextTokenLowerThanSign())
            {
                return CompileLowerThan(token);
            }
            
            return 0;
        }

        private object CompileStatement(Token token)
        {
            return 0;
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
                    SymbolTable.Define(token.Value, TokenType.Integer);
                    lValue = SymbolTable.GetSymbol(token.Value);
                }
            }
            else
                throw new Exception("Missing identifier");

            token = GetNext();
            if (token.Type != TokenType.Equals)
                throw new Exception("Missing '='");

            object compileExpression = ParseExpression();

            _compileCommands.AddLast(new []{ "$assignment", lValue.Name, compileExpression });

            token = GetNext();
            if (token.Type != TokenType.EOL)
                throw new Exception("Expected ';'");
        }

<<<<<<< HEAD
        private object CompileLowerThan(Token token)
        {
            return ParseLowerThan(token);
        }

        private object CompileLowerOrEqThan()
        {
            return 0;
        }

        private object CompileExpression()
        {
            // Room to add other expressions, not needed now

            return ParseAddExpression();
        }

=======
>>>>>>> 67aa199181a6e082a3d5db6ab7aaad7d56b61b2a
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

        private object ParseLowerThan(Token token)
        {
            object id;
            if (token.Type == TokenType.Identifier)
            {
                id = token.Value;
                if (IsNextTokenLowerThanSign())
                {
                    token = GetNext();
                    object term = ParseTerm();
                    term = new[] {"$lowerThan", id, term};
                    return term;
                }

            }
            return 0;
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

<<<<<<< HEAD
        private bool IsNextTokenLowerThanSign()
        {
            return
                new[] { TokenType.LowerThan }.Contains(
                    PeakNext().Type);
=======
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
>>>>>>> 67aa199181a6e082a3d5db6ab7aaad7d56b61b2a
        }
    }
}
