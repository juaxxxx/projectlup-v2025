using System.Collections.Generic;

namespace ES
{
    public class Parallel : BTNode
    {
        private List<BTNode> children = new List<BTNode>();

        public Parallel(List<BTNode> children)
        {
            this.children = children;
        }

        public override NodeState Evaluate()
        {
            bool anyRunning = false;
            for (int i = 0; i < children.Count; i++)
            {
                NodeState state = children[i].Evaluate();

                if (state == NodeState.Running)
                {
                    anyRunning = true;
                }
            }

            // 자식 중 하나라도 실행 중이면 Running
            // 그렇지 않으면 Success
            return anyRunning ? NodeState.Running : NodeState.Success;
        }

        public override void Reset()
        {
            for (int i = 0; i < children.Count; i++)
            {
                children[i].Reset();
            }
        }
    }
}
