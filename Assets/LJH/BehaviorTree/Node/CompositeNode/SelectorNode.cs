using System.Collections.Generic;
using UnityEngine;

namespace RL
{
    public class SelectorNode : CompositeNode
    {
        public SelectorNode(List<Node> childList) : base(childList) { }

        public override NodeState Evaluate()
        {
            if (childListNodes.Count == 0)
            {
                UnityEngine.Debug.LogError("Child List Empty");
                return NodeState.Fail;
            }

            for (int i = currWorkingNodeIndex; i < childListNodes.Count; i++)
            {
                NodeState childNodeState = childListNodes[i].Evaluate();

                switch (childNodeState)
                {
                    case NodeState.Success:
                        currWorkingNodeIndex = 0;
                        return NodeState.Success;

                    case NodeState.Running:
                        currWorkingNodeIndex = i;
                        return NodeState.Running;

                    case NodeState.Fail:
                        continue;
                    default:
                        CallWrontState();
                        currWorkingNodeIndex = 0;
                        return NodeState.Fail;
                }

            }

            currWorkingNodeIndex = 0;
            return NodeState.Fail;
        }
    }
}

