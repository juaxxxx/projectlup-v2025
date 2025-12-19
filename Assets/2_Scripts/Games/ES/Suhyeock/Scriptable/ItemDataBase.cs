using NUnit.Framework;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace LUP.ES
{
    [CreateAssetMenu(fileName = "ItemDataBase", menuName = "ES/ItemDataBase")]
    public class ItemDataBase : ScriptableObject
    {
        [SerializeReference]
        public List<BaseItemData> items = new List<BaseItemData> {
                new RangedWeaponItemData(0, "Pistol", "Pistol",  5f, 5f, 0.3f, 10f, 8,  1.2f),
                new RangedWeaponItemData(1, "AR", "AR",  20f, 10f, 0.15f, 20f, 30, 2.0f),
                new RangedWeaponItemData(2, "Rifle", "Rfile",  45f, 20f, 0.7f, 25f, 10, 4.0f),
                new ArmorItemData(3, "Helmet", "Helmet",5, ArmorSlot.Head ),
                new ArmorItemData(4, "BodyArmor", "BodyArmor",5, ArmorSlot.Body ),
                new ConsumableItemData(5, "Bandage", "Bandage", 5.0f, 5.0f, EffectType.Heal, 30.0f, 3),
                new MaterialItemData(6, "Key", "Key", MaterialTier.Common, 5),
                new MeleeWeaponItemData(7, "Sword", "Sword", 30.0f, 2f, 0.8f, 160f),
                new ThrowingWeaponData(8, "Rock", "Sword", 30.0f, 6f, 0.9f, 1.0f, 4.0f),
        };

        private Dictionary<int, BaseItemData> itemDictionary;

        public void Initiallize()
        {
            itemDictionary = new Dictionary<int, BaseItemData>();
            for (int i = 0; i < items.Count; i++)
            {
                if (!itemDictionary.ContainsKey(items[i].id))
                {
                    itemDictionary.Add(items[i].id, items[i]);
                }
            }
        }

        public BaseItemData GetItemByID(int id)
        {
            if (itemDictionary == null || itemDictionary.Count == 0)
                return null;

            if (itemDictionary.TryGetValue(id, out BaseItemData item))
            {
                return item;
            }

            return null;
        }

        public int GetItemCount() 
        { 
            return items.Count; 
        }

    }
}
