using System.Collections.Generic;

public class Sequence : BTNode
{
    private List<BTNode> children = new List<BTNode>();

    public Sequence(List<BTNode> children)
    {
        this.children = children;
    }

    public override NodeState Evaluate()
    {
        bool isAnyNodeRunning = false;
        for (int i = 0; i < children.Count; i++) 
        {
            switch (children[i].Evaluate())
            {
                case NodeState.Failure:
                    return NodeState.Failure; // 하나라도 실패하면 전체 실패
                case NodeState.Running:
                    isAnyNodeRunning = true;
                    break;
                case NodeState.Success:
                    continue; // 다음 노드로 이동
            }
        }

        //foreach(BTNode node in children)
        //{
        //    switch (node.Evaluate())
        //    {
        //        case NodeState.Failure:
        //            return NodeState.Failure; // 하나라도 실패하면 전체 실패
        //        case NodeState.Running:
        //            isAnyNodeRunning = true;
        //            break;
        //        case NodeState.Success:
        //            continue; // 다음 노드로 이동
        //    }
        //}

        return isAnyNodeRunning ? NodeState.Running : NodeState.Success;
    }
}    

