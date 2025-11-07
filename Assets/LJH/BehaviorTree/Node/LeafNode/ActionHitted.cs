using UnityEngine;

namespace RL
{
    public class ActionHitted : LeafNode
    {
        bool isAnimOnPlayed = false;
        public ActionHitted(BlackBoard blackBoard, BaseBehaviorTree behaviorTree) : base(blackBoard, behaviorTree)
        {

        }
        public override NodeState Evaluate()
        {
            UnityEngine.Debug.Log("Action Hitted");

            if (isAnimOnPlayed)
            {
                nodeState = NodeState.Running;
                return nodeState;
            }

            isAnimOnPlayed = true;
            nodeState = NodeState.Running;

            behaviorTree.PlayAnimation("Hitted", this);
            blackBoard.InHittedState = true;

            return nodeState;
        }

        public override void OnAnimationEnd()
        {
            UnityEngine.Debug.Log("Hit Animation Ended");
            isAnimOnPlayed = false;
            nodeState = NodeState.Success;
            blackBoard.InHittedState = false;
            blackBoard.OnHitted = false;
        }
    }
}

