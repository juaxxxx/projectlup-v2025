using UnityEngine;

public interface IInventoryItemable
{
    int ItemID { get; }
    string ItemName { get; }
    Sprite Icon { get; }
    Define.ItemType ItemType { get; }
    int MaxStackSize { get; }
    bool IsStackable { get; }

    bool CanUse();

    ItemRuntimeData ToSaveData();
    void FromSaveData(ItemRuntimeData saveData);
}
