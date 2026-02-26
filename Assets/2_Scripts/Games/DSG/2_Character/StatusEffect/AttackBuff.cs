using LUP.DSG.Utils.Enums;
using UnityEngine;

namespace LUP.DSG
{
    public class AttackBuff : StatusEffect
    {
        private ActionEffect buffdebuffEffect;
        private float playerAttack;
        public AttackBuff(EOperationType Type, float Amount, int Turns)
            : base(EStatusEffectType.AttackBuff,Type, Amount, Turns) { }
        public override void Apply(Character C)
        {
            playerAttack = C.characterData.attack;
            float result = 0;
            Operation.TryEval(opType, playerAttack, amount,out result);
            C.characterData.attack = result;
            
            if(opType == EOperationType.Minus)
            {
                buffdebuffEffect = ActionEffect.Aura_AttackBuff;
            }
            else if(opType == EOperationType.Plus)
            {
                buffdebuffEffect = ActionEffect.Aura_AttackDebuff;
            }
        }
        public override void Turn(Character C) {  }
        public override void Remove(Character C)
        {
            C.characterData.attack = playerAttack;
        }

        public override void AttachEffect(Character C)
        {
            C.ActioneffectPool.PlayVFXAttached(buffdebuffEffect, C.transform, new Vector3(0, 0, 0), Quaternion.identity, true);
        }
    }
}