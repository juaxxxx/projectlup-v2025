using LUP.DSG.Utils.Enums;
using UnityEngine;

namespace LUP.DSG
{
    public class AttackBuff : StatusEffect
    {
        public EOperationType operationType;
        private ActionEffect buffdebuffEffect;
        private float playerAttack;
        public AttackBuff(EOperationType Type, float Amount, int Turns)
            : base(EStatusEffectType.AttackBuff,Type, Amount, Turns)
        {
            operationType = Type;
        }
        public override void Apply(Character C)
        {
            playerAttack = C.characterData.attack;
            float result = 0;
            Operation.TryEval(operationType, playerAttack, amount,out result);
            C.characterData.attack = result;
            
            if(operationType == EOperationType.Minus)
            {
                buffdebuffEffect = ActionEffect.Aura_AttackBuff;
            }
            else if(operationType == EOperationType.Plus)
            {
                buffdebuffEffect = ActionEffect.Aura_AttackDebuff;
            }

            C.ActioneffectPool.PlayVFXAttached(buffdebuffEffect,C.transform,new Vector3(0,0,0),Quaternion.identity,true);
        }
        public override void Turn(Character C) {  }
        public override void Remove(Character C)
        {
            C.characterData.attack = playerAttack;
        }
    }
}