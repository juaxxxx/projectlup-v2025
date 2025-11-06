using UnityEngine;

namespace ES
{
    public class CastingInteractionAction : BTNode
    {
        PlayerBlackboard blackboard;

        public CastingInteractionAction(PlayerBlackboard blackboard)
        {
            this.blackboard = blackboard;
        }

        public override NodeState Evaluate()
        {
            if (blackboard.interactingObject == null ||
                blackboard.rightJoystick.Horizontal != 0 || blackboard.rightJoystick.Vertical != 0 ||
                blackboard.leftJoystick.Horizontal != 0 || blackboard.leftJoystick.Vertical != 0)
            {
                return NodeState.Failure;
            }
            bool isCompleted = blackboard.interactingObject.TryStartInteraction(Time.deltaTime);
            if (isCompleted)
            {
                blackboard.ResetInteractionState();
                return NodeState.Success;
            }
            return NodeState.Running;
        }

        public override void Reset()
        {
            if (blackboard.interactingObject != null)
            {
                blackboard.interactingObject.HideInteractionTimerUI();
                blackboard.interactingObject.ResetInteraction();
                blackboard.ResetInteractionState();
               
            }
        }
    }
}
