using UnityEngine;

namespace LUP.RL
{
    public class ReduceHP : LeafNode
    {
        public ReduceHP(BlackBoard blackBoar, BaseBehaviorTree behaviorTree) : base(blackBoar, behaviorTree)
        {

        }
        public override NodeState Evaluate()
        {
            UnityEngine.Debug.Log("Reduce HP");
            return NodeState.Success;
        }

        public override void OnAnimationEnd(AnimatorStateInfo animInfo)
        {

        }
    }
}

