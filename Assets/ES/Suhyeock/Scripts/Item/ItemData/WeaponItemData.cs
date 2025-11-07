using System;
using UnityEngine;

namespace ES
{
    [Serializable]
    public class WeaponItemData : BaseItemData
    {
        public float bulletSpeed;
        public float damage;
        public float range;

        public int magCapacity; // 탄창 용량

        public float timeBetFire; // 총알 발사 간격
        public float reloadTime; // 재장전 소요 시간

        public WeaponItemData(int id, string name, string iconName, float bulletSpeed, float damage, float range, int magCapacity, float timeBetFire, float reloadTime)
        {
            this.id = id;
            this.name = name;
            type = ItemType.Weapon;
            this.iconName = iconName;

            this.bulletSpeed = bulletSpeed;  
            this.damage = damage;
            this.range = range; 
            this.magCapacity = magCapacity;
            this.timeBetFire = timeBetFire;
            this.reloadTime = reloadTime;
            stackSize = 1;
        }
    }
}
