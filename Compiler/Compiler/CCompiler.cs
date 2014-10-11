using System;
using System.Linq;
using System.Text.RegularExpressions;
using DP_Tokenizer;
using Compiler.Node;

namespace Compiler
{
    public class CCompiler
    {
        private readonly TokenList<Token> _tokenList;
        private TokenList<Token>.Node<Token> _currentToken;

        private readonly TokenList<BaseNode> _compileCommands;
        private readonly SymbolTable _symbolTable;

        // Label stacks
        private LabelStack _endIfLabels;
        private LabelStack _elseLabels;
        private LabelStack _beginWhileLabels;
        private LabelStack _endWhileLabels;

        public CCompiler(TokenList<Token> tokenList)
        {
            _tokenList = tokenList;
            _compileCommands = new TokenList<BaseNode>();
            _symbolTable = new SymbolTable();
            
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

        private Token PeekNext()
        {
            return _currentToken == null ? _tokenList.Head.Data : (_currentToken.Next != null ? _currentToken.Next.Data : null);
        }

        private Token PeekNextTwo()
        {
            return _currentToken == null
                ? ((_tokenList.Head.Next != null) ? _tokenList.Head.Next.Data : null)
                : (_currentToken.Next.Next != null ? _currentToken.Next.Next.Data : null);
        }

        public void Compile()
        {
            while (PeekNext() != null)
            {
                ParseStatement();
            }
        }

        public TokenList<BaseNode> GetCompilerTokens()
        {
            return _compileCommands;
        }

        public SymbolTable GetSymbolTable()
        {
            return _symbolTable;
        }

        private void Match(TokenType token)
        {
            if (PeekNext() == null)
                throw new Exception("Expected " + token.ToString());

            Token actual = GetNext();
            if (actual.Type != token)
            {
                throw new Exception("Expected " + token.ToString());
            }
        }

        private void ParseStatement()
        {
            switch (PeekNext().Type)
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
                case TokenType.Show:
                    ParseShow();
                    break;
                default:
                    throw new Exception("Expected statement identifier");
            }
        }

        private void ParseShow()
        {
            Match(TokenType.Show);
            Match(TokenType.OpenParenthesis);

            object parameters = ParseExpression();

            Match(TokenType.CloseParenthesis);
            Match(TokenType.EOL);

            _compileCommands.AddLast(new ShowNode("$show",parameters)); //new[]{"$show", parameters});
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

            _compileCommands.AddLast(new DoNothingNode("Do Nothing")); //new[] { "Do Nothing" });
            var insertIfBefore = _compileCommands.Tail;

            bool hasBrackets = false;
            if (PeekNext().Type == TokenType.OpenCurlyBracket)
            {
                GetNext();
                hasBrackets = true;
            }

            do
            {
                ParseStatement();
            } while (hasBrackets && PeekNext().Type != TokenType.CloseCurlyBracket);

            if (hasBrackets)
                Match(TokenType.CloseCurlyBracket);

            _compileCommands.AddLast(new DoNothingNode("Do Nothing")); //new[] { "Do Nothing" });
            _compileCommands.AddBefore(insertIfBefore, new CompilerNode("$if", condition, new ConditionalJumpNode("$goto",_compileCommands.Tail))); //insertIfBefore, new[] { "$if", condition, new object[] { "$goto", _compileCommands.Tail } });
        }

        private void ParseIfElse()
        {
            Match(TokenType.OpenParenthesis);

            object condition = ParseExpression();

            Match(TokenType.CloseParenthesis);


            _compileCommands.AddLast(new DoNothingNode("Do Nothing")); //new[] { "Do Nothing" });
            var insertIfBefore = _compileCommands.Tail;

            bool hasBrackets = false;
            if (PeekNext().Type == TokenType.OpenCurlyBracket)
            {
                GetNext();
                hasBrackets = true;
            }

            do
            {
                ParseStatement();
            } while (hasBrackets && PeekNext().Type != TokenType.CloseCurlyBracket);

            if (hasBrackets)
                Match(TokenType.CloseCurlyBracket);

            Match(TokenType.Else);

            _compileCommands.AddLast(new DoNothingNode("Do Nothing")); //new[] { "Do Nothing" });
            var elseGoto = _compileCommands.Tail;

            bool elseHasBrackets = false;
            if (PeekNext().Type == TokenType.OpenCurlyBracket)
            {
                GetNext();
                elseHasBrackets = true;
            }

            do
            {
                ParseStatement();
            } while (elseHasBrackets && PeekNext().Type != TokenType.CloseCurlyBracket);

            if (elseHasBrackets)
                Match(TokenType.CloseCurlyBracket);

            _compileCommands.AddLast(new DoNothingNode("Do Nothing")); //new[] { "Do Nothing" });

            // Add for if the if was true, goto the end of the if. The last pushed token. Continue code from there
            _compileCommands.AddBefore(elseGoto, new ConditionalJumpNode("$goto", _compileCommands.Tail));//elseGoto, new object[] {"$goto", _compileCommands.Tail});

            _compileCommands.AddBefore(insertIfBefore, new CompilerNode("$ifElse", condition, new ConditionalJumpNode("$goto",elseGoto))); //insertIfBefore, new[] { "$ifElse", condition, new object[] { "$goto", elseGoto} });
        }


        private void ParseWhileStatement()
        {
            Match(TokenType.While);
            Match(TokenType.OpenParenthesis);

            _compileCommands.AddLast(new DoNothingNode("Do Nothing")); //new [] {"Do Nothing"} );
            var insertWhileAfter = _compileCommands.Tail;

            object condition = ParseExpression();

            Match(TokenType.CloseParenthesis);
            
            bool hasBrackets = false;
            if (PeekNext().Type == TokenType.OpenCurlyBracket)
            {
                GetNext();
                hasBrackets = true;
            }

            do
            {
                ParseStatement();
            } while (hasBrackets && PeekNext().Type != TokenType.CloseCurlyBracket);

            if (hasBrackets)
                Match(TokenType.CloseCurlyBracket);

            _compileCommands.AddLast(new ConditionalJumpNode("$goto", insertWhileAfter)); //new object[] { "$goto", insertWhileAfter });
            _compileCommands.AddLast(new DoNothingNode("Do nothing")); //new[] { "Do Nothing" });

            _compileCommands.AddAfter(insertWhileAfter, new CompilerNode("$while",condition, new ConditionalJumpNode("$goto",_compileCommands.Tail))); //insertWhileAfter, new[] { "$while", condition, new object[] { "$goto", _compileCommands.Tail } });
        }

        private void ParseAssignStatement()
        {
            if (IsSecondTokenUniOp())
            {
                BaseNode uniParse = (BaseNode)ParseExpression();
                _compileCommands.AddLast(uniParse);
                return;
            }

            var token = GetNext(); // type or variable-name

            Symbol lValue;
            if (token.Type == TokenType.Identifier)
            {
                lValue = _symbolTable.GetSymbol(token.Value);
                if (lValue == null)
                {
                    _symbolTable.Define(token.Value, TokenType.Integer);
                    lValue = _symbolTable.GetSymbol(token.Value);
                }
            }
            else
                throw new Exception("Missing identifier");

            Match(TokenType.Equals);

            object compileExpression = ParseExpression();

            _compileCommands.AddLast(new CompilerNode("$assignment", lValue.Name, compileExpression)); //new []{ "$assignment", lValue.Name, compileExpression });

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
            else if (token.Type == TokenType.String)
            {
                string value = token.Value;
                value = Regex.Replace(value, "^\"", "");
                value = Regex.Replace(value, "\"$", "");
                return value;
            }
            else if (token.Type == TokenType.Identifier)
            {
                string varName = token.Value;
                var variable = _symbolTable.GetSymbol(varName);
                if (variable == null)
                    throw new Exception("Unkown identifier: " + varName);

                return new object[] {"$getVariable", varName};
            }
            else if (token.Type == TokenType.OpenParenthesis)
            {
                object expr = ParseExpression();
                Match(TokenType.CloseParenthesis);
                return expr;
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
            object term = ParseUniExpression();
            while (IsNextTokenMulOp())
            {
                Token mullOp = GetNext();
                object secondTerm = ParseUniExpression();
                switch (mullOp.Type)
                {
                    case TokenType.OperatorMultiply:
                        term = new[] { "$mul", term, secondTerm };
                        break;
                    case TokenType.OperatorDivide:
                        term = new[] { "$div", term, secondTerm };
                        break;
                    case TokenType.OperatorRaised:
                        term = new[] { "$raise", term, secondTerm };
                        break;
                }
            }

            return term;
        }

        private object ParseUniExpression()
        {
            object term = ParseTerm();
            while (IsNextTokenUniOp())
            {
                Token mullOp = GetNext();
                switch (mullOp.Type)
                {
                    case TokenType.UniOperatorPlus:
                        term = new[] { "$uniPlus", term}; Match(TokenType.EOL);
                        break;
                    case TokenType.UniOperatorMinus:
                        term = new[] { "$uniMin", term}; Match(TokenType.EOL);
                        break;
                }
            }
            return term;
        }


        private bool IsNextTokenMulOp()
        {
            return
                new[] {TokenType.OperatorMultiply, TokenType.OperatorRaised, TokenType.OperatorDivide}.Contains(
                    PeekNext().Type);
        }

        private bool IsNextTokenAddOp()
        {
            return
                new[] { TokenType.OperatorPlus, TokenType.OperatorMinus }.Contains(
                    PeekNext().Type);
        }

        private bool IsNextTokenRelationalOp()
        {
            return
                new[]
                {
                    TokenType.Comparator, TokenType.GreaterOrEqThan, TokenType.GreaterThan, TokenType.LowerOrEqThan,
                    TokenType.LowerThan
                }.Contains(PeekNext().Type);
        }

        private bool IsNextTokenLogicalOp()
        {
            return new[] {TokenType.Logical}.Contains(PeekNext().Type);
        }

        private bool IsNextTokenUniOp()
        {
            return new[] { TokenType.UniOperatorPlus, TokenType.UniOperatorMinus }.Contains(PeekNext().Type);
        }

        private bool IsSecondTokenUniOp()
        {
            return new[] { TokenType.UniOperatorPlus, TokenType.UniOperatorMinus }.Contains(PeekNextTwo().Type);
        }
    }
}
