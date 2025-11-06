using UnityEngine;
namespace RL
{
    public class JoyStickSC : MonoBehaviour
    {
        public float speed = 5;
        public FixedJoystick fixedJoystick;

        public PlayerMove playermove;
        public void Update()
        {

            float h = fixedJoystick.Horizontal;
            float v = fixedJoystick.Vertical;
            playermove.MoveByJoystick(h, v);
        }

    }

}