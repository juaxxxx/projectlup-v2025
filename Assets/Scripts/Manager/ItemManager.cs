using System.Collections.Generic;
using UnityEngine;

namespace LUP
{
    public class ItemManager : Singleton<ItemManager>
    {
        private Dictionary<int, LUPItemData> itemDatabase = new Dictionary<int, LUPItemData>();

        public override void Awake()
        {
            base.Awake();

            LoadAllItems();
        }

        private void LoadAllItems()
        {
            itemDatabase.Clear();

            LUPItemData[] items = Resources.LoadAll<LUPItemData>("Items");

            foreach (var item in items)
            {
                if (item != null && item.ItemID != 0)
                {
                    if (itemDatabase.ContainsKey(item.ItemID))
                    {
                        Debug.LogWarning($"중복된 ItemID 발견: {item.ItemID}. 나중에 로드된 아이템으로 덮어씁니다.");
                    }

                    itemDatabase[item.ItemID] = item;
                }
            }

            Debug.Log($"ItemManager: {itemDatabase.Count}개의 아이템 로드 완료");
        }

        public IItemable GetItem(int itemID)
        {
            if (itemID == 0)
            {
                Debug.LogWarning("ItemID가 유효하지 않습니다.");
                return null;
            }

            if (itemDatabase.TryGetValue(itemID, out LUPItemData item))
            {
                return item;
            }

            Debug.LogWarning($"아이템을 찾을 수 없습니다: {itemID}");
            return null;
        }

        public IEnumerable<LUPItemData> GetAllItems()
        {
            return itemDatabase.Values;
        }

        public void ReloadItems()
        {
            LoadAllItems();
        }

        public List<LUPItemData> GetItemsByType(LUP.Define.ItemType type)
        {
            List<LUPItemData> result = new List<LUPItemData>();

            foreach (var item in itemDatabase.Values)
            {
                if (item.Type == type)
                {
                    result.Add(item);
                }
            }

            return result;
        }

        public bool HasItem(int itemID)
        {
            return itemDatabase.ContainsKey(itemID);
        }

    }
}
