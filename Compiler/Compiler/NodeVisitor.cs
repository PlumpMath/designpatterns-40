using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Compiler.Node;

namespace Compiler
{
    public class NodeVisitor
    {
        public void visit(DoNothingNode node) { }
        public void visit(JumpNode jump) { }
        public void visit(ConditionalJumpNode conditionalJump) { }
        public void visit(DirectFunctionCallNode directFunctionCallNode) { }
        public void visit(FunctionCallNode functionCallNode) { }
        public void visit(ShowNode showNode) { }
        public void visit(CompilerNode compilerNode) { }
        public void visit(BaseNode baseNode) { }
    }
}
