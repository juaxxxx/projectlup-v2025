using UnityEngine;

public class CastingInteractionAction : BTNode
{
    PlayerBlackboard blackboard;

    public CastingInteractionAction(PlayerBlackboard blackboard)
    {
        this.blackboard = blackboard;
    }

    public override NodeState Evaluate()
    {
        bool isCompleted = blackboard.interactingObject.TryStartInteraction(Time.deltaTime);
        if (isCompleted)
        {
            blackboard.ResetInteractionState();
            return NodeState.Success;
        }
        return NodeState.Running;
    }
}
