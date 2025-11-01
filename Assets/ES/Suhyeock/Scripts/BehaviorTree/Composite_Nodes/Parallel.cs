using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;


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

        //foreach (BTNode node in children)
        //{
        //    NodeState state = node.Evaluate();

        //    if (state == NodeState.Running)
        //    {
        //        anyRunning = true;
        //    }
        //    // Failure나 Success는 무시하고 계속 다음 노드를 실행
        //}

        // 자식 중 하나라도 실행 중이면 Running
        // 그렇지 않으면 Success
        return anyRunning ? NodeState.Running : NodeState.Success;
    }
}
