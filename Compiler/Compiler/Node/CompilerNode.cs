using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiler.Node
{
    public class CompilerNode : BaseNode
    {
        private string _functionName;
        private object _parameters;
        private object _extra;

        public CompilerNode(string functionName, object parameters, object extra)
        {
            _functionName = functionName;
            _parameters = parameters;
            _extra = extra;
        }

        public void accept(NodeVisitor visitor) 
        {
            visitor.visit(this);
        }
    }
}
