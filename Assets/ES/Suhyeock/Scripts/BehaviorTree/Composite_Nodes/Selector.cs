using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;

public class Selector : BTNode
{
    private List<BTNode> children = new List<BTNode>();

    public Selector(List<BTNode> children)
    {
        this.children = children;
    }

    public override NodeState Evaluate()
    {
        for (int i = 0; i < children.Count; i++)
        {
            switch (children[i].Evaluate())
            {
                case NodeState.Failure:
                    continue; // 다음 노드로 이동
                case NodeState.Running:
                    return NodeState.Running; // 실행 중이면 즉시 반환
                case NodeState.Success:
                    return NodeState.Success; // 성공하면 전체 성공
            }
        }

        //foreach (BTNode node in children)
        //{
        //    switch (node.Evaluate())
        //    {
        //        case NodeState.Failure:
        //            continue; // 다음 노드로 이동
        //        case NodeState.Running:
        //            return NodeState.Running; // 실행 중이면 즉시 반환
        //        case NodeState.Success:
        //            return NodeState.Success; // 성공하면 전체 성공
        //    }
        //}

        // 모든 노드가 실패하면 전체 실패
        return NodeState.Failure;
    }
}