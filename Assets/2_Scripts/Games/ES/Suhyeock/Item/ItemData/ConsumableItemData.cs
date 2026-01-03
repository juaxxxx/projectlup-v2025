using System;
using UnityEngine;

namespace LUP.ES
{
    public enum EffectType
    {
        Heal,
    
    }
    [Serializable]
    public class ConsumableItemData : BaseItemData
    {
        public float effectDuration;  // 효과 지속 시간
        public float useTime;         // 사용에 걸리는 시간
        public EffectType effectType;     // 발동할 효과의 종류
        public float effectValue;     // 효과의 크기


        public ConsumableItemData(int id, string name, string description, string iconName, float dropChance, float effectDuration, float useTime, EffectType effectType, float effectValue, int stackSize) : base(id, name, description,iconName, stackSize, dropChance)
        {
            itemType = ItemType.Consumable;
            this.effectDuration = effectDuration;
            this.useTime = useTime;
            this.effectType = effectType;
            this.effectValue = effectValue;
        }
    }
}
