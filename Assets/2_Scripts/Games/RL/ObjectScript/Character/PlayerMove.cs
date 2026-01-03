using UnityEngine;

namespace LUP.RL
{
    public class PlayerMove : MonoBehaviour
    {

        [Header("이동 속도")]
        private Archer character;        
        
        public float speed = 5f;
        public float baseSpeed = 5f;
        public Transform targetPoint;
        public  bool isMoving = false;
        private void Awake()
        {
            character = GetComponent<Archer>();
        }
        private void Start()
        {
           speed  = character.RuntimeData.currentData.speed;
        }
        public void AddSpeed(float amount)
        {
            speed += amount;
        }
  
        public void MoveByJoystick(float h, float v)
        {
      
            Vector3 dir = new Vector3(h, 0, v).normalized;
            transform.position += dir * speed * Time.deltaTime;

            if (dir != Vector3.zero)
            {
                isMoving = true;
                transform.forward = dir;
            }
            else
            {
                isMoving = false;
            }
        }

    }
}