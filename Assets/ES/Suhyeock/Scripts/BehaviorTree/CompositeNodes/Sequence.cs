using System.Collections.Generic;

namespace ES
{
    public class Sequence : BTNode
    {
        private List<BTNode> children = new List<BTNode>();
        private int currentIndex = 0;

        public Sequence(List<BTNode> children)
        {
            this.children = children;
        }

        public override NodeState Evaluate()
        {
            for (int i = currentIndex; i < children.Count; i++) 
            {
                switch (children[i].Evaluate())
                {
                    case NodeState.Running:
                        currentIndex = i;
                        return NodeState.Running;
                    case NodeState.Failure:
                        currentIndex = 0;
                        return NodeState.Failure; // 하나라도 실패하면 전체 실패
                    case NodeState.Success:
                        continue; // 다음 노드로 이동
                }
            }

            currentIndex = 0;
            return NodeState.Success;
        }

        public override void Reset()
        {
            currentIndex = 0;
            for (int i = 0; i < children.Count; i++)
            {
                children[i].Reset();
            }
        }
    }    
}

