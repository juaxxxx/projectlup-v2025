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

        public ArmorItemData(int id, string name, string iconName, int defense, ArmorSlot armorSlot) : base(id, name, iconName, 1) 
        {
            itemType = ItemType.Armor;
            this.defense = defense;
            this.armorSlot = armorSlot;
        }
    }
}
