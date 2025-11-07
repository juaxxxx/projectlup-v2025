using System.Collections.Generic;
using UnityEngine;

namespace PCR
{
    public sealed class SelectorNode : BTNode
    {
        List<BTNode> children;
        public SelectorNode(List<BTNode> nodes) { children = nodes; }

        public override BTNode.WorkerNodeState Evaluate()
        {
            foreach (BTNode child in children)
            {
                WorkerNodeState result = child.Evaluate();
                switch (result)
                {
                    case BTNode.WorkerNodeState.RUNNING:
                        return BTNode.WorkerNodeState.RUNNING;
                    case BTNode.WorkerNodeState.SUCCESS:
                        return BTNode.WorkerNodeState.SUCCESS;

                }
            }
            return WorkerNodeState.FAILURE;
        }
    }

}