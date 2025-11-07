using DSG.Utils.Enums;
using UnityEngine;

namespace DSG
{
    public class AttackBuff : IStatusEffect
    {
        public EOperationType operationType;
        public AttackBuff(EOperationType Type, float Amount = 1f, int Turns = 1)
            : base(EStatusEffectType.AttackBuff, Amount, Turns)
        {
            operationType = Type;
        }
        public override void Apply(Character C)
        {
            float playerAttack = C.BattleComp.attack;
            float result = 0;
            Operation.TryEval(operationType, playerAttack, amount,out result);
            C.BattleComp.attack = result;
        }
        public override void Turn(Character C) { }
        public override void Remove(Character C)
        {
            C.characterData.attack -= amount;
        }
    }
}