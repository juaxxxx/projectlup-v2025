using System;
namespace LUP.ST
{

    public class Decorator : BaseNode
    {
        private Func<bool> condition;
        private BaseNode child;

        public Decorator(Func<bool> condition, BaseNode child)
        {
            this.condition = condition;
            this.child = child;
        }

        public override NodeState Evaluate()
        {
            if (condition())
            {
                return child.Evaluate();
            }
            return NodeState.FAILURE;
        }

        public override void Reset()
        {
            base.Reset();
            child.Reset();
        }
    }

}