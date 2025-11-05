using UnityEngine;

public interface IInventoryItem
{
    int ItemID { get; }
    string ItemName { get; }
    Sprite Icon { get; }

    ItemRuntimeData ToSaveData();

    void FromSaveData(ItemRuntimeData SaveData);
}
