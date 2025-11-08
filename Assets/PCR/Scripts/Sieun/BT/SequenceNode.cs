using System.Collections.Generic;

namespace LUP.PCR
{
    public sealed class SequenceNode : BTNode
    {
        List<BTNode> children;
        public SequenceNode(List<BTNode> BTNodes) { children = BTNodes; }

        public override WorkerNodeState Evaluate()
        {
            foreach (BTNode child in children)
            {
                WorkerNodeState result = child.Evaluate();
                switch (result)
                {
                    case BTNode.WorkerNodeState.RUNNING:
                        return BTNode.WorkerNodeState.RUNNING;
                    case BTNode.WorkerNodeState.SUCCESS:
                        continue;
                    case BTNode.WorkerNodeState.FAILURE:
                        return BTNode.WorkerNodeState.FAILURE;
                }
            }
            return WorkerNodeState.SUCCESS;
        }
    }

}
