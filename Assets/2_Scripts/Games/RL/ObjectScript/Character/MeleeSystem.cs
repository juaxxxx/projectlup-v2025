using LUP.ES;
using UnityEngine;
using UnityEngine.Jobs;
namespace LUP.RL
{
    public class MeleeSystem : MonoBehaviour
    {
        private int Damage;
        private GameObject Owner;

        public GameObject Effectprefab;
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
                enemy.TakeDamage(Damage, Effectprefab);
                return;
            }

            // Player
            Archer archer = other.GetComponentInParent<Archer>();
            if (archer != null)
            {
                archer.TakeDamage(Damage);
                return;
            }

        }
    }
}
