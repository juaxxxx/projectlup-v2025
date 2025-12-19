using System.Collections;
using System.Linq;
using UnityEngine;

namespace LUP.ES
{
    public class TryInteractAction : BTNode
    {
        private PlayerBlackboard blackboard;
        private CharacterController characterController;
        private float rotationSpeed = 720f;
        public TryInteractAction(PlayerBlackboard blackboard, CharacterController characterController)
        {
            this.blackboard = blackboard;
            this.characterController = characterController;
        }


        public override NodeState Evaluate()
        {
            blackboard.isInteractionButtonPressed = false;

            if(blackboard.weapon.state == WeaponState.RELOADING)
                return NodeState.Failure;

            IInteractable target = blackboard.InteractionDetector.GetNearestInteractable();
            if (target == null)
            {
                return NodeState.Failure;
            }

            target.HideInteractionPrompt();
            target.ShowInteractionTimerUI();
            blackboard.interactingObject = target;
            blackboard.StartCoroutine(LookAtTargert(target));
            blackboard.moveState = MoveState.INTERACTING;
            blackboard.SetWeaponVisible(false);
            return NodeState.Success;
        }

        IEnumerator LookAtTargert(IInteractable target)
        {
            Vector3 direction = target.transform.position - blackboard.transform.position;
            direction.y = 0;
            if (direction == Vector3.zero)
                yield break;
            Quaternion targetRotation = Quaternion.LookRotation(direction);

            while (Quaternion.Angle(blackboard.transform.rotation, targetRotation) > 0.1f)
            {
                blackboard.transform.rotation = Quaternion.RotateTowards(blackboard.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
                yield return null;
            }

            blackboard.transform.rotation = targetRotation;
        }

        public override void Reset()
        {

        }
    }
}
