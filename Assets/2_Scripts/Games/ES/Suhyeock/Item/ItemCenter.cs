using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

namespace LUP.ES
{
    public class ItemCenter : MonoBehaviour
    {
        public ItemDataBase itemDataBase;

        private List<BaseItemData> droppableItems = new List<BaseItemData>();

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Awake()
        {
            if (itemDataBase != null)
            {
                itemDataBase.Initiallize();
                CacheDroppableItems();
            }
        }

        private void CacheDroppableItems()
        {
            droppableItems.Clear();

            foreach (BaseItemData item in itemDataBase.items)
            {
                if (item.dropChance > 0) // 확률이 0인 아이템(운영자용 등)은 제외
                {
                    droppableItems.Add(item);
                }
            }
        }

        public List<Item> GenerateLoot()
        {
            List<Item> generatedLoot = new List<Item>();
            //int itemsToGenerate = Random.Range(minItemsToDrop, maxItemsToDrop + 1);

            foreach (BaseItemData item in droppableItems)
            {
                if (IsDropSuccessful(item.dropChance))
                {
                    Item newItem = CreateItemFromData(item);
                    if (newItem != null)
                    {
                        generatedLoot.Add(newItem);
                    }
                }
            }
            return generatedLoot;
        }

        private bool IsDropSuccessful(float chancePercentage)
        {
            // 0.0 ~ 100.0 사이의 랜덤 값 생성
            float randomValue = Random.Range(0f, 100f);
            return randomValue <= chancePercentage;
        }

        private Item CreateItemFromData(BaseItemData itemData)
        {
            switch (itemData.itemType)
            {
                case ItemType.Weapon:
                    return new WeaponItem(itemData as WeaponItemData);

                case ItemType.Armor:
                    return new Armor(itemData as ArmorItemData);

                case ItemType.Consumable:
                    return new Consumable(itemData as ConsumableItemData);

                case ItemType.Material:
                    return new CraftingMaterial(itemData as MaterialItemData);

                default:
                    return null;
            }
        }
    }
}
