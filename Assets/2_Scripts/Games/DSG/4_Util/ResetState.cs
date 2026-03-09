using UnityEngine;

public class ResetState : StateMachineBehaviour
{
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetInteger("CharacterState", 0);
    }
}
