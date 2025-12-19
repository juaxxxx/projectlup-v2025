using UnityEngine;

namespace LUP.ES
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
            if (blackboard.interactingObject == null)
            {
                if (blackboard.animator != null)
                {
                    blackboard.animator.SetBool("IsInteracting", false);
                }
                return NodeState.Failure;
            }

            // 기수 추가한 코드
            if (blackboard.interactingObject is ExtractionPoint)
            {
                return NodeState.Failure;
            }
            // 기수 추가한 코드if(blackboard.interactingObject.InterruptsOnMove) 여기만
            if (blackboard.interactingObject.InterruptsOnMove)
            {
                if (blackboard.rightJoystick.Horizontal != 0 || blackboard.rightJoystick.Vertical != 0 ||
                    blackboard.leftJoystick.Horizontal != 0 || blackboard.leftJoystick.Vertical != 0)
                {
                    return NodeState.Failure;
                }
            }
         
            bool isCompleted = blackboard.interactingObject.TryStartInteraction(Time.deltaTime);
            if (isCompleted)
            {
                if (blackboard.animator != null)
                    blackboard.animator.SetBool("IsInteracting", false);
                blackboard.ResetInteractionState();
                blackboard.SetWeaponVisible(true);
                return NodeState.Success;
            }
            if (blackboard.animator != null)
                blackboard.animator.SetBool("IsInteracting", true);
            return NodeState.Running;
        }

        public override void Reset()
        {
            if (blackboard.interactingObject != null)
            {
                blackboard.interactingObject.HideInteractionTimerUI();
                blackboard.interactingObject.ResetInteraction();
                blackboard.ResetInteractionState();

                if (blackboard.animator != null)
                {
                    blackboard.animator.SetBool("IsInteracting", false);
                    blackboard.SetWeaponVisible(true);
                }
            }
        }
    }
}
