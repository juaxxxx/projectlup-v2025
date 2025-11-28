using System;
using UnityEngine;


namespace LUP.ES
{

    [Serializable]
    public class MeleeWeaponItemData : WeaponItemData
    {
        public float attackAngle;
        public MeleeWeaponItemData(int id, string name, string iconName, float damage, float range, float timeBetAttack, float attackAngle) : base(id, name, iconName, damage, range, timeBetAttack)
        {
            weaponType = WeaponType.Melee;
            this.attackAngle = attackAngle;
        }
    }
}