using UnityEngine;

namespace RL
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
            // 입력값 받기
            float h = joystick.fixedJoystick.Horizontal;
            float v = joystick.fixedJoystick.Vertical;

            // 입력이 없으면 이동하지 않음
            if (h < 0f && v < 0f)
                return NodeState.Fail;
            Debug.Log("움직임");
            // 실제 이동 수행
            bb.Move.MoveByJoystick(h, v);
            return NodeState.Running;
        }
    }
}