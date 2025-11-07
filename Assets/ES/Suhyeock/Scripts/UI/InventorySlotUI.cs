using UnityEngine;
using UnityEngine.UI;

namespace ES
{
    public class InventorySlotUI : MonoBehaviour
    {
        public Image iconImgae;
        public Text stackText;

        private int slotIndex;
        private InventoryUIController inventoryUIController;
        private ItemIconLoader itemIconLoader;

        public void Init(int slotIndex, InventoryUIController inventoryUIController, ItemIconLoader itemIconLoader)
        {
            this.slotIndex = slotIndex;
            this.inventoryUIController = inventoryUIController;
            this.itemIconLoader = itemIconLoader;
        }

        public void UpdateSlot(InventorySlot dataSlot)
        {
            if (dataSlot.IsEmpty)
            {
                iconImgae.gameObject.SetActive(false);
                stackText.text = "";
            }
            else
            {
                iconImgae.gameObject.SetActive(true);
                iconImgae.sprite = itemIconLoader.LoadIconSprite(dataSlot.item.baseItem.iconName);

                stackText.text = dataSlot.item.count.ToString();
            }
        }

    }
}
