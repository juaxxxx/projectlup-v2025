using UnityEngine;

namespace RL
{
    public class PlayerMove : MonoBehaviour
    {

        [Header("이동 속도")]
        public float speed = 5f;
        public float baseSpeed = 5f;
        private void Start()
        {
            speed = baseSpeed;
        }
        public void AddSpeed(float amount)
        {
            speed += amount;
        }
        void Update()
        {
     
            // WASD 입력 받기
            float z = 0f;
            float x = 0f;

            if (Input.GetKey(KeyCode.W))
                z = 1f;
            else if (Input.GetKey(KeyCode.S))
                z = -1f;

            if (Input.GetKey(KeyCode.D))
                x = 1f;
            else if (Input.GetKey(KeyCode.A))
                x = -1f;

            // 이동 방향 계산 (3D 기준, Y는 0)
            Vector3 dir = new Vector3(z, 0, x).normalized;



            // 실제 이동
            transform.position += dir * speed * Time.deltaTime;
        }
        public void MoveByJoystick(float h, float v)
        {
      
            Vector3 dir = new Vector3(h, 0, v).normalized;
            transform.position += dir * speed * Time.deltaTime;

            if (dir != Vector3.zero)
            {
                transform.forward = dir;
            }
        }

    }
}