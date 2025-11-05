using System.Collections.Generic;
using UnityEngine;

namespace RL
{
    public class SequenceNode : CompositeNode
    {
        public SequenceNode(List<Node> childList) : base(childList) { }

        public override NodeState Evaluate()
        {
            if(childListNodes.Count == 0)
            {
                UnityEngine.Debug.LogError("Child List Empty");
                return NodeState.Fail;
            }

            for(int i = currWorkingNodeIndex; i < childListNodes.Count; i++)
            {
                NodeState childNodeState = childListNodes[i].Evaluate();

                if (childNodeState == NodeState.Fail)
                {
                    currWorkingNodeIndex = 0;
                    return NodeState.Fail;
                }
                    

                else if(childNodeState == NodeState.Running)
                {
                    currWorkingNodeIndex = i;
                    return NodeState.Running;
                }
                    
            }

            currWorkingNodeIndex = 0;
            return NodeState.Success;
        }
    }
}

