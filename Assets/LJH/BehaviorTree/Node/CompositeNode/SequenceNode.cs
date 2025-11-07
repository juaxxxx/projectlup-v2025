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
                NodeState compositNodeResult = childListNodes[i].Evaluate();


                switch (compositNodeResult)
                {
                    case NodeState.Fail:
                        currWorkingNodeIndex = 0;
                        return NodeState.Fail;

                    case NodeState.Running:
                        currWorkingNodeIndex = i;
                        return NodeState.Running;

                    case NodeState.Success:
                        continue;

                    default:
                        CallWrontState();
                        currWorkingNodeIndex = 0;
                        return NodeState.Fail;
                }

            }

            currWorkingNodeIndex = 0;
            return NodeState.Success;
        }
    }
}

