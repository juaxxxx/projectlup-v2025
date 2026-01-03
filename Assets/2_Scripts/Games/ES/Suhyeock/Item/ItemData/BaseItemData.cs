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
        public int ID;
        public string Name;
        public string IconName;
        public string Description;
        public int MaxStackSize;
        public float dropChance;

        public ItemType itemType;
        public BaseItemData(int id, string name, string description,string iconName, int stackSize, float dropChance)
        {
            ID = id;
            Name = name;
            IconName = iconName;
            Description = description;
            MaxStackSize = stackSize;
            this.dropChance = dropChance;
        }
    }
}
