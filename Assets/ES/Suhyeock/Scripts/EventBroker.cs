using System;
using System.Collections.Generic;
using UnityEngine;

public class EventBroker : MonoBehaviour
{
    public Action<bool> OnGameFinished;

    public Action<bool> OnInventoryVisibilityChanged;
    public Action<List<Item>> OnOpenLootDisplay;
    public Action OnCloseLootDisplay;

    public Action<float, float> OnReloadTimeUpdate;

    public void ReportGameFinish(bool isSuccess)
    {
        OnGameFinished?.Invoke(isSuccess);
    }

    public void HandleIventoryVisibility(bool isVisible)
    {
        OnInventoryVisibilityChanged?.Invoke(isVisible);
    }

    public void OpenLootDisplay(List<Item> items)
    {
        OnOpenLootDisplay?.Invoke(items);
    }
    public void CloseLootDisplay()
    {
        OnCloseLootDisplay?.Invoke();
    }

    public void ReloadTimeUpdate(float time, float reloadTime)
    {
        OnReloadTimeUpdate?.Invoke(time, reloadTime);
    }
}
