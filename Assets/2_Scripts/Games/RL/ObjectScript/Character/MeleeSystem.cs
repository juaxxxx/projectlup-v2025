using LUP.ES;
using UnityEngine;
using UnityEngine.Jobs;
namespace LUP.RL
{
    public class MeleeSystem : MonoBehaviour
    {
        private int Damage;
        private GameObject Owner;
        public Collider hitcolider;


        private void Update()
        {
         
        }
        public void Awake()
        {
            Owner = transform.root.gameObject;
            //hitcolider.enabled = false;

        }
        public void EnableHitbox()
        {
            hitcolider.enabled = true;
        }
        public void DisableHitbox()
        {
            if (Damage <= 0) return;
            hitcolider.enabled = false;
        }
   
        public void MeleeAttack(int damage)
        {
            Damage = damage;
            
        }
        public void ForceDisableHitbox()
        {

            hitcolider.enabled = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (hitcolider.enabled == false) return;
            if (other.gameObject == Owner) return;

            Enemy enemy = other.GetComponentInParent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(Damage);
                Debug.Log($"Enemy 피격 : {enemy.name}, 데미지 : {Damage}");
                return;
            }

            // Player
            Archer archer = other.GetComponentInParent<Archer>();
            if (archer != null)
            {
                archer.TakeDamage(Damage);
                Debug.Log($"Player 피격 : {archer.name}, 데미지 : {Damage}");
                return;
            }

        }
    }
}
