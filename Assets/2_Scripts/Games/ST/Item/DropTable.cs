using UnityEngine;
using System.Collections.Generic;
namespace LUP.ST
{

    [CreateAssetMenu(fileName = "Drop Table", menuName = "Game/Drop Table")]
    public class DropTable : ScriptableObject
    {
        [System.Serializable]
        public class DropItem
        {
            public ItemData item;
            public int goldAmount;          // 골드 양 (골드 타입인 경우)
            public float dropChance;        // 드롭 확률 0~1
        }

        [Header("드롭 리스트")]
        public List<DropItem> dropItems = new List<DropItem>();

        public List<DropItem> GetDrops()
        {
            List<DropItem> drops = new List<DropItem>();

            foreach (DropItem dropItem in dropItems)
            {
                if (Random.value <= dropItem.dropChance)
                {
                    drops.Add(dropItem);
                }
            }

            return drops;
        }
    }
}