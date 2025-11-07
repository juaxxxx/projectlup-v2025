using UnityEngine;

public class AnimatorCallBack : StateMachineBehaviour
{
    public AnimationStateCallback animEndCallBack;
    public delegate void AnimationStateCallback(AnimatorStateInfo stateInfo);

    public void SetAnimEndCallBack(AnimationStateCallback callback)
    {
        animEndCallBack = callback;
    }

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animEndCallBack?.Invoke(stateInfo);
    }
}
