using UnityEngine;

namespace ES
{
    public class Weapon : Item
    {
        public float bulletSpeed;
        public float damage;
        public float range;
        public int magCapacity; // 탄창 용량
        public float timeBetFire; // 총알 발사 간격
        public float reloadTime; // 재장전 소요 시간

        public Weapon(WeaponItemData itemData) : base(itemData)
        {
            bulletSpeed = itemData.bulletSpeed;
            damage = itemData.damage;
            range = itemData.range;
            magCapacity = itemData.magCapacity;
            timeBetFire = itemData.timeBetFire;
            reloadTime = itemData.reloadTime;
            count = 1;
        }
    }
}
