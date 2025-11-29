using UnityEngine;
using System.Collections.Generic;
namespace LUP.ST
{

    public static class GameData
    {
        // »πµÊ«— ∞ÒµÂ
        public static int TotalGold { get; set; }

        // »πµÊ«— ¿Á∑· æ∆¿Ã≈€µÈ
        public static List<ItemData> CollectedMaterials { get; private set; } = new List<ItemData>();

        // ≈≥ ƒ´øÓ∆Æ
        public static int TotalKills { get; set; }

        // «√∑π¿Ã Ω√∞£
        public static float PlayTime { get; set; }

        public static void Reset()
        {
            TotalGold = 0;
            CollectedMaterials.Clear();
            TotalKills = 0;
            PlayTime = 0f;
        }

        public static void AddGold(int amount)
        {
            TotalGold += amount;
            Debug.Log($"∞ÒµÂ »πµÊ: +{amount} (√—: {TotalGold})");
        }

        public static void AddMaterial(ItemData item)
        {
            CollectedMaterials.Add(item);
            Debug.Log($"¿Á∑· »πµÊ: {item.itemName}");
        }
    }
}