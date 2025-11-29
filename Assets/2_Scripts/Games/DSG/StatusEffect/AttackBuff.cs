using LUP.DSG.Utils.Enums;
using UnityEngine;

namespace LUP.DSG
{
    public class AttackBuff : IStatusEffect
    {
        public EOperationType operationType;
        public AttackBuff(EOperationType Type, float Amount, int Turns)
            : base(EStatusEffectType.AttackBuff,Type, Amount, Turns)
        {
            operationType = Type;
        }
        public override void Apply(Character C)
        {
            float playerAttack = C.characterData.attack;
            float result = 0;
            Operation.TryEval(operationType, playerAttack, amount,out result);
            C.characterData.attack = result;
        }
        public override void Turn(Character C) { }
        public override void Remove(Character C)
        {
            C.characterData.attack -= amount;
        }
    }
}