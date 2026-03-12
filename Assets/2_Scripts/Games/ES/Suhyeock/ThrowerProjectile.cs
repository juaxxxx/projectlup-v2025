using LUP.ES;
using System.Collections.Generic;
using UnityEngine;

namespace LUP.ES
{ 
    public class ThrowerProjectile : MonoBehaviour
    {
        private float damage;
        private float radius;
        public LayerMask targetLayer;
        private BulletObjectPool ownerPool;
        private VFXObjectPool vfxObjectPool;

        [SerializeField] private GameObject explosionPrefab; // 이펙트 프리팹 연결
        //[SerializeField] private float vfxDuration = 2.0f;   // 이펙트가 유지될 시간
        [SerializeField] private float scaleMultiplier = 1.5f; // 크기 보정값 (아래 설명 참고)

        private bool hasExploded = false;
        public void Init(BulletObjectPool objectPool, Vector3 position, Quaternion rotation, float damage, float radius)
        {
            ownerPool = objectPool;
            transform.position = position;
            transform.rotation = rotation;
            this.damage = damage;
            this.radius = radius;
            vfxObjectPool = FindFirstObjectByType<VFXObjectPool>();

            hasExploded = false;
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Player"))
                return;
            ApplyRadialDamage();
        }

        private void ApplyRadialDamage()
        {
            if (hasExploded) return;
            hasExploded = true;

            Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius, targetLayer);

            // 중복 타격을 방지하기 위해 이미 타격한 대상을 기록할 컬렉션 생성
            HashSet<HealthComponent> damagedTargets = new HashSet<HealthComponent>();

            foreach (Collider hit in hitColliders)
            {
                if (hit.TryGetComponent(out HealthComponent healthComponent))
                {
                    // 이 HealthComponent가 기록된 적이 없다면 (처음 맞는 거라면)
                    if (!damagedTargets.Contains(healthComponent))
                    {
                        healthComponent.TakeDamage(damage);
                        damagedTargets.Add(healthComponent); // 데미지를 주었다고 기록함
                        Debug.Log("Hit!!!!!!!!!!!!!!!!");
                    }
                }
            }

            SoundManager.Instance.PlaySFX("Explosion", gameObject);
            SpawnExplosionVFX();

            ownerPool.Return(gameObject);

        }

        private void SpawnExplosionVFX()
        {
            if (explosionPrefab != null)
            {
                GameObject instance = vfxObjectPool.SpawnVFX(explosionPrefab, transform.position);
                instance.transform.localScale = Vector3.one * scaleMultiplier;
                //GameObject vfxInstance = Instantiate(explosionPrefab, transform.position, Quaternion.identity);

                //vfxInstance.transform.localScale = Vector3.one * radius * scaleMultiplier;

                //Destroy(vfxInstance, vfxDuration);
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, radius);
        }
    }

}

