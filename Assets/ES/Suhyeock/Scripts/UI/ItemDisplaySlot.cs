using UnityEngine;
using UnityEngine.UI;

public class ItemDisplaySlot : MonoBehaviour
{
    public ItemIconLoader itemIconLoader;
    public Text nameText;
    public Text countText;
    public Image iconImage;
    public Button acquireButton;

    private Inventory inventory;
    private Item item;
    private void Awake()
    {
        acquireButton.onClick.AddListener(AcquireItemToInventory);
    }
    public void SetInventory(Inventory inventory)
    {
        this.inventory = inventory;
    }

    public void SetItem(Item item)
    {
        if (item == null)
        {
            gameObject.SetActive(false);
            return;
        }
        nameText.text = item.baseItem.name;
        countText.text = "x " + item.count.ToString();
        iconImage.sprite = itemIconLoader.LoadIconSprite(item.baseItem.iconName);
        this.item = item;
        gameObject.SetActive(true);
    }

    private void AcquireItemToInventory()
    {
        if (item != null && inventory != null)
        {
            bool success = inventory.AddItem(item);

            if (success)
            {
                gameObject.SetActive(false);
                Destroy(this.gameObject);
            }
        }
    }
}
