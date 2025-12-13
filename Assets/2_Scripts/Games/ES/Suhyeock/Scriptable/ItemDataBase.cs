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
                new RangedWeaponItemData(1, "Pistol", "Pistol",  5f, 5f, 0.3f, 10f, 8,  1.2f),
                new RangedWeaponItemData(2, "SMG", "SMG",  15f, 7f, 0.1f, 25f, 20, 1.3f),
                new RangedWeaponItemData(3, "AR", "AR",  20f, 10f, 0.15f, 20f, 30, 2.0f),
                new RangedWeaponItemData(4, "Rifle", "Rfile",  45f, 20f, 0.7f, 25f, 10, 4.0f),
                new ArmorItemData(5, "Helmet", "Helmet",5, ArmorSlot.Head ),
                new ArmorItemData(6, "BodyArmor", "BodyArmor",5, ArmorSlot.Body ),
                new ConsumableItemData(7, "Bandage", "Bandage", 5.0f, 5.0f, EffectType.Heal, 30.0f, 3),
                new MaterialItemData(8, "Key", "Key", MaterialTier.Common, 5),
                new MeleeWeaponItemData(9, "Sword", "Sword", 30.0f, 2f, 0.8f, 160f),
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
