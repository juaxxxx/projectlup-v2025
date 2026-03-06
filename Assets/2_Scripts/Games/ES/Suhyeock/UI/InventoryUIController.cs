using DG.Tweening;
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
        public GameObject inventoryDisplayPanel;

        public GameObject itemSlotPrefab;
        public Transform slotsParent;
        public ItemIconLoader itemIconLoader;
        public Text inventoryCountText;

        private List<InventorySlotUI> uiSlots = new List<InventorySlotUI>();
        public InventorySlotUI weaponSlot;
        private bool isOpen = false;
        private void Start()
        {
            eventBroker = FindAnyObjectByType<EventBroker>();
            inventory = FindAnyObjectByType<Inventory>();
            inventoryDisplayPanel.SetActive(isOpen);
            eventBroker.OnInventoryVisibilityChanged += SetInventoryOpen;
            inventory.OnInventoryUpdated += UpdateUI;
            InitUI();
        }

        private void OnDestroy()
        {
            if (eventBroker != null)
                eventBroker.OnInventoryVisibilityChanged -= SetInventoryOpen;

            if (inventory != null)
                inventory.OnInventoryUpdated -= UpdateUI;
        }
        public void SetInventoryOpen(bool isOpen)
        {
            this.isOpen = isOpen;
            InventoryAnimation(isOpen);
        }

        public void ToggleInventory()
        {
            isOpen = !isOpen;
            InventoryAnimation(isOpen);
        }

        private void InventoryAnimation(bool isOpen)
        {
            inventoryDisplayPanel.transform.DOKill();
            if (isOpen)
            {
                inventoryDisplayPanel.SetActive(true);
                inventoryDisplayPanel.transform.localScale = Vector3.zero;
                inventoryDisplayPanel.transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.OutBack);
            }
            else
            {
                inventoryDisplayPanel.transform.DOScale(Vector3.zero, 0.3f).SetEase(Ease.InBack)
                    .OnComplete(() =>
                    {
                        inventoryDisplayPanel.SetActive(false);
                    });
            }
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
            weaponSlot.Init(-1, this, itemIconLoader);
            UpdateUI();
        }

        public void OnInventorySlotClicked(int slotIndex)
        {
            // 1. 유효성 검사
            if (slotIndex < 0 || slotIndex >= inventory.slots.Count) return;

            InventorySlot slotData = inventory.slots[slotIndex];

            // 2. 빈 슬롯이면 아무것도 안 함
            if (slotData.IsEmpty) return;

            Debug.Log($"슬롯 클릭됨: {slotIndex}번, 아이템: {slotData.item.baseItem.Name}");

            // 3. 인벤토리에게 장착 요청
            inventory.EquipItem(slotIndex);
        }

        public void UpdateUI()
        {
            for (int i = 0; i < uiSlots.Count; i++)
            {
                InventorySlot dataSlot = inventory.slots[i];
                uiSlots[i].UpdateSlot(dataSlot);
            }
            Debug.Log("Inventory UI Updated!");
            weaponSlot.UpdateSlot(inventory.weaponSlot);
        }
    }
}
