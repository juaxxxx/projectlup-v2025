using UnityEngine;
namespace ST
{

    public static class CombatUtility
    {

        public static void ShootBullet(
            StatComponent stats,
            GameObject bulletPrefab,
            Transform firePoint,
            Vector3 direction,
            string targetTag)
        {
            if (bulletPrefab == null || firePoint == null || stats == null)
            {
                return;
            }

            GameObject bullet = Object.Instantiate(
                bulletPrefab,
                firePoint.position,
                Quaternion.LookRotation(direction)
            );

            Bullet bulletScript = bullet.GetComponent<Bullet>();
            if (bulletScript != null)
            {
                bulletScript.damage = stats.CalculateDamage();
                bulletScript.targetTag = targetTag;
            }
            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = direction.normalized * stats.BulletSpeed;
            }
            float lifetime = stats.AttackRange / stats.BulletSpeed;
            Object.Destroy(bullet, lifetime);
        }
    }
}