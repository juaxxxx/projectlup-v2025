using UnityEngine;
using System.Collections;
namespace LUP.RL
{
    public class FireSystem : MonoBehaviour
    {
        public Transform spawnPoint;
        public BulletData bulletData;
        private Archer player;
        public float fireDelay = 1f;
        private float LastfiremTime;

        private void Awake()
        {
            Archer player = GetComponent<Archer>();
            fireDelay = player.RuntimeData.currentData.AttackSpeed;
        }
        public void TryFire(Transform target, int attackValue)
        {
            if(target == null)
            {
                return;
            }
            Debug.Log("АјАн");
            var dir = (target.position - spawnPoint.position).normalized;
            var rot = Quaternion.LookRotation(dir);
            //Instantiate(bulletData.bulletPrefab, spawnPoint.position, rot);
            GameObject obj = Instantiate(bulletData.bulletPrefab, spawnPoint.position, rot);
            ProjectileBase tilebase = obj.GetComponent<ProjectileBase>();
            tilebase.Init(bulletData, gameObject, attackValue, target, bulletData.effectprefab);
        }

    }

}
