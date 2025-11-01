using System.Collections.Generic;
using System;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public event Action OnInventoryUpdated;
    public int inventorySize = 30;
    public List<InventorySlot> slots;

    private void Awake()
    {
        slots = new List<InventorySlot>(inventorySize);
        for (int i = 0; i < inventorySize; i++)
        {
            slots.Add(new InventorySlot());
        }
    }

    public bool AddItem(Item item)
    {
        // 1. 스택 가능한 아이템이라면 기존 슬롯 검색 후 추가 (Stacking Logic)
        if(item.baseItem.type == ItemType.Consumable || item.baseItem.type == ItemType.Material)
        {
            for (int i = 0; i < slots.Count; i++)
            {
                InventorySlot slot = slots[i];
                if (slot.item == null)
                    continue;
                if (slot.item.baseItem.id == item.baseItem.id)
                {
                    int canAdd = slot.item.baseItem.stackSize - slot.item.count;

                    int actualAdd = Math.Min(canAdd, item.count);
                    
                    slot.item.count += actualAdd;
                    item.count -= actualAdd;

                    if (item.count <= 0)
                    {
                        OnInventoryUpdated?.Invoke();
                        return true;
                    }
                }
            }
        }

        // 2. 새로운 슬롯에 아이템 추가
        InventorySlot emptySlot = slots.Find(s => s.IsEmpty);
        if (emptySlot != null)
        {
            emptySlot.item = item;

            OnInventoryUpdated?.Invoke();
            return true;
        }

        return false; // 인벤토리가 가득 참
    }
}
