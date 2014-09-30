using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualMachine.Interface;

namespace VirtualMachine
{
// ReSharper disable once InconsistentNaming
    public class CSGenerator 
    {
        public void runAssignment(string varName, object expression)
        {
            throw new NotImplementedException();
        }

        public object runGetVariable(string varName)
        {
            throw new NotImplementedException();
        }

        public bool runAnd(object condition1, object condition2)
        {
            throw new NotImplementedException();
        }

        public bool runOr(object condition1, object condition2)
        {
            throw new NotImplementedException();
        }

        public bool runLess(object condition1, object condition2)
        {
            throw new NotImplementedException();
        }

        public bool runLessOrEq(object condition1, object condition2)
        {
            throw new NotImplementedException();
        }

        public bool runGreater(object condition1, object condition2)
        {
            throw new NotImplementedException();
        }

        public bool runGreaterOrEq(object condition1, object condition2)
        {
            throw new NotImplementedException();
        }

        public bool runEquals(object condition1, object condition2)
        {
            throw new NotImplementedException();
        }

        public bool runNotEquals(object condition1, object condition2)
        {
            throw new NotImplementedException();
        }

        public object runAdd(object value1, object value2)
        {
            throw new NotImplementedException();
        }

        public object runMin(object value1, object value2)
        {
            throw new NotImplementedException();
        }

        public object runMul(object value1, object value2)
        {
            throw new NotImplementedException();
        }

        public object runDiv(object value1, object value2)
        {
            throw new NotImplementedException();
        }

        public object runRaise(object value1, object value2)
        {
            throw new NotImplementedException();
        }

        public void runIf(object condition, object[] endLocation)
        {
            throw new NotImplementedException();
        }

        public void runIfElse(object condition, object[] elseLocation)
        {
            throw new NotImplementedException();
        }

        public void runWhile(object condition, object[] endLocation)
        {
            throw new NotImplementedException();
        }

        public void runGoto(object[] gotoLocation)
        {
            throw new NotImplementedException();
        }


        public void runGoto(dynamic gotoLocation)
        {
            throw new NotImplementedException();
        }
    }
}
