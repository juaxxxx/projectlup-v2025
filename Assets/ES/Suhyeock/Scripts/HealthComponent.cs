using UnityEngine;

namespace ES
{
    public class HealthComponent : MonoBehaviour
    {
        public float HP = 0.0f;
        public float MaxHP = 100.0f;
        [HideInInspector]
        public bool isHit = false;
        [HideInInspector]
        public bool isDead = false;

        public void Start()
        {
            HP = MaxHP;
        }

        public void TakeDamage(float damage)
        {
            Debug.Log("TakeDamage");
            if (isDead)
            {
                return;
            }
            HP -= damage;
            isHit = true;
            if (HP < 0.0f)
            {
                isDead = true;
                HP = 0.0f;
            }
        }


    }
}


