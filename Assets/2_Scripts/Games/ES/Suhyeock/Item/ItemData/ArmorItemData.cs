using System;
using UnityEngine;

namespace LUP.ES
{
    public enum ArmorSlot
    {
        Head,
        Body,
        Gloves,
        Shoes,
    }
    [Serializable]
    public class ArmorItemData : BaseItemData
    {
        public int defense;
        public ArmorSlot armorSlot;

        public ArmorItemData(int id, string name, string description, string iconName, float dropChance, int defense, ArmorSlot armorSlot) : base(id, name, description, iconName, 1, dropChance) 
        {
            itemType = ItemType.Armor;
            this.defense = defense;
            this.armorSlot = armorSlot;
        }
    }
}
