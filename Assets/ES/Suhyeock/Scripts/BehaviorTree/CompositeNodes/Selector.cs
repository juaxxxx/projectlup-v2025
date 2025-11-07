using System.Collections.Generic;

namespace ES
{
    public class Selector : BTNode
    {
        private List<BTNode> children = new List<BTNode>();
        private BTNode lastRunningNode = null;

        public Selector(List<BTNode> children)
        {
            this.children = children;
        }

        public override NodeState Evaluate()
        {
            for (int i = 0; i < children.Count; i++)
            {
                BTNode child = children[i];
                switch (child.Evaluate())
                {
                    case NodeState.Failure:
                        child.Reset();
                        continue; // 다음 노드로 이동
                    case NodeState.Running:
                        if (lastRunningNode != null && lastRunningNode != child)
                        {
                            lastRunningNode.Reset();
                        }
                        lastRunningNode = child;
                        return NodeState.Running; // 실행 중이면 즉시 반환
                    case NodeState.Success:
                        if(lastRunningNode != null)
                        {
                            lastRunningNode.Reset();
                            lastRunningNode = null;
                        }
                        return NodeState.Success; // 성공하면 전체 성공
                }
            }

            if (lastRunningNode != null)
            {
                lastRunningNode.Reset();
                lastRunningNode = null;
            }

            // 모든 노드가 실패하면 전체 실패
            return NodeState.Failure;
        }

        public override void Reset()
        {
            if (lastRunningNode != null)
            {
                lastRunningNode.Reset();
                lastRunningNode = null;
            }
            for (int i = 0;i < children.Count;i++)
            {
                children[i].Reset();
            }
        }
    }
}