using System.Collections.Generic;
using UnityEngine;

namespace LUP.PCR
{
    public sealed class SelectorNode : BTNode
    {
        List<BTNode> children;
        public SelectorNode(List<BTNode> nodes) { children = nodes; }

        public override BTNode.NodeState Evaluate()
        {
            foreach (BTNode child in children)
            {
                NodeState result = child.Evaluate();
                switch (result)
                {
                    case BTNode.NodeState.RUNNING:
                        return BTNode.NodeState.RUNNING;
                    case BTNode.NodeState.SUCCESS:
                        return BTNode.NodeState.SUCCESS;
                    case NodeState.FAILURE:
                        continue;

                }
            }
            return NodeState.FAILURE;
        }
    }

}