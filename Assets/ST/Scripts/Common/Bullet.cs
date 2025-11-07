using UnityEngine;
namespace ST
{

    public class Bullet : MonoBehaviour
    {
        public float damage = 10f;
        public string targetTag = "Enemy";

        void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag(targetTag))
                return;

            IDamageable damageable = other.GetComponent<IDamageable>();

            if (damageable != null)
            {
                damageable.TakeDamage(damage);
                // Debug.Log($"총알 명중! {other.name}에게 {damage} 데미지");  // 로그 선택
            }

            Destroy(gameObject);
        }
    }
}