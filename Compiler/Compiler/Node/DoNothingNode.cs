using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiler.Node
{
    public class DoNothingNode : BaseNode
    {
        private string _functionName;
        public DoNothingNode(string functionName)
        {
            _functionName = functionName;
        }

        public void accept(NodeVisitor visitor)
        {
            visitor.visit(this);
        }
    }
}
