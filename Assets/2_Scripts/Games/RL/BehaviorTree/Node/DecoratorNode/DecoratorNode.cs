using UnityEngine;

namespace LUP.RL
{
    public abstract class DecoratorNode : Node
    {
        public Node targetNode;

        public DecoratorNode(Node decoratedNode)
        {
            targetNode = decoratedNode;
        }
    }
}

