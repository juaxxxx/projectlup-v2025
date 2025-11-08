using LUP.DSG;
using UnityEngine;
using LUP.DSG.Utils.Enums;

namespace LUP.DSG
{
    public class StatusEffectFactory
    {
        public IStatusEffect CreateStatusEffect(EStatusEffectType Type,float Stack, int Turn)
        {
            switch(Type)
            {
                case EStatusEffectType.Burn:
                    {
                        return new BurnEffect(Stack,Turn);
                    }
                case EStatusEffectType.Poison:
                    {
                        return new PoisonEffect(Stack,Turn);
                    }
                default:
                    {
                        return null;
                    }
            }
        }
        public IStatusEffect CreateBuffORDeBuff(EStatusEffectType Type, EOperationType OpType,
            float Amount,int Turn)
        {
            switch(Type)
            {
                case EStatusEffectType.AttackBuff:
                    {
                        return new AttackBuff(OpType,Amount,Turn);
                    }
                default:
                    {
                        return null;
                    }
            }
        }
    }
}