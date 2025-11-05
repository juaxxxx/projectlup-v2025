using System;
using UnityEngine;

[Serializable]
public class ItemRuntimeData : BaseRuntimeData
{
    [SerializeField] private int _itemId = 0;
    [SerializeField] private int _count = 0;

    public int ItemId
    {
        get => _itemId;
        set => SetValue(ref _itemId, value);
    }

    public int Count
    {
        get => _count;
        set => SetValue(ref _count, value);
    }

    public override void ResetData()
    {
        _itemId = 0;
        _count = 0;
    }
}