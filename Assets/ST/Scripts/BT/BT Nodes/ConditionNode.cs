using System;
namespace ST
{

    public class ConditionNode : BaseNode
    {
        private Func<bool> condition;

        public ConditionNode(Func<bool> condition)
        {
            this.condition = condition;
        }

        public override NodeState Evaluate()
        {
            return condition() ? NodeState.SUCCESS : NodeState.FAILURE;
        }
        public override void Reset()
        {
            base.Reset();
        }
    }
}