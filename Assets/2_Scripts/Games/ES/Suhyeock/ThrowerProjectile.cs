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
        ownerPool.Return(gameObject);

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}

}

