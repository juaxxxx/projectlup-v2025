using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 인벤토리 전체의 저장 데이터
/// JSON으로 직렬화하여 파일에 저장됨
/// </summary>
[Serializable]
public class InventoryRuntimeData : BaseRuntimeData
{
    [SerializeField] private int _capacity = 30;
    [SerializeField] private List<ItemRuntimeData> _slots = new List<ItemRuntimeData>();

    // 재화 저장
    [SerializeField] private List<string> _currencyKeys = new List<string>();
    [SerializeField] private List<long> _currencyValues = new List<long>();

    // 장비 저장 (슬롯명 + 아이템ID)
    [SerializeField] private List<string> _equipmentSlotNames = new List<string>();
    [SerializeField] private List<int> _equippedItemIds = new List<int>();

    public int Capacity
    {
        get => _capacity;
        set => SetValue(ref _capacity, value);
    }

    public List<ItemRuntimeData> Slots
    {
        get => _slots;
        set => SetValue(ref _slots, value);
    }

    public override void ResetData()
    {
        _capacity = 30;
        _slots.Clear();
        _currencyKeys.Clear();
        _currencyValues.Clear();
        _equipmentSlotNames.Clear();
        _equippedItemIds.Clear();
    }

    /// <summary>
    /// 빈 슬롯으로 초기화
    /// </summary>
    public void InitializeEmptySlots(int capacity)
    {
        _capacity = capacity;
        _slots.Clear();

        for (int i = 0; i < capacity; i++)
        {
            _slots.Add(new ItemRuntimeData
            {
                ItemId = -1,
                Count = 0
            });
        }
    }

    /// <summary>
    /// 특정 슬롯에 아이템 저장
    /// </summary>
    public void SetSlot(int index, ItemRuntimeData itemData)
    {
        if (index < 0 || index >= _slots.Count)
        {
            Debug.LogError($"Invalid slot index: {index}");
            return;
        }

        _slots[index] = itemData;
    }

    /// <summary>
    /// 특정 슬롯 데이터 가져오기
    /// </summary>
    public ItemRuntimeData GetSlot(int index)
    {
        if (index < 0 || index >= _slots.Count)
        {
            Debug.LogError($"Invalid slot index: {index}");
            return null;
        }

        return _slots[index];
    }

    /// <summary>
    /// 재화 저장
    /// </summary>
    public void SaveCurrencies(Dictionary<string, long> currencies)
    {
        _currencyKeys.Clear();
        _currencyValues.Clear();

        foreach (var currency in currencies)
        {
            _currencyKeys.Add(currency.Key);
            _currencyValues.Add(currency.Value);
        }
    }

    /// <summary>
    /// 재화 로드
    /// </summary>
    public Dictionary<string, long> LoadCurrencies()
    {
        Dictionary<string, long> currencies = new Dictionary<string, long>();

        for (int i = 0; i < _currencyKeys.Count && i < _currencyValues.Count; i++)
        {
            currencies[_currencyKeys[i]] = _currencyValues[i];
        }

        return currencies;
    }

    /// <summary>
    /// 장비 저장
    /// </summary>
    public void SaveEquipments(Dictionary<string, IInventoryItemable> equipments)
    {
        _equipmentSlotNames.Clear();
        _equippedItemIds.Clear();

        foreach (var equipment in equipments)
        {
            if (equipment.Value != null)
            {
                _equipmentSlotNames.Add(equipment.Key);
                _equippedItemIds.Add(equipment.Value.ItemID);
            }
        }
    }

    /// <summary>
    /// 장비 로드 (아이템 ID만 반환, 실제 로드는 Inventory에서)
    /// </summary>
    public Dictionary<string, int> LoadEquipmentIds()
    {
        Dictionary<string, int> equipmentIds = new Dictionary<string, int>();

        for (int i = 0; i < _equipmentSlotNames.Count && i < _equippedItemIds.Count; i++)
        {
            equipmentIds[_equipmentSlotNames[i]] = _equippedItemIds[i];
        }

        return equipmentIds;
    }
}
