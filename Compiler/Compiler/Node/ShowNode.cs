using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiler.Node
{
    public class ShowNode : BaseNode
    {
        public ShowNode(string functionName, object parameters)
        {
            
        }

        public void accept(NodeVisitor visitor)
        {
            visitor.visit(this);
        }
    }
}
