using LUP.DSG;
using UnityEngine;
using LUP.DSG.Utils.Enums;

namespace LUP.DSG
{
    public class StatusEffectFactory
    {
        public IStatusEffect CreateStatusEffect(EStatusEffectType Type, EOperationType OpType = EOperationType.Plus,
            float Stack = 1f, int Turn = 1)
        {
            switch(Type)
            {
                case EStatusEffectType.Burn:
                    {
                        return new BurnEffect(OpType, Stack,Turn);
                    }
                case EStatusEffectType.Poison:
                    {
                        return new PoisonEffect(OpType,Stack, Turn);
                    }
                case EStatusEffectType.AttackBuff:
                    {
                        return new AttackBuff(OpType, Stack, Turn);
                    }
                default:
                    {
                        return null;
                    }
            }
        }
    }
}