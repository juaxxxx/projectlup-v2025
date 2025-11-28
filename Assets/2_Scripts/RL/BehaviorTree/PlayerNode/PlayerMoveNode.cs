using UnityEngine;

namespace LUP.RL
{
    public class PlayerMoveNode : PlayerLeafNode
    {
        private readonly PlayerBlackBoard bb;
        private readonly PlayerBehaviorTree bt;
        private readonly JoyStickSC joystick;

        public PlayerMoveNode(PlayerBlackBoard blackboard, JoyStickSC js, PlayerBehaviorTree behaviorTree)
        {
            bb = blackboard;
            bt = behaviorTree;
            joystick = js;
        }

        public override NodeState Evaluate()
        {
            float h = joystick.fixedJoystick.Horizontal;
            float v = joystick.fixedJoystick.Vertical;
            if (Mathf.Abs(h) < 0.05f && Mathf.Abs(v) < 0.05f)
            {
                if (bt.GetCurrentAnimState().IsName("Wait") == false && bt.GetCurrentAnimState().IsName("Attack") == false)
                {
                    bt.PlayAnimation(ActionState.Wait, this);
                }


                bb.Move.isMoving = false;
                return NodeState.Fail;
            }
            else
            {
                if (bt.GetCurrentAnimState().IsName("MoveTo") == false)
                {
                    bt.PlayAnimation(ActionState.MoveTo, this);
                }

                bb.Move.MoveByJoystick(h, v);
                bb.Move.isMoving = true;
                return NodeState.Running;
            }
            // 실제 이동 수행
            //bb.Move.MoveByJoystick(h, v);
            //return NodeState.Running;
        }

        public override void OnPlayerAnimationEnd(AnimatorStateInfo animInfo)
        {

        }
    }
}