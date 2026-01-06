using UnityEngine;
using System.Collections.Generic;

namespace LUP.ES
{
    [CreateAssetMenu(fileName = "ItemIconLoader", menuName = "ES/ItemIconLoader")]
    public class ItemIconLoader : ScriptableObject
    {
        public List<ItemIconEntry> iconEntries = new List<ItemIconEntry>();

        private Dictionary<int, Sprite> itemIcons = null;

        public void Initialize()
        {
            if (itemIcons != null) return;

            itemIcons = new Dictionary<int, Sprite>();

            foreach (ItemIconEntry entry in iconEntries)
            {
                if (!itemIcons.ContainsKey(entry.itemID))
                {
                    itemIcons.Add(entry.itemID, entry.iconSprite);
                }
            }
        }

        public Sprite LoadIconSprite(int ID)
        {
            if (itemIcons == null)
            {
                Initialize();
            }

            if (itemIcons != null && itemIcons.TryGetValue(ID, out Sprite icon))
            {
                return icon;
            }
            return null;
        }
    }
}