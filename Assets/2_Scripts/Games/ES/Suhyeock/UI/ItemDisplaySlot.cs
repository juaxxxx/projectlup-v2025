using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LUP.ES
{
    public class ItemDisplaySlot : MonoBehaviour
    {
        public ItemIconLoader itemIconLoader;
        public Text nameText;
        public Text countText;
        public Image iconImage;
        public Button acquireButton;

        private Inventory inventory;
        private Item item;
        private List<Item> sourceList;

        private void Awake()
        {
            acquireButton.onClick.AddListener(AcquireItemToInventory);
        }
        public void SetInventory(Inventory inventory)
        {
            this.inventory = inventory;
        }

        public void SetItem(Item item, List<Item> sourceList)
        {
            if (item == null)
            {
                gameObject.SetActive(false);
                return;
            }
            this.item = item;
            this.sourceList = sourceList;

            nameText.text = item.baseItem.Name;
            countText.text = "x " + item.count.ToString();
            iconImage.sprite = itemIconLoader.LoadIconSprite(item.baseItem.ID);
            gameObject.SetActive(true);
        }

        private void AcquireItemToInventory()
        {
            if (item != null && inventory != null)
            {
                bool success = inventory.AddItem(item);

                if (success)
                {
                    if (sourceList != null) sourceList.Remove(item);
                    gameObject.SetActive(false);
                    Destroy(this.gameObject);
                }
            }
        }
    }
}
