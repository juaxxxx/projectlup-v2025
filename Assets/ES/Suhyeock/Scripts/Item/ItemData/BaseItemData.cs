using System;
using UnityEngine;

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
    public string name;
    public ItemType type;
    public string iconName;
    public int stackSize;
}
