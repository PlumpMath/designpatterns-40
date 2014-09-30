using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Reflection;
using Compiler;
using DP_Tokenizer;
using VirtualMachine.Interface;

namespace VirtualMachine
{
    public class VVirtualMachine : ICodeGenerator
    {
        private readonly TokenList<object[]> _compileTokens;
        private readonly SymbolTable _symbols;
        private readonly Dictionary<string, Delegate> _runDelegates;

        private TokenList<object[]>.Node<object[]> _currentToken;

        private object[] GetNext()
        {
            return _currentToken == null ? (_currentToken = _compileTokens.Head).Data : (_currentToken.Next != null ? (_currentToken = _currentToken.Next).Data : null);
        }

        private object[] PeekNext()
        {
            return _currentToken == null ? _compileTokens.Head.Data : (_currentToken.Next != null ? _currentToken.Next.Data : null);
        }

        public VVirtualMachine(TokenList<object[]> compileTokens, SymbolTable symbols)
        {
            _compileTokens = compileTokens;
            _symbols = symbols;
            _runDelegates = getRunDelegates();
        }

        public void RunCode()
        {
            while(PeekNext() != null)
            {
                object[] compileToken = GetNext();

                if (compileToken.Length == 1)
                    continue;

                if (!isDelegate(compileToken))
                    continue;

                var functionName = (string)compileToken[0];
                var paramsObj = new object[compileToken.Length - 1];
                Array.Copy(compileToken, 1, paramsObj, 0, compileToken.Length - 1);

                var delegateToRun = _runDelegates[functionName];
                if (!isAction(delegateToRun))
                    runAction(delegateToRun, paramsObj);
                else
                    runFunction(delegateToRun, paramsObj);
            }
        }

        public bool isDelegate(object[] paramObj)
        {
            var functionName = (string)paramObj[0];
            return _runDelegates.ContainsKey(functionName);
        }

        private bool isAction(Delegate delegateToRun)
        {
            return (delegateToRun.GetType().IsSubclassOf(typeof (MulticastDelegate)) &&
                   delegateToRun.GetType().GetMethod("Invoke").ReturnType == typeof (void));
        }

        private bool checkParameters(Delegate delegateToRun, object[] parameters)
        {
            var parametersExpected = delegateToRun.GetMethodInfo().GetParameters();
            if (parameters.Length < parametersExpected.Length)
                throw new Exception("Expected " + parametersExpected.Length + " parameters but received " + parameters.Length);

            return true;
        }

        private object runFunction(object[] parameters)
        {
            var functionName = (string)parameters[0];
            var paramsObj = new object[parameters.Length - 1];
            Array.Copy(parameters, 1, paramsObj, 0, parameters.Length - 1);

            var delegateToRun = _runDelegates[functionName];
            if (isAction(delegateToRun))
                throw new Exception("Expected Function not Action");

            return runFunction(delegateToRun, paramsObj);
        }

        private void runAction(object[] parameters)
        {
            var functionName = (string)parameters[0];
            var paramsObj = new object[parameters.Length - 1];
            Array.Copy(parameters, 1, paramsObj, 0, parameters.Length - 1);

            var delegateToRun = _runDelegates[functionName];
            if (!isAction(delegateToRun))
                throw new Exception("Expected Action not Function");

            runAction(delegateToRun, paramsObj);
        }

        private void runAction(Delegate delegateToRun, dynamic parameters)
        {
            if (checkParameters(delegateToRun, parameters))
                delegateToRun.DynamicInvoke(parameters);
        }

        private object runFunction(Delegate functionToRun, object[] parameters)
        {
            if (checkParameters(functionToRun, parameters))
               return functionToRun.DynamicInvoke(parameters);
            return null;
        }

        public void runShow(object message)
        {
            if (message.GetType() == typeof(object[]))
                message = runFunction((object[])message);

            dynamic castMessage = message;
            MessageBox.Show("" + castMessage);
        }

        public void runIf(object condition, object[] endLocation)
        {
            if (condition.GetType() == typeof(object[]))
            {
                runIf(runFunction((object[])condition), endLocation);
                return;
            }

            dynamic castCondition = condition;
            if (castCondition)
                return;
            else
                runAction(endLocation);
        }

        public void runIfElse(object condition, object[] elseLocation)
        {
            if (condition.GetType() == typeof(object[]))
            {
                runIfElse(runFunction((object[])condition), elseLocation);
                return;
            }

            dynamic castCondition = condition;
            if (castCondition)
                return;
            else
                runAction(elseLocation);
        }

        public void runWhile(object condition, object[] endLocation)
        {
            if (condition.GetType() == typeof(object[]))
            {
                runWhile(runFunction((object[])condition), endLocation);
                return;
            }

            dynamic castCondition = condition;
            if (castCondition)
                return;
            else
                runAction(endLocation);
        }

        public void runGoto(dynamic gotoLocation)
        {
            _currentToken = gotoLocation;
        }

        public void runAssignment(string varName, dynamic expression)
        {
            if (expression.GetType() == typeof (object[]))
            {
                runAssignment(varName, runFunction((object[])expression));
                return;
            }

            _symbols.GetSymbol(varName).Value = expression;
        }

        public dynamic runGetVariable(string varName)
        {
            return _symbols.GetSymbol(varName).Value;
        }

        public bool runAnd(object condition1, object condition2)
        {
            if (condition1.GetType() == typeof(object[]))
            {
                condition1 = runFunction((object[])condition1);
            }
            if (condition2.GetType() == typeof(object[]))
            {
                condition2 = runFunction((object[])condition2);
            }

            dynamic castCondition1 = condition1;
            dynamic castCondition2 = condition2;

            return castCondition1 && castCondition2;
        }

        public bool runOr(object condition1, object condition2)
        {
            if (condition1.GetType() == typeof(object[]))
            {
                condition1 = runFunction((object[])condition1);
            }
            if (condition2.GetType() == typeof(object[]))
            {
                condition2 = runFunction((object[])condition2);
            }

            dynamic castCondition1 = condition1;
            dynamic castCondition2 = condition2;

            return castCondition1 || castCondition2;
        }

        public bool runLess(object condition1, object condition2)
        {
            if (condition1.GetType() == typeof(object[]))
            {
                condition1 = runFunction((object[])condition1);
            }
            if (condition2.GetType() == typeof(object[]))
            {
                condition2 = runFunction((object[])condition2);
            }

            dynamic castCondition1 = condition1;
            dynamic castCondition2 = condition2;

            return castCondition1 < castCondition2;
        }

        public bool runLessOrEq(object condition1, object condition2)
        {
            if (condition1.GetType() == typeof(object[]))
            {
                condition1 = runFunction((object[])condition1);
            }
            if (condition2.GetType() == typeof(object[]))
            {
                condition2 = runFunction((object[])condition2);
            }

            dynamic castCondition1 = condition1;
            dynamic castCondition2 = condition2;

            return castCondition1 <= castCondition2;
        }

        public bool runGreater(object condition1, object condition2)
        {
            if (condition1.GetType() == typeof(object[]))
            {
                condition1 = runFunction((object[])condition1);
            }
            if (condition2.GetType() == typeof(object[]))
            {
                condition2 = runFunction((object[])condition2);
            }

            dynamic castCondition1 = condition1;
            dynamic castCondition2 = condition2;

            return castCondition1 > castCondition2;
        }

        public bool runGreaterOrEq(object condition1, object condition2)
        {
            if (condition1.GetType() == typeof(object[]))
            {
                condition1 = runFunction((object[])condition1);
            }
            if (condition2.GetType() == typeof(object[]))
            {
                condition2 = runFunction((object[])condition2);
            }

            dynamic castCondition1 = condition1;
            dynamic castCondition2 = condition2;

            return castCondition1 >= castCondition2;
        }

        public bool runEquals(object condition1, object condition2)
        {
            if (condition1.GetType() == typeof(object[]))
            {
                condition1 = runFunction((object[])condition1);
            }
            if (condition2.GetType() == typeof(object[]))
            {
                condition2 = runFunction((object[])condition2);
            }

            return condition1 != condition2;
        }

        public bool runNotEquals(object condition1, object condition2)
        {
            return !runEquals(condition1, condition2);
        }

        public dynamic runAdd(object value1, object value2)
        {
            if (value1.GetType() == typeof(object[]))
            {
                value1 = runFunction((object[])value1);
            }
            if (value2.GetType() == typeof(object[]))
            {
                value2 = runFunction((object[])value2);
            }

            dynamic v1Cast = Convert.ChangeType(value1, value1.GetType());
            dynamic v2Cast = Convert.ChangeType(value2, value2.GetType());

            return v1Cast + v2Cast;
        }

        public dynamic runMin(object value1, object value2)
        {
            if (value1.GetType() == typeof(object[]))
            {
                value1 = runFunction((object[])value1);
            }
            if (value2.GetType() == typeof(object[]))
            {
                value2 = runFunction((object[])value2);
            }

            dynamic v1Cast = Convert.ChangeType(value1, value1.GetType());
            dynamic v2Cast = Convert.ChangeType(value2, value2.GetType());

            return v1Cast - v2Cast;
        }

        public dynamic runMul(object value1, object value2)
        {
            if (value1.GetType() == typeof(object[]))
            {
                value1 = runFunction((object[])value1);
            }
            if (value2.GetType() == typeof(object[]))
            {
                value2 = runFunction((object[])value2);
            }

            dynamic v1Cast = Convert.ChangeType(value1, value1.GetType());
            dynamic v2Cast = Convert.ChangeType(value2, value2.GetType());

            return v1Cast * v2Cast;
        }

        public dynamic runDiv(object value1, object value2)
        {
            if (value1.GetType() == typeof(object[]))
            {
                value1 = runFunction((object[])value1);
            }
            if (value2.GetType() == typeof(object[]))
            {
                value2 = runFunction((object[])value2);
            }

            dynamic v1Cast = Convert.ChangeType(value1, value1.GetType());
            dynamic v2Cast = Convert.ChangeType(value2, value2.GetType());

            return v1Cast / v2Cast;
        }

        public dynamic runRaise(object value1, object value2)
        {
            if (value1.GetType() == typeof(object[]))
            {
                value1 = runFunction((object[])value1);
            }
            if (value2.GetType() == typeof(object[]))
            {
                value2 = runFunction((object[])value2);
            }

            dynamic v1Cast = value1;
            dynamic v2Cast = value2;

            return v1Cast ^ v2Cast;
        }
        public dynamic runUniPlus(object value)
        {
            string varName = "";
            if (value.GetType() == typeof(object[]))
            {
                var vars = (object[])value;
                varName = (string)vars[1];
                value = runFunction((object[])value);
            }

            dynamic vCast = value;
            vCast++;
            runAssignment(varName, vCast);
            return vCast;
        }

        public dynamic runUniMin(object value)
        {
            string varName = "";
            if (value.GetType() == typeof (object[]))
            {
                var vars = (object[]) value;
                varName = (string)vars[1];
                value = runFunction((object[]) value);
            }

            dynamic vCast = value;
            vCast--;
            runAssignment(varName, vCast);
            return vCast;
        }

        private Dictionary<string, Delegate> getRunDelegates()
        {
            return new Dictionary<string, Delegate>()
            {
                {"$show", new Action<object>(runShow)},
                {"$if", new Action<object, object[]>(runIf)},
                {"$ifElse", new Action<object, object[]>(runIfElse)},
                {"$while", new Action<object, object[]>(runWhile)},
                {"$goto", new Action<dynamic>(runGoto)},
                {"$assignment", new Action<string, object>(runAssignment)},
                {"$getVariable", new Func<string, object>(runGetVariable)},
                {"$and", new Func<object, object, bool>(runAnd)},
                {"$or", new Func<object, object, bool>(runOr)},
                {"$less", new Func<object, object, bool>(runLess)},
                {"$lessOrEq", new Func<object, object, bool>(runLessOrEq)},
                {"$greater", new Func<object, object, bool>(runGreater)},
                {"$greaterOrEq", new Func<object, object, bool>(runGreaterOrEq)},
                {"$equals", new Func<object, object, bool>(runEquals)},
                {"$notEquals", new Func<object, object, bool>(runNotEquals)},
                {"$add", new Func<object, object, dynamic>(runAdd)},
                {"$min", new Func<object, object, dynamic>(runMin)},
                {"$mul", new Func<object, object, dynamic>(runMul)},
                {"$div", new Func<object, object, dynamic>(runDiv)},
                {"$raise", new Func<object, object, dynamic>(runRaise)},
                {"$uniPlus", new Func<object, dynamic>(runUniPlus)},
                {"$uniMin", new Func<object, dynamic>(runUniMin)}
            };
        }
    }
}
