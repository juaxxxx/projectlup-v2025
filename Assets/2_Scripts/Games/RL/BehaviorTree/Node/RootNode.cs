using UnityEngine;

namespace LUP.RL
{
    public class RootNode : Node
    {
        Node topChildNode;
        public RootNode(Node singleChildNode)
        {
            topChildNode = singleChildNode;
        }
        public override NodeState Evaluate()
        {
            return topChildNode.Evaluate();
        }
    }
}

