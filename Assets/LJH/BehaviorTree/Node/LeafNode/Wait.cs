using UnityEngine;

namespace RL
{
    public class Wait : LeafNode
    {
        public Wait(BlackBoard blackBoard, BaseBehaviorTree behaviorTree) : base(blackBoard, behaviorTree)
        {

        }
        public override NodeState Evaluate()
        {
            UnityEngine.Debug.Log("Action Wait");
            return NodeState.Success;
        }
        public override void OnAnimationEnd()
        {

        }
    }
}

