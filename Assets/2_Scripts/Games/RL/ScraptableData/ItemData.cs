using LUP.Define;
using System.Collections.Generic;
using UnityEngine;

namespace LUP.RL
{
 
    [CreateAssetMenu(fileName = "ItemData", menuName = "RLGame/ItemData")]
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

    public class ItemDataComparer : IEqualityComparer<ItemData>
    {
        public bool Equals(ItemData x, ItemData y)
    => x.GetDisplayableName() == y.GetDisplayableName();

        public int GetHashCode(ItemData obj)
        {
            return obj.GetDisplayableName().GetHashCode();
        }
    }
}


