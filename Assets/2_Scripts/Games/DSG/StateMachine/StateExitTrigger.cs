using LUP.DSG;
using UnityEngine;

public class StateExitTrigger : StateMachineBehaviour
{
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetInteger("CharacterState", 0);
    }
}
