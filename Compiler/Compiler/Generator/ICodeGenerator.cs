using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DP_Tokenizer;

namespace Compiler.Generator
{
    interface ICodeGenerator
    {
        void InitSymbolTables(SymbolTable classSymTable);
        void EmitEnvironment();
        void EmitBootstrapper();

        void Return();
        void BeginWhile();
        void WhileCondition();
        void EndWhile();
        void BeginIf();
        void PossibleElse();
        void EndIf();
        void Assignment(Token varName, bool withArrayIndex);
        void Add();
        void Sub();
        void Mul();
        void Div();
        void Mod();
        void And();
        void Or();
        void Less();
        void Greater();
        void Equal();
        void LessOrEqual();
        void GreaterOrEqual();
        void NotEqual();
        void IntConst(int value);
        void StrConst(string value);
        void True();
        void False();
        void Null();
        void This();
        void Negate();
        void Not();
        void VariableRead(Token varName, bool withArrayIndex);
        void DiscardReturnValueFromLastCall();

    }
}
