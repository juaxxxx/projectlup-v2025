using System;
using UnityEngine;

namespace ES
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

        public ArmorItemData(int id, string name, string iconName, int defense, ArmorSlot armorSlot)
        {
            this.id = id;
            this.name = name;
            type = ItemType.Armor;
            this.iconName = iconName;
            this.defense = defense;
            this.armorSlot = armorSlot;
            stackSize = 1;
        }
    }
}
