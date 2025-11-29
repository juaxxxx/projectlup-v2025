using System;
using UnityEngine;

namespace LUP.ES
{
    public enum ItemType
    {
        None,
        Weapon,
        Armor,
        Consumable,
        Material,
    }

    [Serializable]
    public class BaseItemData
    {
        public int id;
        public string iconName;
        public string name;
        public int stackSize;

        public ItemType itemType;
        public BaseItemData(int id, string name, string iconName, int stackSize)
        {
            this.id = id;
            this.name = name;
            this.iconName = iconName;
            this.stackSize = stackSize;
        }
    }
}
