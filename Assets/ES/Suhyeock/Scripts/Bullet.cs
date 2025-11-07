using UnityEngine;

namespace ES
{
    public class Bullet : MonoBehaviour
    {
        private float maxDistance;
        private Vector3 spawnPostion;
        private float speed = 0f;
        private float damage = 0f;
        private BulletObjectPool ownerPool;


        void Update()
        {
            transform.position += transform.forward * speed * Time.deltaTime;
            if (Vector3.Distance(spawnPostion, transform.position) > maxDistance)
            {
                ownerPool.Return(gameObject);
            }
        }

        public void Init(BulletObjectPool ownerPool, Vector3 position, Quaternion rotation, float maxDistance, float damage, float speed)
        {
            this.ownerPool = ownerPool;
            transform.position = position;
            transform.rotation =  rotation;
            spawnPostion = position;
            this.maxDistance = maxDistance;
            this.damage = damage;
            this.speed = speed;

        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Enemy"))
            {
                Debug.Log("OnTriggerEnter");
                if (other.TryGetComponent(out HealthComponent healthComponent))
                {
                    healthComponent.TakeDamage(damage);
                    ownerPool.Return(gameObject);
                }
            }
           
        }
    }
}
