using System.Linq;
using UnityEngine;

public class TryInteractAction : BTNode
{
    private PlayerBlackboard blackboard;
    private CharacterController characterController;
    
    public TryInteractAction(PlayerBlackboard blackboard, CharacterController characterController)
    {
        this.blackboard = blackboard;
        this.characterController = characterController;
    }


    public override NodeState Evaluate()
    {
        blackboard.isInteractionButtonPressed = false;

        //Collider[] hits = Physics.OverlapSphere(
        //    characterController.transform.position,
        //    blackboard.InteractionRadius,
        //    blackboard.InteractableLayer
        //);

        ////hits중에 가장 가까운 target 구하기
        //IInteractable target = null;
        //float minDistance = float.MaxValue;

        //for (int i = 0; i < hits.Length; i++)
        //{
        //    IInteractable interactable = hits[i].GetComponent<IInteractable>();
        //    if (interactable != null && interactable.CanInteract())
        //    {
        //        float distance = Vector3.Distance(characterController.transform.position, hits[i].transform.position);
        //        if (distance < minDistance)
        //        {
        //            minDistance = distance;
        //            target = interactable;
        //        }
        //    }
        //}


        IInteractable target = blackboard.InteractionDetector.GetNearestInteractable();
        if (target == null)
        {
            return NodeState.Failure;
        }

        target.HideInteractionPrompt();
        target.ShowInteractionTimerUI();
        blackboard.interactingObject = target;
        return NodeState.Success;
    }

}
