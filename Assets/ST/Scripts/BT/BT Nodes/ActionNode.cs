using System;
namespace ST
{
    public class ActionNode : BaseNode
    {
        private Func<NodeState> action;

        public ActionNode(Func<NodeState> action)
        {
            this.action = action;
        }

        public override NodeState Evaluate()
        {
            return action();
        }
        public override void Reset()
        {
            base.Reset();
        }
    }

}
