using UnityEngine;

namespace LUP.RL
{
    public class Wait : LeafNode
    {
        public Wait(BlackBoard blackBoard, BaseBehaviorTree behaviorTree) : base(blackBoard, behaviorTree)
        {

        }
        public override NodeState Evaluate()
        {
            //UnityEngine.Debug.Log("Action Wait");

            if(behaviorTree.GetCurrentAnimState().IsName("Idle") == false)
            {
                behaviorTree.PlayAnimation(ActionState.Idle, this);

                SetNavAgentDeActivate(true);
            }

            return NodeState.Success;
        }
        public override void OnAnimationEnd(AnimatorStateInfo animInfo)
        {

        }
    }
}

