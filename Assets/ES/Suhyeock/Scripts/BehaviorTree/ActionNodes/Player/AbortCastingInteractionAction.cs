using UnityEngine;

namespace ES
{
    public class AbortCastingInteractionAction : BTNode
    {
        PlayerBlackboard blackboard;
        public AbortCastingInteractionAction(PlayerBlackboard blackboard)
        {
            this.blackboard = blackboard;
        }

        public override NodeState Evaluate()
        {
            blackboard.interactingObject.HideInteractionTimerUI();
            blackboard.interactingObject.ResetInteraction();
            blackboard.ResetInteractionState();
            return NodeState.Success;
        }

        public override void Reset()
        {

        }
    }
}
