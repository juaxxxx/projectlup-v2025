using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class LootDisplayCenter : MonoBehaviour
{
    public EventBroker eventBroker;
    public Inventory inventory;
    public GameObject itemSlotPrefab;
    public GameObject lootPanel;
    public GameObject ItemDisplayContent;
    private Transform contentParent;
    

    private void Start()
    {
        eventBroker.OnCloseLootDisplay += CloseLootPanel;
        eventBroker.OnOpenLootDisplay += ShowLootItems;

        lootPanel.SetActive(false);
        contentParent = ItemDisplayContent.transform;
    }

    private void OnDestroy()
    {
        eventBroker.OnCloseLootDisplay -= CloseLootPanel;
        eventBroker.OnOpenLootDisplay -= ShowLootItems;
    }
    public void ShowLootItems(List<Item> items)
    {
        ClearExistingSlots();
        if (items == null || items.Count == 0)
        {
            lootPanel.SetActive(false);
            return;
        }


        for (int i = 0; i < items.Count; i++)
        {
            GameObject newSlot = Instantiate(itemSlotPrefab, contentParent);

            ItemDisplaySlot slot = newSlot.GetComponent<ItemDisplaySlot>();
            if (slot != null)
            {
                Debug.Log("Slot");
                slot.SetInventory(inventory);
                slot.SetItem(items[i]);
            }
        }

        lootPanel.SetActive(true);
    }

    public void CloseLootPanel()
    {
        lootPanel.SetActive(false);
        ClearExistingSlots();
    }

    private void ClearExistingSlots()
    {
        for (int i = 0; i < contentParent.childCount; i++)
        {
            Destroy(contentParent.GetChild(i).gameObject);
        }
    }

}
