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

        public MaterialItemData(int id, string name, string iconName, MaterialTier tier, int stackSize) : base(id, name, iconName, stackSize)
        {
            itemType = ItemType.Material;
        }
    }
}
