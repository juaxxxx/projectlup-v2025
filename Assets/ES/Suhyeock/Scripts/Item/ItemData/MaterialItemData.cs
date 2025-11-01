using System;
using UnityEngine;


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

    public MaterialItemData(int id, string name, string iconName, MaterialTier tier, int stackSize)
    {
        base.id = id;
        this.name = name;
        type = ItemType.Material;
        this.iconName = iconName;
        this.tier = tier;
        this.stackSize = stackSize;
    }
}
