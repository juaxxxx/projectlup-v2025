using UnityEngine;
namespace LUP.RL
{
    public class JoyStickSC : MonoBehaviour
    {
        public static JoyStickSC Instance;
        public float speed = 5;
        public FixedJoystick fixedJoystick;

        private PlayerMove playerMove;
        void Awake()
        {
            Instance = this;
        }

        public void SetPlayer(PlayerMove move)
        {
            playerMove = move;
        }
        public void Update()
        {
            if (playerMove == null) return;
            float h = fixedJoystick.Horizontal;
            float v = fixedJoystick.Vertical;
            playerMove.MoveByJoystick(h, v);
        }

    }

}