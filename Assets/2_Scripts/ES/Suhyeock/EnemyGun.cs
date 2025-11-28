using UnityEngine;

namespace LUP.ES
{
    public class EnemyGun : MonoBehaviour
    {
        public ItemDataBase itemDataBase; //임시
        public int selectedWeaponId = 1; //임시
        public WeaponItem weapon;
        public GameObject bulletPrefab;
        private BulletObjectPool bulletPool;
        public Transform firePoint;
        private float nextFireTime = 0f;

        private void Awake()
        {
            bulletPool = GetComponent<BulletObjectPool>();
        }


        private void Start()
        {
            BaseItemData itemData = itemDataBase.GetItemByID(selectedWeaponId);
            WeaponItemData weaponData = itemData as WeaponItemData;
            weapon = new WeaponItem(weaponData);
            bulletPool.Init(bulletPrefab);
        }

        public bool Fire()
        {

            if (Time.time < nextFireTime)
            {
                return false;
            }

            nextFireTime = Time.time + weapon.data.timeBetAttack;
            GameObject obj = bulletPool.Get();
            Bullet bullet = obj.GetComponent<Bullet>();
            if (bullet != null)
            {
                RangedWeaponItemData data = weapon.data as RangedWeaponItemData;
                bullet.Init(bulletPool, firePoint.position, firePoint.rotation, weapon.data.range, weapon.data.damage, data.bulletSpeed);
                return true;
            }
            return true;
        }

        public void Destroy()
        {
            bulletPool.PoolDestroy();
        }
    }
}


