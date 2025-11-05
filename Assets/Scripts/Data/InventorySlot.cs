using UnityEngine;

public class InventorySlot<T> where T : class, IInventoryItem
{
    [SerializeField] private T item;
    [SerializeField] private int count;

    public int SlotIndex { get; set; }

    public T Item
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

    // public bool IsFull => item != null && count >= item.MaxStackSize;
    // public int RemainingSpace => item != null ? item.MaxStackSize - count : 0;

    public InventorySlot(int slotIndex)
    {
        SlotIndex = slotIndex;
        item = null;
        count = 0;
    }

    /// <summary>
    /// 아이템 설정 (새로운 아이템 배치)
    /// </summary>
    public bool SetItem(T newItem, int amount)
    {
        if (newItem == null || amount <= 0)
            return false;

        item = newItem;
        // count = Mathf.Min(amount, newItem.MaxStackSize);
        return true;
    }

    /// <summary>
    /// 아이템 추가 (스택 가능한 경우)
    /// </summary>
    public int AddItem(T newItem, int amount)
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

    /// <summary>
    /// 아이템 제거
    /// </summary>
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

    /// <summary>
    /// 슬롯 비우기
    /// </summary>
    public void Clear()
    {
        item = null;
        count = 0;
    }

    /// <summary>
    /// 다른 슬롯과 교환
    /// </summary>
    public void SwapWith(InventorySlot<T> other)
    {
        if (other == null)
            return;

        T tempItem = item;
        int tempCount = count;

        item = other.item;
        count = other.count;

        other.item = tempItem;
        other.count = tempCount;
    }

    /// <summary>
    /// 저장 데이터로 변환
    /// </summary>
    public ItemRuntimeData ToSaveData()
    {
        return new ItemRuntimeData
        {
            //slotIndex = SlotIndex,
            //isEmpty = IsEmpty,
            //itemSaveData = IsEmpty ? null : item.ToSaveData(),
            //count = count
        };
    }
}
