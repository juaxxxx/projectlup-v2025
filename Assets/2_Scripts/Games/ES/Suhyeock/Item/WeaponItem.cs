using UnityEngine;

namespace LUP.ES
{
    public class WeaponItem : Item
    {
        public WeaponItemData data;
        //public float bulletSpeed;
        //public float damage;
        //public float range;
        //public int magCapacity; // 탄창 용량
        //public float timeBetAttack; // 공격 간격
        //public float reloadTime; // 재장전 소요 시간
        //public WeaponType weaponType;

        public WeaponItem(WeaponItemData itemData) : base(itemData)
        {
            data = itemData;
            //bulletSpeed = itemData.bulletSpeed;
            //damage = itemData.damage;
            //range = itemData.range;
            //magCapacity = itemData.magCapacity;
            //timeBetAttack = itemData.timeBetAttack;
            //reloadTime = itemData.reloadTime;
            count = 1;
        }
    }
}
