using System;
using UnityEngine;

namespace LUP.ES
{
    public enum WeaponType
    {
        Melee,
        Ranged,
        Throwing,
    }

    [Serializable]
    public class WeaponItemData : BaseItemData
    {
        public WeaponType weaponType;
        public float damage;
        public float range;
        public float timeBetAttack; // 공격 간격


        public WeaponItemData(int id, string name, string description, string iconName, float dropChacne, float damage, float range, float timeBetAttack) : base(id, name, description, iconName, 1, dropChacne)
        {
            ID = id;
            Name = name;
            itemType = ItemType.Weapon;
            IconName = iconName;

            this.damage = damage;
            this.range = range;     
            this.timeBetAttack = timeBetAttack;
        }
    }
}
