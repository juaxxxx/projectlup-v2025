using UnityEngine;

public class Failer : BTNode
{
    public BTNode node;
    public Failer(BTNode node)
    {
        this.node = node;
    }
    public override NodeState Evaluate()
    {
        node.Evaluate();
        return NodeState.Failure;
    }
}
