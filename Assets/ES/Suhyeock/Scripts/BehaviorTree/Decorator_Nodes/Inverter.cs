using UnityEngine;

namespace ES
{
    public class Inverter : BTNode
    {
        public BTNode node;

        public Inverter(BTNode node)
        {
            this.node = node;
        }
        public override NodeState Evaluate()
        {
            if (node.Evaluate() == NodeState.Success)
            {
                return NodeState.Failure;
            }
            return NodeState.Success;
        }

        public override void Reset()
        {
            node.Reset();
        }
    }
}
