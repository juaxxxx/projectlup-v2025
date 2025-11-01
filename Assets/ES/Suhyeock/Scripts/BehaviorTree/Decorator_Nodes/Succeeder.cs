using UnityEngine;

public class Succeeder : BTNode
{
    public BTNode node;
    public Succeeder(BTNode node)
    {
        this.node = node;
    }
    public override NodeState Evaluate()
    {
        node.Evaluate();
        return NodeState.Success;
    }
}
