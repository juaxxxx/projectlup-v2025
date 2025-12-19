using UnityEngine;
namespace LUP.ST
{

    public class Bullet : MonoBehaviour
    {
        public float damage = 10f;
        public string targetTag = "Enemy";

        [Header("피격 이펙트")]
        public GameObject hitEffectPrefab;

        void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag(targetTag))
                return;

            IDamageable damageable = other.GetComponent<IDamageable>();

            if (damageable != null)
            {
                damageable.TakeDamage(damage);
                // Debug.Log($"총알 명중! {other.name}에게 {damage} 데미지"); 
                SpawnHitEffect(other);
            }
        }
        private void SpawnHitEffect(Collider other)
        {
            if (hitEffectPrefab != null)
            {
                Vector3 hitPoint = other.ClosestPoint(transform.position);

                GameObject effect = Instantiate(hitEffectPrefab, hitPoint, Quaternion.identity);
                Destroy(effect, 2f);
            }
        }
    }
}