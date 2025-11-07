using UnityEngine;

namespace ES
{
    public class CraftingMaterial : Item
    {
        public MaterialTier tier;
        public int stackSize;     //한 슬롯에 최대로 쌓을 수 있는 개수

        public CraftingMaterial(MaterialItemData itemData) : base(itemData)
        {
            tier = itemData.tier;
            stackSize = itemData.stackSize;
            count = Random.Range(1, stackSize + 1);
        }
    }
}
