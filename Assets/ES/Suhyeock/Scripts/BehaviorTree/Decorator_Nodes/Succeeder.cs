using UnityEngine;

namespace ES
{
    public class Succeeder : BTNode
    {
        public BTNode node;
        public Succeeder(BTNode node)
        {
            this.node = node;
        }
        public override NodeState Evaluate()
        {
            node.Evaluate();
            return NodeState.Success;
        }

        public override void Reset()
        {
            node.Reset();
        }
    }
}
