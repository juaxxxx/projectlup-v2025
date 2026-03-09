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
        public static float Calculator(DamageContext context, out bool isWeak)
        {
            float result = context.attack;

            isWeak = IsWeakness(context.Type, context.enemyType);
            if (isWeak)
            {
                result *= 1.5f;
            }

            result = result - context.enemyDefence;

            result = Mathf.Clamp(result, 0, result);
            return result;
        }

        private static bool IsWeakness(EAttributeType my, EAttributeType target)
        {
            switch (my)
            {
                case EAttributeType.FIRE:
                    if (target == EAttributeType.NATURE)
                        return true;
                    break;
                case EAttributeType.NATURE:
                    if (target == EAttributeType.WATER)
                        return true;
                    break;
                case EAttributeType.WATER:
                    if (target == EAttributeType.FIRE)
                        return true;
                    break;
            }
            return false;
        }
    }
}