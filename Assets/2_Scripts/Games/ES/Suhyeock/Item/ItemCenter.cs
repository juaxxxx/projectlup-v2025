using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

namespace LUP.ES
{
    public class ItemCenter : MonoBehaviour
    {
        public ItemDataBase itemDataBase;

        public int minItemsToDrop = 1;
        public int maxItemsToDrop = 4;

        private List<BaseItemData> droppableItems = new List<BaseItemData>();
        private float totalWeight = 0f;

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
            totalWeight = 0f;

            // itemDataBase의 모든 아이템을 순회
            // (items 리스트가 public이라고 가정. private이면 Getter 필요)
            foreach (var item in itemDataBase.items)
            {
                if (item.dropChance > 0) // 확률이 0인 아이템(운영자용 등)은 제외
                {
                    droppableItems.Add(item);
                    totalWeight += item.dropChance; // 전체 가중치 합산
                }
            }
        }

        private BaseItemData GetRandomItemWeighted()
        {
            if (droppableItems.Count == 0) return null;

            // 0 ~ 전체 가중치 사이의 랜덤 값 선택
            float randomPoint = Random.Range(0, totalWeight);

            // 리스트를 돌면서 랜덤 값을 깎아내림
            foreach (BaseItemData item in droppableItems)
            {
                if (item.dropChance > 0)
                {
                    if (randomPoint <= item.dropChance)
                    {
                        return item;
                    }
                    randomPoint -= item.dropChance;
                }
            }

            // 부동소수점 오차로 인해 루프가 끝났다면 마지막 아이템 반환
            return droppableItems[droppableItems.Count - 1];
        }

        public List<Item> GenerateLoot()
        {
            List<Item> generatedLoot = new List<Item>();
            int itemsToGenerate = Random.Range(minItemsToDrop, maxItemsToDrop + 1);

            for (int i = 0; i < itemsToGenerate; i++)
            {
                //int randomId = Random.Range(1, itemDataBase.GetItemCount() + 1);
                //BaseItemData itemData = itemDataBase.GetItemByID(randomId);
                BaseItemData itemData = GetRandomItemWeighted();
                switch (itemData.itemType)
                {
                    case ItemType.Weapon:
                        {
                            WeaponItemData weaponData = itemData as WeaponItemData;
                            WeaponItem weapon = new WeaponItem(weaponData);
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
}
