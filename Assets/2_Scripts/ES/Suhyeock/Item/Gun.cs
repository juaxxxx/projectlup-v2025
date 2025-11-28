using System.Collections;
using UnityEngine;

namespace LUP.ES
{

    public class Gun : Weapon, IReloadable
    {
        public ItemDataBase itemDataBase; //임시
        public int selectedWeaponId = 1; //임시

        //public EventBroker eventBroker;
        //public GunData gunData;
        public GameObject bulletPrefab;
        private BulletObjectPool bulletPool;
        public Transform firePoint;

        [HideInInspector]
        public int ammoRemain = 0;
        [HideInInspector]
        public int magAmmo = 0;
        private float nextAttackTime = 0f;

        private void Awake()
        {
            bulletPool = GetComponent<BulletObjectPool>();
        }

        private void Start()
        {
            state = WeaponState.READY;
            BaseItemData itemData = itemDataBase.GetItemByID(selectedWeaponId);
            RangedWeaponItemData weaponData = itemData as RangedWeaponItemData;
            weaponItem = new WeaponItem(weaponData);
            if (weaponData != null)
            {
                magAmmo = weaponData.magCapacity;
                bulletPool.Init(bulletPrefab);
            }
        }
        public override void Attack()
        {
            if (Time.time < nextAttackTime)
            {
                return;
            }

            nextAttackTime = Time.time + weaponItem.data.timeBetAttack;
            GameObject obj = bulletPool.Get();
            Bullet bullet = obj.GetComponent<Bullet>();
            RangedWeaponItemData data = weaponItem.data as RangedWeaponItemData;
            if (bullet != null)
            {
                bullet.Init(bulletPool, firePoint.position, firePoint.rotation, data.range, data.damage, data.bulletSpeed);
                magAmmo--;
                return;
            }
            return;
        }

        public void Reload()
        {
            StartCoroutine(ReloadRoutine());
        }

        private IEnumerator ReloadRoutine()
        {
            state = WeaponState.RELOADING;
            float timer = 0f;
            //yield return new WaitForSeconds(gunData.reloadTime);
            RangedWeaponItemData data = weaponItem.data as RangedWeaponItemData;
            while (timer < data.reloadTime)
            {
                timer += Time.deltaTime;
                eventBroker.ReloadTimeUpdate(timer, data.reloadTime);
                yield return null;
            }
            state = WeaponState.READY;
            magAmmo = data.magCapacity;
        }
    }

}
