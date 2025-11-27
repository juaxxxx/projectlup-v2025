using System;
using UnityEngine;

namespace LUP.ES
{
    public enum WeaponType
    {
        Melee,
        Ranged,
    }

    [Serializable]
    public class WeaponItemData : BaseItemData
    {
        public WeaponType weaponType;
        public float damage;
        public float range;
        public float timeBetAttack; // 공격 간격


        public WeaponItemData(int id, string name, string iconName, float damage, float range, float timeBetAttack) : base(id, name, iconName, 1)
        {
            this.id = id;
            this.name = name;
            itemType = ItemType.Weapon;
            this.iconName = iconName;

            this.damage = damage;
            this.range = range;     
            this.timeBetAttack = timeBetAttack;
        }
    }
}
