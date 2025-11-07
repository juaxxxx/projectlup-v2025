using UnityEngine;

namespace RL
{
    public class ActionMovTo : LeafNode
    {
        public ActionMovTo(BlackBoard blackBoard, BaseBehaviorTree behaviorTree) : base(blackBoard, behaviorTree)
        {

        }


        public override NodeState Evaluate()
        {
            UnityEngine.Debug.Log("Action MoveTo");
            return NodeState.Success;
        }

        public override void OnAnimationEnd()
        {

        }
    }
}

