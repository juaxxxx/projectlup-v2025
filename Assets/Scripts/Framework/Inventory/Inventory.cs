using System;
using System.Collections.Generic;
using UnityEngine;

namespace Framework
{
    public class Inventory : MonoBehaviour
    {
        [Header("인벤토리 설정  ")]
        [SerializeField] protected int capacity = 30;
        [SerializeField] protected bool shouldPersist = false;

        protected List<InventorySlot> slots;
        protected Dictionary<string, long> currencies;
        protected Dictionary<string, IInventoryItemable> equipmentSlots;

        public event Action<IInventoryItemable, int> OnItemAdded;
        public event Action<IInventoryItemable, int> OnItemRemoved;
        public event Action<IInventoryItemable, int> OnItemUsed;
        public event Action<int> OnSlotChanged;
        public event Action OnInventoryUpdated;

        public event Action<string, long> OnCurrencyAdded;
        public event Action<string, long> OnCurrencyRemoved;
        public event Action<string, long> OnCurrencyChanged;

        public event Action<string, IInventoryItemable> OnItemEquipped;
        public event Action<string, IInventoryItemable> OnItemUnequipped;

        public int Capacity => capacity;
        public bool ShouldPersist => shouldPersist;
        public List<InventorySlot> Slots => slots;
        public Dictionary<string, long> Currencies => currencies;
        public Dictionary<string, IInventoryItemable> EquipmentSlots => equipmentSlots;

        protected virtual void Awake()
        {
            InitializeSlots();
            InitializeCurrencies();
            InitializeEquipmentSlots();
            RegisterToManager();
        }

        protected virtual void InitializeSlots()
        {
            slots = new List<InventorySlot>(capacity);
            for (int i = 0; i < capacity; i++)
                slots.Add(new InventorySlot(i));
        }

        protected virtual void InitializeCurrencies()
        {
            currencies = new Dictionary<string, long>();
        }

        protected virtual void InitializeEquipmentSlots()
        {
            equipmentSlots = new Dictionary<string, IInventoryItemable>();
        }

        protected virtual void RegisterToManager()
        {
            if (Manager.InventoryManager.Instance != null)
                Manager.InventoryManager.Instance.RegisterInventory(this);
        }

        public virtual bool AddItem(IInventoryItemable item, int amount = 1)
        {
            if (item == null || amount <= 0)
            {
                Debug.LogWarning("Invalid item or amount");
                return false;
            }

            int remainingAmount = amount;

            // 1. 스택 가능한 경우 기존 슬롯에 추가 시도
            if (item.IsStackable)
            {
                foreach (var slot in slots)
                {
                    if (!slot.IsEmpty && slot.Item.ItemID == item.ItemID && !slot.IsFull)
                    {
                        int added = slot.AddItem(item, remainingAmount);
                        remainingAmount -= added;

                        OnSlotChanged?.Invoke(slot.SlotIndex);

                        if (remainingAmount <= 0)
                        {
                            OnItemAdded?.Invoke(item, amount);
                            OnInventoryUpdated?.Invoke();
                            BroadcastItemAdded(item, amount);
                            return true;
                        }
                    }
                }
            }

            // 2. 빈 슬롯 찾기
            while (remainingAmount > 0)
            {
                InventorySlot emptySlot = FindEmptySlot();
                if (emptySlot == null)
                {
                    Debug.LogWarning($"Inventory full! Could not add {remainingAmount} of {item.ItemName}");

                    // 부분 성공
                    if (remainingAmount < amount)
                    {
                        int addedAmount = amount - remainingAmount;
                        OnItemAdded?.Invoke(item, addedAmount);
                        OnInventoryUpdated?.Invoke();
                        BroadcastItemAdded(item, addedAmount);
                    }

                    return false;
                }

                int toAdd = Mathf.Min(remainingAmount, item.MaxStackSize);
                emptySlot.AddItem(item, toAdd);
                remainingAmount -= toAdd;

                OnSlotChanged?.Invoke(emptySlot.SlotIndex);
            }

            OnItemAdded?.Invoke(item, amount);
            OnInventoryUpdated?.Invoke();
            BroadcastItemAdded(item, amount);
            return true;
        }

        /// <summary>
        /// 슬롯 인덱스로 아이템 제거
        /// </summary>
        public virtual bool RemoveItem(int slotIndex, int amount = 1)
        {
            if (slotIndex < 0 || slotIndex >= slots.Count)
            {
                Debug.LogError($"Invalid slot index: {slotIndex}");
                return false;
            }

            var slot = slots[slotIndex];
            if (slot.IsEmpty)
            {
                Debug.LogWarning($"Slot {slotIndex} is empty");
                return false;
            }

            IInventoryItemable item = slot.Item;
            int removed = slot.RemoveItem(amount);

            if (removed > 0)
            {
                OnSlotChanged?.Invoke(slotIndex);
                OnItemRemoved?.Invoke(item, removed);
                OnInventoryUpdated?.Invoke();
                BroadcastItemRemoved(item, removed);
                return true;
            }

            return false;
        }

        /// <summary>
        /// 아이템 ID로 제거
        /// </summary>
        public virtual bool RemoveItemByID(int itemID, int amount = 1)
        {
            int remainingAmount = amount;

            for (int i = 0; i < slots.Count && remainingAmount > 0; i++)
            {
                var slot = slots[i];
                if (!slot.IsEmpty && slot.Item.ItemID == itemID)
                {
                    int toRemove = Mathf.Min(remainingAmount, slot.Count);
                    RemoveItem(i, toRemove);
                    remainingAmount -= toRemove;
                }
            }

            return remainingAmount == 0;
        }

        /// <summary>
        /// 아이템 사용 (소비 아이템용)
        /// </summary>
        public virtual bool UseItem(int slotIndex)
        {
            if (slotIndex < 0 || slotIndex >= slots.Count)
            {
                Debug.LogError($"Invalid slot index: {slotIndex}"); 
                return false;
            }

            var slot = slots[slotIndex];
            if (slot.IsEmpty)
            {
                Debug.LogWarning($"Slot {slotIndex} is empty");
                return false;
            }

            IInventoryItemable item = slot.Item;

            if (!item.CanUse())
            {
                Debug.LogWarning($"{item.ItemName} cannot be used");
                return false;
            }

            // 아이템 사용 이벤트 발생
            OnItemUsed?.Invoke(item, 1);

            // 아이템 1개 제거
            RemoveItem(slotIndex, 1);

            Debug.Log($"Used item: {item.ItemName}");
            return true;
        }

        /// <summary>
        /// 아이템 ID로 사용
        /// </summary>
        public virtual bool UseItemByID(int itemID)
        {
            InventorySlot slot = FindSlotByItemID(itemID);
            if (slot != null)
            {
                return UseItem(slot.SlotIndex);
            }

            Debug.LogWarning($"Item with ID {itemID} not found");
            return false;
        }

        /// <summary>
        /// 재화 추가 (예: "Gold", "Diamond")
        /// </summary>
        public virtual bool AddCurrency(string currencyName, int amount)
        {
            if (string.IsNullOrEmpty(currencyName) || amount <= 0)
            {
                Debug.LogWarning("Invalid currency name or amount");
                return false;
            }

            if (!currencies.ContainsKey(currencyName))
            {
                currencies[currencyName] = 0;
            }

            currencies[currencyName] += amount;

            OnCurrencyAdded?.Invoke(currencyName, amount);
            OnCurrencyChanged?.Invoke(currencyName, currencies[currencyName]);
            OnInventoryUpdated?.Invoke();

            Debug.Log($"Added {amount} {currencyName}. Total: {currencies[currencyName]}");
            return true;
        }

        /// <summary>
        /// 재화 제거
        /// </summary>
        public virtual bool RemoveCurrency(string currencyName, int amount)
        {
            if (string.IsNullOrEmpty(currencyName) || amount <= 0)
            {
                Debug.LogWarning("Invalid currency name or amount");
                return false;
            }

            if (!currencies.ContainsKey(currencyName) || currencies[currencyName] < amount)
            {
                Debug.LogWarning($"Not enough {currencyName}. Have: {GetCurrency(currencyName)}, Need: {amount}");
                return false;
            }

            currencies[currencyName] -= amount;

            OnCurrencyRemoved?.Invoke(currencyName, amount);
            OnCurrencyChanged?.Invoke(currencyName, currencies[currencyName]);
            OnInventoryUpdated?.Invoke();

            Debug.Log($"Removed {amount} {currencyName}. Remaining: {currencies[currencyName]}");
            return true;
        }

        /// <summary>
        /// 재화 설정
        /// </summary>
        public virtual void SetCurrency(string currencyName, int amount)
        {
            if (string.IsNullOrEmpty(currencyName))
            {
                Debug.LogWarning("Invalid currency name");
                return;
            }

            long oldAmount = GetCurrency(currencyName);
            currencies[currencyName] = Mathf.Max(0, amount);

            OnCurrencyChanged?.Invoke(currencyName, currencies[currencyName]);
            OnInventoryUpdated?.Invoke();

            Debug.Log($"Set {currencyName} to {currencies[currencyName]}");
        }

        /// <summary>
        /// 재화 조회
        /// </summary>
        public virtual long GetCurrency(string currencyName)
        {
            if (currencies.ContainsKey(currencyName))
            {
                return currencies[currencyName];
            }
            return 0;
        }

        /// <summary>
        /// 재화 보유 여부
        /// </summary>
        public virtual bool HasCurrency(string currencyName, long amount)
        {
            return GetCurrency(currencyName) >= amount;
        }

        /// <summary>
        /// 장비 장착
        /// </summary>
        public virtual bool EquipItem(string equipSlotName, IInventoryItemable item)
        {
            if (string.IsNullOrEmpty(equipSlotName) || item == null)
            {
                Debug.LogWarning("Invalid equipment slot or item");
                return false;
            }

            // 이미 장착된 아이템이 있으면 해제
            if (equipmentSlots.ContainsKey(equipSlotName) && equipmentSlots[equipSlotName] != null)
            {
                UnequipItem(equipSlotName);
            }

            // 장비 장착
            equipmentSlots[equipSlotName] = item;

            OnItemEquipped?.Invoke(equipSlotName, item);
            OnInventoryUpdated?.Invoke();

            Debug.Log($"Equipped {item.ItemName} to {equipSlotName}");
            return true;
        }

        /// <summary>
        /// 슬롯에서 장비 장착
        /// </summary>
        public virtual bool EquipItemFromSlot(int slotIndex, string equipSlotName)
        {
            if (slotIndex < 0 || slotIndex >= slots.Count)
            {
                Debug.LogError($"Invalid slot index: {slotIndex}");
                return false;
            }

            var slot = slots[slotIndex];
            if (slot.IsEmpty)
            {
                Debug.LogWarning($"Slot {slotIndex} is empty");
                return false;
            }

            IInventoryItemable item = slot.Item;

            // 장비 장착
            if (EquipItem(equipSlotName, item))
            {
                // 인벤토리에서 제거
                RemoveItem(slotIndex, 1);
                return true;
            }

            return false;
        }

        public virtual bool UnequipItem(string equipSlotName)
        {
            if (string.IsNullOrEmpty(equipSlotName))
            {
                Debug.LogWarning("Invalid equipment slot");
                return false;
            }

            if (!equipmentSlots.ContainsKey(equipSlotName) || equipmentSlots[equipSlotName] == null)
            {
                Debug.LogWarning($"No item equipped in {equipSlotName}");
                return false;
            }

            IInventoryItemable item = equipmentSlots[equipSlotName];

            // 인벤토리에 공간이 있는지 확인
            if (!HasSpace())
            {
                Debug.LogWarning("Inventory is full! Cannot unequip item");
                return false;
            }

            // 인벤토리에 추가
            if (AddItem(item, 1))
            {
                equipmentSlots[equipSlotName] = null;

                OnItemUnequipped?.Invoke(equipSlotName, item);
                OnInventoryUpdated?.Invoke();

                Debug.Log($"Unequipped {item.ItemName} from {equipSlotName}");
                return true;
            }

            return false;
        }

        public virtual IInventoryItemable GetEquippedItem(string equipSlotName)
        {
            if (equipmentSlots.ContainsKey(equipSlotName))
            {
                return equipmentSlots[equipSlotName];
            }
            return null;
        }

        public virtual bool IsEquipSlotEmpty(string equipSlotName)
        {
            return GetEquippedItem(equipSlotName) == null;
        }

        public virtual void ClearSlot(int slotIndex)
        {
            if (slotIndex < 0 || slotIndex >= slots.Count)
            {
                Debug.LogError($"Invalid slot index: {slotIndex}");
                return;
            }

            var slot = slots[slotIndex];
            if (!slot.IsEmpty)
            {
                IInventoryItemable item = slot.Item;
                int count = slot.Count;
                slot.Clear();

                OnSlotChanged?.Invoke(slotIndex);
                OnItemRemoved?.Invoke(item, count);
                OnInventoryUpdated?.Invoke();
                BroadcastItemRemoved(item, count);
            }
        }

        public virtual void ClearAll()
        {
            foreach (var slot in slots)
            {
                slot.Clear();
            }

            currencies.Clear();
            equipmentSlots.Clear();

            OnInventoryUpdated?.Invoke();
            Debug.Log($"[{GetType().Name}] Inventory completely cleared");
        }

        public virtual InventorySlot FindEmptySlot()
        {
            return slots.Find(s => s.IsEmpty);
        }

        public virtual InventorySlot FindSlotByItemID(int itemID)
        {
            return slots.Find(s => !s.IsEmpty && s.Item.ItemID == itemID);
        }

        public virtual int GetEmptySlotCount()
        {
            return slots.FindAll(s => s.IsEmpty).Count;
        }

        public virtual bool HasSpace()
        {
            return FindEmptySlot() != null;
        }

        public virtual bool HasItem(int itemID, int amount = 1)
        {
            int totalCount = 0;

            foreach (var slot in slots)
            {
                if (!slot.IsEmpty && slot.Item.ItemID == itemID)
                {
                    totalCount += slot.Count;
                    if (totalCount >= amount)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public virtual int GetItemCount(int itemID)
        {
            int count = 0;

            foreach (var slot in slots)
            {
                if (!slot.IsEmpty && slot.Item.ItemID == itemID)
                {
                    count += slot.Count;
                }
            }

            return count;
        }

        public virtual InventorySlot GetSlot(int index)
        {
            if (index < 0 || index >= slots.Count)
            {
                Debug.LogError($"Invalid slot index: {index}");
                return null;
            }

            return slots[index];
        }

        public virtual void SwapSlots(int slotIndexA, int slotIndexB)
        {
            if (slotIndexA < 0 || slotIndexA >= slots.Count ||
                slotIndexB < 0 || slotIndexB >= slots.Count)
            {
                Debug.LogError("Invalid slot indices for swap");
                return;
            }

            slots[slotIndexA].SwapWith(slots[slotIndexB]);

            OnSlotChanged?.Invoke(slotIndexA);
            OnSlotChanged?.Invoke(slotIndexB);
            OnInventoryUpdated?.Invoke();
        }

        protected virtual void BroadcastItemAdded(IInventoryItemable item, int amount)
        {
            if (Manager.InventoryManager.Instance != null)
            {
                Manager.InventoryManager.Instance.BroadcastItemAdded(item, amount);
            }
        }

        protected virtual void BroadcastItemRemoved(IInventoryItemable item, int amount)
        {
            if (Manager.InventoryManager.Instance != null)
            {
                Manager.InventoryManager.Instance.BroadcastItemRemoved(item, amount);
            }
        }

        #region 저장/로드

        /// <summary>
        /// 인벤토리를 저장 데이터로 변환
        /// </summary>
        public virtual InventoryRuntimeData ToSaveData()
        {
            InventoryRuntimeData saveData = new InventoryRuntimeData();
            saveData.InitializeEmptySlots(capacity);

            // 슬롯 저장
            for (int i = 0; i < slots.Count; i++)
            {
                saveData.SetSlot(i, slots[i].ToSaveData());
            }

            // 재화 저장
            saveData.SaveCurrencies(currencies);

            // 장비 저장
            saveData.SaveEquipments(equipmentSlots);

            return saveData;
        }

        /// <summary>
        /// 저장 데이터에서 인벤토리 복원
        /// </summary>
        public virtual void FromSaveData(InventoryRuntimeData saveData)
        {
            if (saveData == null)
            {
                Debug.LogWarning("[Inventory] Save data is null");
                return;
            }

            // 슬롯 로드
            for (int i = 0; i < saveData.Slots.Count && i < slots.Count; i++)
            {
                ItemRuntimeData itemData = saveData.GetSlot(i);
                if (itemData != null && itemData.ItemId != -1)
                {
                    IInventoryItemable item = Manager.InventoryManager.Instance?.GetItemData(itemData.ItemId);
                    if (item != null)
                    {
                        slots[i].SetItem(item, itemData.Count);
                    }
                }
            }

            // 재화 로드
            Dictionary<string, long> loadedCurrencies = saveData.LoadCurrencies();
            foreach (var currency in loadedCurrencies)
            {
                currencies[currency.Key] = currency.Value;
            }

            // 장비 로드
            Dictionary<string, int> equipmentIds = saveData.LoadEquipmentIds();
            foreach (var equip in equipmentIds)
            {
                IInventoryItemable item = Manager.InventoryManager.Instance?.GetItemData(equip.Value);
                if (item != null)
                {
                    equipmentSlots[equip.Key] = item;
                }
            }

            OnInventoryUpdated?.Invoke();
            Debug.Log($"[Inventory] Loaded from save data");
        }

        #endregion
    }
}
