using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace Compiler.Node
{
    public class ConditionalJumpNode : BaseNode
    {
        private string _functionName;
        private object _parameters;
        public ConditionalJumpNode(string functionName, object parameters)
        {
            _functionName = functionName;
            _parameters = parameters;
        }

        public void accept(NodeVisitor visitor)
        {
            visitor.visit(this);
        }
    }
}
