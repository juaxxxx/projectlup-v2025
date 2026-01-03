using System;
using UnityEngine;

namespace LUP.ES
{
    public enum MaterialTier
    {
        Common,
        Rare,
        Epic
    }

    [Serializable]
    public class MaterialItemData : BaseItemData
    {
        public MaterialTier tier;

        public MaterialItemData(int id, string name, string description, string iconName, float dropChance, MaterialTier tier, int stackSize) : base(id, name, description, iconName, stackSize, dropChance)
        {
            itemType = ItemType.Material;
        }
    }
}
