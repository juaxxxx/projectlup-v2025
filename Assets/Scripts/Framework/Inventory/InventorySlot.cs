using UnityEngine;

namespace Framework
{
    public class InventorySlot
    {
        private IInventoryItemable item;
        private int count;

        public int SlotIndex { get; set; }

        public IInventoryItemable Item
        {
            get => item;
            private set => item = value;
        }

        public int Count
        {
            get => count;
            private set => count = Mathf.Max(0, value);
        }

        public bool IsEmpty => item == null || count <= 0;

        public bool IsFull => item != null && count >= item.MaxStackSize;
        public int RemainingSpace => item != null ? item.MaxStackSize - count : 0;

        public InventorySlot(int slotIndex)
        {
            SlotIndex = slotIndex;
            item = null;
            count = 0;
        }

        public bool SetItem(IInventoryItemable newItem, int amount)
        {
            if (newItem == null || amount <= 0)
                return false;

            item = newItem;
            count = Mathf.Min(amount, newItem.MaxStackSize);
            return true;
        }

        public int AddItem(IInventoryItemable newItem, int amount)
        {
            if (newItem == null || amount <= 0)
                return 0;

            // 빈 슬롯인 경우
            if (IsEmpty)
            {
                int addAmount = Mathf.Min(amount, newItem.MaxStackSize);
                SetItem(newItem, addAmount);
                return addAmount;
            }
             
            // 같은 아이템이고 스택 가능한 경우
            if (item.ItemID == newItem.ItemID && item.IsStackable)
            {
                int canAdd = item.MaxStackSize - count;
                int actualAdd = Mathf.Min(canAdd, amount);
                count += actualAdd;
                return actualAdd;
            }

            return 0;
        }

        public int RemoveItem(int amount)
        {
            if (IsEmpty || amount <= 0)
                return 0;

            int removed = Mathf.Min(count, amount);
            count -= removed;

            if (count <= 0)
            {
                Clear();
            }

            return removed;
        }

        public void Clear()
        {
            item = null;
            count = 0;
        }

        public void SwapWith(InventorySlot other)
        {
            if (other == null)
                return;

            IInventoryItemable tempItem = item;
            int tempCount = count;

            item = other.item;
            count = other.count;

            other.item = tempItem;
            other.count = tempCount;
        }

        public ItemRuntimeData ToSaveData()
        {
            if (IsEmpty)
            {
                return new ItemRuntimeData
                {
                    ItemId = -1,
                    Count = 0
                };
            }

            ItemRuntimeData data = item.ToSaveData();
            data.Count = count;
            return data;
        }
    }
}

