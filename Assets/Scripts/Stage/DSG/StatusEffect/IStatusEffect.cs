using LUP.DSG.Utils.Enums;
using UnityEngine;
using static LUP.DSG.Character;

namespace LUP.DSG
{
    public abstract class IStatusEffect
    {
        public EStatusEffectType type { get; private set; }
        public int remainingTurns;
        public float amount;

        public IStatusEffect(EStatusEffectType Type, float Amount = 1f, int Turn = 1)
        {
            type = Type;
            amount = Amount;
            remainingTurns = Turn;
        }
        public virtual void Apply(Character target) { }
        public virtual void Turn(Character target) { }
        public virtual void Remove(Character target) { }
    }
}