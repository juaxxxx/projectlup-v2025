using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
//using static UnityEditor.Progress;

namespace LUP.ES
{
    public class Inventory : MonoBehaviour
    {
        public event Action OnInventoryUpdated;
        public int inventorySize = 30;
        public List<InventorySlot> slots;
        public InventorySlot weaponSlot;
        private PlayerBlackboard playerBlackboard;
        private WeaponEquip weaponEquip;
        void Start()
        {
            playerBlackboard = FindAnyObjectByType<PlayerBlackboard>();
            weaponEquip = playerBlackboard.GetComponent<WeaponEquip>();
            slots = new List<InventorySlot>(inventorySize);
            for (int i = 0; i < inventorySize; i++)
            {
                slots.Add(new InventorySlot());
            }
            weaponSlot = new InventorySlot();
            weaponSlot.item = playerBlackboard.weapon.weaponItem;
        }

        public bool AddItem(Item item)
        {
            // 1. 스택 가능한 아이템이라면 기존 슬롯 검색 후 추가 (Stacking Logic)
            if (item.baseItem.itemType == ItemType.Consumable || item.baseItem.itemType == ItemType.Material)
            {
                for (int i = 0; i < slots.Count; i++)
                {
                    InventorySlot slot = slots[i];
                    if (slot.item == null)
                        continue;
                    if (slot.item.baseItem.ID == item.baseItem.ID)
                    {
                        int canAdd = slot.item.baseItem.MaxStackSize - slot.item.count;

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
        public void EquipItem(int slotIndex)
        {
            InventorySlot slot = slots[slotIndex];
            if (slot.IsEmpty) return;

            // 1. 아이템 데이터 가져오기
            Item itemToEquip = slot.item;

            if (itemToEquip == null)
                return;

            if (itemToEquip.baseItem.itemType == ItemType.Material ||
                itemToEquip.baseItem.itemType == ItemType.Consumable)
                return;

            if (weaponEquip == null)
                return;

            Item previousItem = playerBlackboard.weapon.weaponItem;
            switch (playerBlackboard.extractionShooterStage.RuntimeData.PlayerID)
            {
                case 0:
                    MeleeWeaponItemData meleeWeaponItemData = itemToEquip.baseItem as MeleeWeaponItemData;
                    if (meleeWeaponItemData == null)
                        return;
                    break;
                case 1:
                    RangedWeaponItemData rangedWeaponItemData = itemToEquip.baseItem as RangedWeaponItemData;
                    if (rangedWeaponItemData == null)
                        return;
                    break;
                case 2:
                    ThrowingWeaponData throwingWeaponData = itemToEquip.baseItem as ThrowingWeaponData;
                    if (throwingWeaponData == null)
                        return;
                    break;
                default:
                    break;
            }
            playerBlackboard.CurrentWeaponID = itemToEquip.baseItem.ID;
            weaponEquip.EquipWeapon();
            weaponSlot.item = slot.item;
            slot.item = previousItem;
            //RemoveItem(slotIndex);

            // 변경 사항 알림 (이것이 호출되면 InventoryUIController.UpdateUI가 실행됨)
            OnInventoryUpdated?.Invoke();
        }

        public void RemoveItem(int slotIndex)
        {
            if (slotIndex < 0 || slotIndex >= slots.Count) return;

            // 해당 슬롯을 비움 (null 처리)
            slots[slotIndex].item = null;
        }

        public List<Item> GetItems()
        {
            List<Item> items = new List<Item>();
            foreach (InventorySlot slot in slots)
            {
                if (slot.item == null)
                    continue;
                if (slot.item.ItemID == 1 || slot.item.ItemID == 4 || slot.item.ItemID == 7)
                    continue;
                items.Add(slot.item);
            }
            items.Add(playerBlackboard.weapon.weaponItem);
            return items;
        }
    }
}


