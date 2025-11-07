using UnityEngine;

namespace LUP.RL
{
    public class PlayerMoveNode : Node
    {
        private readonly PlayerBlackBoard bb;
        private readonly JoyStickSC joystick;

        public PlayerMoveNode(PlayerBlackBoard blackboard, JoyStickSC js)
        {
            bb = blackboard;
            joystick = js;
        }

        public override NodeState Evaluate()
        {
            float h = joystick.fixedJoystick.Horizontal;
            float v = joystick.fixedJoystick.Vertical;
            if (Mathf.Abs(h) < 0.05f && Mathf.Abs(v) < 0.05f)
            {
                bb.Move.isMoving = false;
                return NodeState.Fail;  
            }
            else
            {
                bb.Move.MoveByJoystick(h, v);
                bb.Move.isMoving = true;
                return NodeState.Running;  
            }
            // 실제 이동 수행
            //bb.Move.MoveByJoystick(h, v);
            //return NodeState.Running;
        }
    }
}