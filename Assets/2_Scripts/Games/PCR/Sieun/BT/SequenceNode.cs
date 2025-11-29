using System.Collections.Generic;

namespace LUP.PCR
{
    public sealed class SequenceNode : BTNode
    {
        List<BTNode> children;
        public SequenceNode(List<BTNode> BTNodes) { children = BTNodes; 
}
        public override NodeState Evaluate()
        {
            foreach (BTNode child in children)
            {
                NodeState result = child.Evaluate();
                switch (result)
                {
                    case BTNode.NodeState.RUNNING:
                        return BTNode.NodeState.RUNNING;
                    case BTNode.NodeState.FAILURE:
                        return BTNode.NodeState.FAILURE;
                    case BTNode.NodeState.SUCCESS:
                        continue;

                }
            }
            return NodeState.SUCCESS;
        }
    }

}
