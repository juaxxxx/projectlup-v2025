using System;
using UnityEngine;

namespace LUP.ES
{
    [Serializable]
    public class ThrowingWeaponData : WeaponItemData
    {
        public float attackRadius;
        public float arcHeight;
        public float maxChargeTime;
        public float minRange = 0.5f;
        public ThrowingWeaponData(int id, string name, string iconName, float damage, float range, float timeBetAttack, float attackRadius, float arcHeight, float maxChargeTime) : base(id, name, iconName, damage, range, timeBetAttack)
        {
            weaponType = WeaponType.Throwing;
            this.attackRadius = attackRadius;
            this.arcHeight = arcHeight;
            this.maxChargeTime = maxChargeTime;
        }
    }

}
