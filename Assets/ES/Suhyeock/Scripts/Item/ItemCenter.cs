using NUnit.Framework;
using System.Collections.Generic;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;
public class ItemCenter : MonoBehaviour
{
    public ItemDataBase itemDataBase;

    public int minItemsToDrop = 1;
    public int maxItemsToDrop = 4;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if (itemDataBase != null)
        {
            itemDataBase.Initiallize();
        }
    }

    public List<Item> GenerateLoot()
    {
        List<Item> generatedLoot = new List<Item>();
        int itemsToGenerate = Random.Range(minItemsToDrop, maxItemsToDrop + 1);

        for (int i = 0; i < itemsToGenerate; i++)
        {
            int randomId = Random.Range(1, itemDataBase.GetItemCount() + 1);
            BaseItemData itemData = itemDataBase.GetItemByID(randomId);
            switch (itemData.type)
            {
                case ItemType.Weapon:
                    {
                        WeaponItemData weaponData = itemData as WeaponItemData;
                        Weapon weapon = new Weapon(weaponData);
                        generatedLoot.Add(weapon);
                    }
                    break;
                case ItemType.Armor:
                    {
                        ArmorItemData armorData = itemData as ArmorItemData;
                        Armor armor = new Armor(armorData);
                        generatedLoot.Add(armor);
                    }
                    break;
                case ItemType.Consumable:
                    {
                        ConsumableItemData consumableData = itemData as ConsumableItemData;
                        Consumable consumable = new Consumable(consumableData);
                        generatedLoot.Add(consumable);
                    }
                    break;
                case ItemType.Material:
                    {
                        MaterialItemData materialData = itemData as MaterialItemData;
                        CraftingMaterial material = new CraftingMaterial(materialData);
                        generatedLoot.Add(material);
                    }
                    break;
                default:
                    break;
            }

            
        }

        return generatedLoot;
    }
}
