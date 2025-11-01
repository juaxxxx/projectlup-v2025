using UnityEngine;

public class CastingInteractionCondition : BTNode
{
    private PlayerBlackboard blackboard;
    public CastingInteractionCondition(PlayerBlackboard blackboard)
    {
        this.blackboard = blackboard;
    }
    public override NodeState Evaluate()
    {
        if (blackboard.interactingObject != null)
        {
            return NodeState.Success;
        }
        else
        {
            return NodeState.Failure;
        }
    }
}
