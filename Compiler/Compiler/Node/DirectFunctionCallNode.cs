using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiler.Node
{
    public class DirectFunctionCallNode : BaseNode
    {
        public DirectFunctionCallNode()
        {
            
        }
        public void accept(NodeVisitor visitor)
        {
            visitor.visit(this);
        }
    }
}
