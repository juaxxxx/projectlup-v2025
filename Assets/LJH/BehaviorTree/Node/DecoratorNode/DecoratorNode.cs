using UnityEngine;

namespace RL
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

