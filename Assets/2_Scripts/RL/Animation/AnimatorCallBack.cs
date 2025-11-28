using UnityEngine;
using UnityEngine.Animations;

public class AnimatorCallBack : StateMachineBehaviour
{
    public float animTargetCallBackRate = 1.0f;
    public AnimationStateCallback animCallBack;
    public AnimationStateCallback animEndCallBack;
    public delegate void AnimationStateCallback(AnimatorStateInfo stateInfo);

    private bool alreadyanimCallBacked = false;

    public void SetAnimCallBBack(AnimationStateCallback callback)
    {
        animCallBack = callback;
    }

    public void SetAnimEndCallBack(AnimationStateCallback callback)
    {
        animEndCallBack = callback;
    }

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        alreadyanimCallBacked = false;
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animEndCallBack?.Invoke(stateInfo);
        alreadyanimCallBacked = false;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animTargetCallBackRate == 1.0f || alreadyanimCallBacked == true)
            return;

        float progress = stateInfo.normalizedTime % 1.0f;
        if(progress >= animTargetCallBackRate)
        {
            alreadyanimCallBacked = true;
            animCallBack?.Invoke(stateInfo);
        }
    }
}
