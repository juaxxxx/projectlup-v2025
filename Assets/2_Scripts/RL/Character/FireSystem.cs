using UnityEngine;
using System.Collections;
namespace LUP.RL
{
    public class FireSystem : MonoBehaviour
    {
        public Transform spawnPoint;
        public BulletData bulletData;
        public float fireDelay = 1f;
        private float LastfiremTime;

        public void TryFire(Transform target, int attackValue)
        {
            if(target == null)
            {
                Debug.Log(" get null");
                return;
            }
            GameObject obj = Instantiate(bulletData.bulletPrefab, spawnPoint.position, spawnPoint.rotation);
            ProjectileBase tilebase = obj.GetComponent<ProjectileBase>();
            tilebase.Init(bulletData, gameObject, attackValue, target);
        }

    }

}
