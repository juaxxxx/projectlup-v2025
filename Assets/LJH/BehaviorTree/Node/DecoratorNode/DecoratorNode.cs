using UnityEngine;

namespace RL
{
    public abstract class DecoratorNode : Node
    {
        protected Node targetNode;

        public DecoratorNode(Node decoratedNode)
        {
            targetNode = decoratedNode;
        }
    }
}

