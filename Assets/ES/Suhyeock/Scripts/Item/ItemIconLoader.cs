using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "ItemIconLoader", menuName = "Scriptable Objects/ItemIconLoader")]
public class ItemIconLoader : ScriptableObject
{
    public List<ItemIconEntry> iconEntries = new List<ItemIconEntry>();

    private Dictionary<string, Sprite> itemIcons = null;

    public void Initialize()
    {
        if (itemIcons != null) return;

        itemIcons = new Dictionary<string, Sprite>();

        foreach (ItemIconEntry entry in iconEntries)
        {
            if (!itemIcons.ContainsKey(entry.iconName))
            {
                itemIcons.Add(entry.iconName, entry.iconSprite);
            }
        }
    }

    public Sprite LoadIconSprite(string iconName)
    {
        if (itemIcons == null)
        {
            Initialize();
        }

        if (itemIcons != null && itemIcons.TryGetValue(iconName, out Sprite icon))
        {
            return icon;
        }
        return null;
    }
}