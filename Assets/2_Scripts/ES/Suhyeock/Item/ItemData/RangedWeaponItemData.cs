using System;
using UnityEngine;

namespace LUP.ES
{

    [Serializable]
    public class RangedWeaponItemData : WeaponItemData
    {
        public float bulletSpeed;
        public int magCapacity; // 탄창 용량
        public float reloadTime; // 재장전 소요 시간

        public RangedWeaponItemData(int id, string name, string iconName, float damage, float range, float timeBetAttack, float bulletSpeed, int magCapacity, float reloadTime) : base(id, name, iconName, damage, range, timeBetAttack)
        {
            weaponType = WeaponType.Ranged;
            this.bulletSpeed = bulletSpeed;
            this.magCapacity = magCapacity;
            this.reloadTime = reloadTime;
        }
    }

}

