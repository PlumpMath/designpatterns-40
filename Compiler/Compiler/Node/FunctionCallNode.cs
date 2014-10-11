using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiler.Node
{
    public class FunctionCallNode : BaseNode
    {
        public FunctionCallNode()
        {
            
        }

        public void accept(NodeVisitor visitor)
        {
            visitor.visit(this);
        }
    }
}
