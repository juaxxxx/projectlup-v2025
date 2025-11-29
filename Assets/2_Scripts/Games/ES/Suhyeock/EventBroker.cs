using System;
using System.Collections.Generic;
using UnityEngine;

namespace LUP.ES
{
    public class EventBroker : MonoBehaviour
    {
        public Action<bool> OnGameFinished;

        public Action<bool> OnInventoryVisibilityChanged;
        public Action<List<Item>> OnOpenLootDisplay;
        public Action OnCloseLootDisplay;

        public Action<float, float> OnReloadTimeUpdate;

        // 기수 추가한 코드
        public Action ExtractionSuccess;

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

        // 기수 추가한 코드
        public void OnExtractionSuccess()
        {
            Debug.Log("EventBroker: Extraction Success Triggered!");
            ExtractionSuccess?.Invoke();
        }
    }
}