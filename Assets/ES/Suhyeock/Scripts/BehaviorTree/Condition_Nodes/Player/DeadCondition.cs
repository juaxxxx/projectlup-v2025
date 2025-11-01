using UnityEngine;

public class DeadCondition : BTNode
{
    PlayerBlackboard blackboard;

    public DeadCondition(PlayerBlackboard blackboard)
    {
        this.blackboard = blackboard;
    }

    public override NodeState Evaluate()
    {
        if(blackboard.HP <= 0)
        {
            return NodeState.Success;
        } 
        return NodeState.Failure;
    }
}
