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
        private Transform aimPivot;

        [HideInInspector]
        public int ammoRemain = 0;
        [HideInInspector]
        public int magAmmo = 0;
        private float nextAttackTime = 0f;

        private FollowCamera cameraScript;
        public void Init(int id)
        {
            aimPivot = transform.root;
            bulletPool = GetComponent<BulletObjectPool>();
            selectedWeaponId = id;
            state = WeaponState.READY;
            BaseItemData itemData = itemDataBase.GetItemByID(selectedWeaponId);
            RangedWeaponItemData weaponData = itemData as RangedWeaponItemData;
            weaponItem = new WeaponItem(weaponData);
            if (weaponData != null)
            {
                magAmmo = weaponData.magCapacity;
                bulletPool.Init(bulletPrefab);
            }

            GameObject camObj = GameObject.FindGameObjectWithTag("MainCamera");
            if (camObj != null)
            {
                cameraScript = camObj.GetComponent<FollowCamera>();
            }
        }
        public override bool Attack()
        {
            if (Time.time < nextAttackTime)
            {
                return false;
            }

            nextAttackTime = Time.time + weaponItem.data.timeBetAttack;
            GameObject obj = bulletPool.Get();
            Bullet bullet = obj.GetComponent<Bullet>();
            RangedWeaponItemData data = weaponItem.data as RangedWeaponItemData;
            if (bullet != null)
            {
                Quaternion shootRotation = aimPivot.rotation;
                bullet.Init(bulletPool, firePoint.position, shootRotation, data.range, data.damage, data.bulletSpeed);
                magAmmo--;
                cameraScript.Shake(0.08f, 0.03f);
                return true;
            }
            return false;
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
