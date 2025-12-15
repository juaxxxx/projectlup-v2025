using UnityEngine;

using LUP.Define;

namespace LUP.RL
{
    [CreateAssetMenu(fileName = "ItemData", menuName = "Scriptable Objects/ItemData")]
    public class ItemData : ScriptableObject, IDisplayable
    {
        [SerializeField]
        private string itemName;

        [SerializeField]
        private Sprite itemImage;

        [SerializeField]
        private int itemAmount;

        public ItemType itemType;

        public string GetDisplayableName() { return itemName; }
        public Sprite GetDisplayableImage() { return itemImage; }

        public void SetDisplayableImage(Sprite image) { itemImage = image; }
        public void SetItemName(string name) { itemName = name; }

        public int GetExtraInfo() { return itemAmount; }
        public void SetExtraInfo(int amount) { itemAmount = amount; }
    }
}


