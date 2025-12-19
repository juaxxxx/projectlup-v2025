using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LUP.ES
{
    public class InventoryUIController : MonoBehaviour
    {
        [HideInInspector]
        public EventBroker eventBroker;
        [HideInInspector]
        public Inventory inventory;
        public GameObject iventoryDisplayPanel;

        public GameObject itemSlotPrefab;
        public Transform slotsParent;
        public ItemIconLoader itemIconLoader;
        public Text inventoryCountText;

        private List<InventorySlotUI> uiSlots = new List<InventorySlotUI>();

        private bool isOpen = false;
        private void Start()
        {
            eventBroker = FindAnyObjectByType<EventBroker>();
            inventory = FindAnyObjectByType<Inventory>();
            iventoryDisplayPanel.SetActive(isOpen);
            eventBroker.OnInventoryVisibilityChanged += SetInventoryOpen;
            inventory.OnInventoryUpdated += UpdateUI;
            InitUI();
        }

        private void OnDestroy()
        {
            eventBroker.OnInventoryVisibilityChanged -= SetInventoryOpen;
        }
        public void SetInventoryOpen(bool isOpen)
        {
            this.isOpen = isOpen;
            iventoryDisplayPanel.SetActive(isOpen);
        }

        public void ToggleInventory()
        {
            isOpen = !isOpen;
            iventoryDisplayPanel.SetActive(isOpen);
        }

        private void InitUI()
        {
            inventoryCountText.text = "(" + "0" + "/" + inventory.slots.Count + ")";
            for (int i = 0; i < inventory.slots.Count; i++)
            {
                GameObject uiObject = Instantiate(itemSlotPrefab, slotsParent);
                InventorySlotUI uiSlot = uiObject.GetComponent<InventorySlotUI>();

                uiSlot.Init(i, this, itemIconLoader);
                uiSlots.Add(uiSlot);
            }
        }

        public void UpdateUI()
        {
            // 이벤트 발생 시, 모든 UI 슬롯을 현재 인벤토리 데이터로 갱신합니다.
            for (int i = 0; i < uiSlots.Count; i++)
            {
                InventorySlot dataSlot = inventory.slots[i];
                uiSlots[i].UpdateSlot(dataSlot);
            }
            Debug.Log("Inventory UI Updated!");
        }
    }
}
