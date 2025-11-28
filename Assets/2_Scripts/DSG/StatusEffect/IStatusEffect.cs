using LUP.DSG.Utils.Enums;
using UnityEngine;
using static LUP.DSG.Character;

namespace LUP.DSG
{
    public abstract class IStatusEffect
    {
        public EStatusEffectType effectType { get; private set; }
        public EOperationType opType { get; private set; }
        public int remainingTurns;
        public float amount;

        public IStatusEffect(EStatusEffectType EffectType, EOperationType OpType,
            float Amount, int Turn)
        {
            effectType = EffectType;
            opType = OpType;
            amount = Amount;
            remainingTurns = Turn;
        }
        public virtual void Apply(Character target) { } //@TODO : 기본 상태이상도 operation계산해서 적용
        public virtual void Turn(Character target) { }
        public virtual void Remove(Character target) { }
    }
}