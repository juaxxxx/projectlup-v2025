using LUP.ES;
using UnityEngine;

namespace LUP.ES
{ 
    public class ThrowerProjectile : MonoBehaviour
    {
        private float damage;
        private float radius;
        public LayerMask targetLayer;
        private BulletObjectPool ownerPool;

        [SerializeField] private GameObject explosionPrefab; // 이펙트 프리팹 연결
        [SerializeField] private float vfxDuration = 2.0f;   // 이펙트가 유지될 시간
        [SerializeField] private float scaleMultiplier = 1.5f; // 크기 보정값 (아래 설명 참고)

        public void Init(BulletObjectPool objectPool, Vector3 position, Quaternion rotation, float damage, float radius)
        {
            ownerPool = objectPool;
            transform.position = position;
            transform.rotation = rotation;
            this.damage = damage;
            this.radius = radius;
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Player"))
                return;
            ApplyRadialDamage();
        }

        private void ApplyRadialDamage()
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius, targetLayer);

            foreach (Collider hit in hitColliders)
            {
                if (hit.TryGetComponent(out HealthComponent healthComponent))
                {
                    healthComponent.TakeDamage(damage);
                }
            }
            SpawnExplosionVFX();

            ownerPool.Return(gameObject);

        }

        private void SpawnExplosionVFX()
        {
            if (explosionPrefab != null)
            {
                GameObject vfxInstance = Instantiate(explosionPrefab, transform.position, Quaternion.identity);

                vfxInstance.transform.localScale = Vector3.one * radius * scaleMultiplier;

                Destroy(vfxInstance, vfxDuration);
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, radius);
        }
    }

}

