using UnityEngine;

namespace LUP.ES
{
    public class WeaponItem : Item
    {
        public WeaponItemData data;

        public WeaponItem(WeaponItemData itemData) : base(itemData)
        {
            data = itemData;
            count = 1;
        }
    }
}
