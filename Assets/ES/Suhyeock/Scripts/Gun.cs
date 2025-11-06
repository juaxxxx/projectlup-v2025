using System.Collections;
using UnityEngine;

namespace ES
{
    public enum GunState
    {
        READY,
        ATTACKING,
        RELOADING,
    }

    public class Gun : MonoBehaviour
    {
        public ItemDataBase itemDataBase; //임시
        public int selectedWeaponId = 1; //임시

        public EventBroker eventBroker;
        //public GunData gunData;
        public Weapon weapon;
        private BulletObjectPool bulletPool;
        public GameObject bulletPrefab;
        public Transform firePoint;
        [HideInInspector]
        public GunState state;

        [HideInInspector]
        public int ammoRemain = 0;
        [HideInInspector]
        public int magAmmo = 0;
        private float nextFireTime = 0f;

        private void Awake()
        {
            bulletPool = GetComponent<BulletObjectPool>();
            //ammoRemain = gunData.startAmmoRemain;
        }
        private void Start()
        {
            state = GunState.READY;
            BaseItemData itemData = itemDataBase.GetItemByID(selectedWeaponId);
            WeaponItemData weaponData = itemData as WeaponItemData;
            weapon = new Weapon(weaponData);
            magAmmo = weapon.magCapacity;
            bulletPool.Init(bulletPrefab);
        }
        public void Fire()
        {
            if (Time.time < nextFireTime)
            {
                return;
            }

            nextFireTime = Time.time + weapon.timeBetFire;
            GameObject obj = bulletPool.Get();
            Bullet bullet = obj.GetComponent<Bullet>();
            if (bullet != null)
            {
                bullet.Init(bulletPool, firePoint.position, firePoint.rotation, weapon.range, weapon.damage, weapon.bulletSpeed);
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
            state = GunState.RELOADING;
            float timer = 0f;
            //yield return new WaitForSeconds(gunData.reloadTime);
            while (timer < weapon.reloadTime)
            {
                timer += Time.deltaTime;
                eventBroker.ReloadTimeUpdate(timer, weapon.reloadTime);
                yield return null;
            }
            state = GunState.READY;
            magAmmo = weapon.magCapacity;
        }
    }

}
