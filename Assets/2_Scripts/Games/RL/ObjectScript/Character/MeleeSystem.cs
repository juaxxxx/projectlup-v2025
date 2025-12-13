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
            hitcolider.enabled = false;

        }
        public void EnableHitbox()
        {
            hitcolider.enabled = true;
            Debug.Log($"{hitcolider}Hitbox On");
        }
        public void DisableHitbox()
        {
            if (Damage <= 0) return;
            hitcolider.enabled = false;
            Debug.Log("Hitbox OFF");
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

            Enemy enemy = other.GetComponent<Enemy>();
            Archer archer = other.GetComponent<Archer>();
            if (hitcolider.enabled == false) return;
            if (other.gameObject == Owner) return;

            if (enemy == null) return;
            Debug.Log($"충돌한객체 : {other}");

            //enemy.TakeDamage(Damage); Debug.Log($"충돌한 객체 : {other} : 받은 데미지 : ${Damage}");
            
            

           
        }
    }
}
