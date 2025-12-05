using UnityEngine;

namespace LUP.RL
{
    public class ActionHitted : LeafNode
    {
        bool isAnimOnPlayed = false;
        public ActionHitted(BlackBoard blackBoard, BaseBehaviorTree behaviorTree) : base(blackBoard, behaviorTree)
        {

        }
        public override NodeState Evaluate()
        {

            //UnityEngine.Debug.Log("Action Hitted");

            if(behaviorTree.GetCurrentAnimState().IsName("Attack"))
            {
                blackBoard.InHittedState = false;
                blackBoard.OnHitted = false;

                nodeState = NodeState.Fail;
                return nodeState;
            }

            if (isAnimOnPlayed)
            {
                nodeState = NodeState.Running;
                return nodeState;
            }

            isAnimOnPlayed = true;
            nodeState = NodeState.Running;


            behaviorTree.PlayAnimation(ActionState.Hitted, this);
            blackBoard.InHittedState = true;

            SetNavAgentDeActivate(true);

            return nodeState;
        }

        public override void OnAnimationEnd(AnimatorStateInfo animInfo)
        {
            //UnityEngine.Debug.Log("Hit Animation Ended");
            isAnimOnPlayed = false;
            nodeState = NodeState.Success;
            blackBoard.InHittedState = false;
            blackBoard.OnHitted = false;
            blackBoard.HittedAccumTime += animInfo.length / animInfo.speed;

            if (blackBoard.HittedAccumTime >= blackBoard.RampageTime)
                blackBoard.OnRampage = true;
        }
    }
}

