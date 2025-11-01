using UnityEngine;

public class Armor : Item
{
    public int defense;
    public ArmorSlot armorSlot;

    public Armor(ArmorItemData itemData) : base(itemData)
    {
        defense = itemData.defense;
        armorSlot = itemData.armorSlot;
        count = 1;
    }
}
