using UnityEngine;

public class StateExitTrigger : StateMachineBehaviour
{
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        int state = animator.GetInteger("CharacterState");
        if(state == 7)
        {
            animator.SetInteger("CharacterState", 0);
        }
    }
}
