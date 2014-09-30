using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DP_Tokenizer;

namespace VirtualMachine.Interface
{
    interface ICodeGenerator
    {
        void runShow(object message);
        void runIf(object condition, object[] endLocation);
        void runIfElse(object condition, object[] elseLocation);
        void runWhile(object condition, object[] endLocation);
        void runGoto(dynamic gotoLocation);
        void runAssignment(string varName, object expression);
        object runGetVariable(string varName);
        bool runAnd(object condition1, object condition2);
        bool runOr(object condition1, object condition2);
        bool runLess(object condition1, object condition2);
        bool runLessOrEq(object condition1, object condition2);
        bool runGreater(object condition1, object condition2);
        bool runGreaterOrEq(object condition1, object condition2);
        bool runEquals(object condition1, object condition2);
        bool runNotEquals(object condition1, object condition2);
        dynamic runAdd(object value1, object value2);
        dynamic runMin(object value1, object value2);
        dynamic runMul(object value1, object value2);
        dynamic runDiv(object value1, object value2);
        dynamic runRaise(object value1, object value2);
        dynamic runUniPlus(object value);
        dynamic runUniMin(object value);
    }
}
