using UnityEngine;

namespace LUP
{
    public interface IItemable
    {
        int ItemID { get; }                      // 고유 ID (필수)
        string ItemName { get; }                    // 표시용 이름
        LUP.Define.ItemType Type { get; }           // 아이템 타입
        int MaxStackSize { get; }                   // 최대 스택 (1 = 스택 불가)
        Sprite Icon { get; }                        // UI 아이콘

        bool IsUsable { get; }

        // 아이템 사용시 호출됨 (구체적 동작은 각 아이템이 구현)
        void OnUse();
    }

    public class InventorySlot
    {
        public IItemable Item { get; private set; }
        public int Quantity { get; private set; }

        public bool IsEmpty => Item == null;
        public bool IsFull => Quantity >= Item.MaxStackSize;
        public bool CanStack => !IsFull;

        public InventorySlot(IItemable item, int quantity)
        {
            Item = item;
            Quantity = quantity;
        }

        public bool TryAddQuantity(int amount)
        {
            if (Quantity + amount > Item.MaxStackSize)
                return false;

            Quantity += amount;
            return true;
        }

        public bool TryRemoveQuantity(int amount)
        {
            if (Quantity < amount)
                return false;

            Quantity -= amount;
            return true;
        }

        public void Clear()
        {
            Item = null;
            Quantity = 0;
        }
    }
}

