using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

namespace ES
{
    [CreateAssetMenu(fileName = "ItemDataBase", menuName = "Scriptable Objects/ItemDataBase")]
    public class ItemDataBase : ScriptableObject
    {
        [SerializeReference]
        private List<BaseItemData> items = new List<BaseItemData> {
                new WeaponItemData(1, "Pistol", "Pistol", 10f, 5f, 10f, 8, 0.3f, 1.2f),
                new WeaponItemData(2, "SMG", "SMG", 25f, 15f, 7f, 20, 0.1f, 1.3f),
                new WeaponItemData(3, "AR", "AR", 20f, 20f, 10f, 30, 0.15f, 2.0f),
                new WeaponItemData(4, "LMG", "LMG", 25f, 17f, 20f, 100, 0.12f, 4.0f),
                new ArmorItemData(5, "Helmet", "Helmet",5, ArmorSlot.Head ),
                new ArmorItemData(6, "BodyArmor", "BodyArmor",5, ArmorSlot.Body ),
                new ConsumableItemData(7, "Bandage", "Bandage", 5.0f, 5.0f, EffectType.Heal, 30.0f, 3),
                new MaterialItemData(8, "Key", "Key", MaterialTier.Common, 5)
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
