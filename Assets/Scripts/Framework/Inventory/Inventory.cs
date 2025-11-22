using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LUP
{
    public class Inventory
    {
        private Dictionary<string, InventorySlot> slots;

        public event Action<IItemable, int> OnItemAdded;
        public event Action<IItemable, int> OnItemRemoved;
        public event Action<IItemable> OnItemUsed;

        public Inventory()
        {
            slots = new Dictionary<string, InventorySlot>();
        }

        public bool AddItem(IItemable item, int quantity = 1)
        {
            if (item == null || quantity <= 0)
            {
                Debug.LogWarning("Invalid item or quantity");
                return false;
            }

            if (slots.TryGetValue(item.ItemID.ToString(), out var existingSlot))
            {
                if (existingSlot.CanStack)
                {
                    if (existingSlot.TryAddQuantity(quantity))
                    {
                        OnItemAdded?.Invoke(item, quantity);
                        return true;
                    }

                    int remaining = quantity - (item.MaxStackSize - existingSlot.Quantity);
                    existingSlot.TryAddQuantity(item.MaxStackSize - existingSlot.Quantity);

                    return AddItem(item, remaining);
                }
            }

            string slotKey = GenerateSlotKey(item.ItemID);
            slots[slotKey] = new InventorySlot(item, quantity);

            OnItemAdded?.Invoke(item, quantity);
            return true;
        }


        public bool UseItem(int itemID, int quantity = 1)
        {
            if (!slots.TryGetValue(itemID.ToString(), out var slot))
            {
                Debug.LogWarning($"Item {itemID} not found");
                return false;
            }

            if (!slot.Item.IsUsable)
            {
                Debug.LogWarning($"Item {slot.Item.ItemName} is not usable");
                return false;
            }

            if (slot.Quantity < quantity)
            {
                Debug.LogWarning($"Not enough {slot.Item.ItemName}. Required: {quantity}, Have: {slot.Quantity}");
                return false;
            }

            for (int i = 0; i < quantity; i++)
            {
                slot.Item.OnUse();
                OnItemUsed?.Invoke(slot.Item);
            }

            RemoveItem(itemID, quantity);

            return true;
        }

        public bool RemoveItem(int itemID, int quantity = 1)
        {
            string key = itemID.ToString();
            if (!slots.TryGetValue(key, out var slot))
                return false;

            if (slot.Quantity < quantity)
                return false;

            slot.TryRemoveQuantity(quantity);

            if (slot.Quantity <= 0)
            {
                slots.Remove(key);
            }

            OnItemRemoved?.Invoke(slot.Item, quantity);
            return true;
        }

        public int GetItemCount(int itemID)
        {
            int total = 0;

            foreach (var slot in slots.Values)
            {
                if (slot.Item.ItemID == itemID)
                {
                    total += slot.Quantity;
                }
            }

            return total;
        }

        public bool HasItem(int itemID, int requiredQuantity = 1)
        {
            return GetItemCount(itemID) >= requiredQuantity;
        }

        public List<InventorySlot> GetAllItems()
        {
            return slots.Values.ToList();
        }

        public void Clear()
        {
            slots.Clear();
        }

        private string GenerateSlotKey(int itemID)
        {
            int counter = 0;
            string key = itemID.ToString();

            while (slots.ContainsKey(key))
            {
                counter++;
                key = $"{itemID}_{counter}";
            }

            return key;
        }

        public void SaveInventory(string filename)
        {
            InventoryData data = new InventoryData();

            foreach (var kvp in slots)
            {
                data.slots.Add(new TestInvenRuntimeData
                {
                    slotKey = kvp.Key,
                    itemID = kvp.Value.Item.ItemID,
                    quantity = kvp.Value.Quantity
                });
            }

            JsonDataHelper.SaveData(data, filename);
        }


        public class InventoryData
        {
            public List<TestInvenRuntimeData> slots = new List<TestInvenRuntimeData>();
        }

        public void LoadInventory(string filename)
        {
            if (!JsonDataHelper.FileExists(filename))
            {
                Debug.LogWarning($"인벤토리 파일이 없습니다: {filename}. 빈 인벤토리로 시작합니다.");
                slots.Clear();
                return;
            }

            InventoryData data = JsonDataHelper.LoadData<InventoryData>(filename);

            if (data == null)
            {
                Debug.LogError($"인벤토리 데이터 로드 실패: {filename}");
                return;
            }

            slots.Clear();

            // slots가 null이거나 비어있을 경우 대비
            if (data.slots == null || data.slots.Count == 0)
            {
                Debug.LogWarning("인벤토리에 저장된 아이템이 없습니다.");
                return;
            }

            // ItemManager 초기화 체크
            if (ItemManager.Instance == null)
            {
                Debug.LogError("ItemManager가 초기화되지 않았습니다!");
                return;
            }

            int loadedCount = 0;

            foreach (var slotData in data.slots)
            {
                if (slotData == null || slotData.itemID == 0)
                {
                    Debug.LogWarning("잘못된 슬롯 데이터를 건너뜁니다.");
                    continue;
                }

                IItemable item = ItemManager.Instance.GetItem(slotData.itemID);

                if (item != null)
                {
                    slots[slotData.slotKey] = new InventorySlot(item, slotData.quantity);
                    Debug.Log($"아이템 로드 성공: {item.ItemName} x{slotData.quantity}");
                    loadedCount++;
                }
                else
                {
                    Debug.LogWarning($"아이템을 찾을 수 없습니다: {slotData.itemID}");
                }
            }

            Debug.Log($"인벤토리 로드 완료: 총 {loadedCount}/{data.slots.Count}개 슬롯");
        }
    }
}


