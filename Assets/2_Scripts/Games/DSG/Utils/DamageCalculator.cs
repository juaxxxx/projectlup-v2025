using UnityEngine;
using LUP.DSG.Utils.Enums;

namespace LUP.DSG.Utils
{
    public struct DamageContext
    {
        public float attack;
        public float enemyDefence;
        public EAttributeType Type;
        public EAttributeType enemyType;
    }
    public static class DamageCalculator
    {
        public static float Calculator(DamageContext context)
        {
            float result = context.attack;

            if(IsWeakness(context.Type,context.enemyType))
            {
                result *= 1.5f;
            }

            result = result - context.enemyDefence;

            Mathf.Clamp(result, 0, result);
            return result;
        }

        private static bool IsWeakness(EAttributeType my, EAttributeType target)
        {
            switch (my)
            {
                case EAttributeType.PAPER:
                    if (target == EAttributeType.ROCK)
                        return true;
                    break;
                case EAttributeType.ROCK:
                    if (target == EAttributeType.SCISSORS)
                        return true;
                    break;
                case EAttributeType.SCISSORS:
                    if (target == EAttributeType.PAPER)
                        return true;
                    break;
            }
            return false;
        }
    }
}