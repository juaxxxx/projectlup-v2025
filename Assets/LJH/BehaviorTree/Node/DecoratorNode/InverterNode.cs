using UnityEngine;

namespace RL
{
    public class InverterNode : DecoratorNode
    {
        public InverterNode(Node targetNode) : base(targetNode) { }
        public override NodeState Evaluate()
        {
            NodeState targetState = targetNode.Evaluate();

            switch (targetState)
            {
                case NodeState.Running:
                    return NodeState.Running;

                case NodeState.Fail:
                    return NodeState.Success;

                case NodeState.Success:
                    return NodeState.Fail;

                default:
                    CallWrontState();
                    return NodeState.Fail;
            }


        }
    }
}

