using UnityEngine;

public class HitCondition : BTNode
{
    PlayerBlackboard blackboard;

    public HitCondition(PlayerBlackboard blackboard)
    {
        this.blackboard = blackboard;
    }

    public override NodeState Evaluate()
    {
        if (blackboard.isHit == true)
        {
            //blackboard.isHit = false;
            return NodeState.Success;
        }
        return NodeState.Failure;
    }

}
