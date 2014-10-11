using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Compiler.Node;

namespace Compiler
{
    public class NextStepVisitor : NodeVisitor
    {
        private bool nextStep = true;

        public void visit(JumpNode jumpNode)
        {
            nextStep = false;
        }

        public void visit(ConditionalJumpNode conditionalJumpNode)
        {
            nextStep = false;
        }

        public bool setNextStep()
        {
            return nextStep;
        }
    }
}
