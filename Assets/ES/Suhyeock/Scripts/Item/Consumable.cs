using System.Drawing;
using UnityEngine;

namespace ES
{
    public class Consumable : Item
    {
        public float effectDuration;  // 효과 지속 시간
        public float useTime;         // 사용에 걸리는 시간
        public EffectType effectType;     // 발동할 효과의 종류
        public float effectValue;     // 효과의 크기
        public int stackSize;

        public Consumable(ConsumableItemData itemData) : base(itemData)
        {
            effectDuration = itemData.effectDuration;
            useTime = itemData.useTime;
            effectType = itemData.effectType;
            effectValue = itemData.effectValue;
            stackSize = itemData.stackSize;
            count = Random.Range(1, stackSize + 1);
        }
    }
}
