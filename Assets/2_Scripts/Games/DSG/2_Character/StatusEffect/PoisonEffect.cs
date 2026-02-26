using LUP.DSG;
using LUP.DSG.Utils.Enums;
using UnityEngine;

namespace LUP.DSG
{
    public class PoisonEffect : StatusEffect
    {
        public PoisonEffect(EOperationType oPType,float amount, int turns)
           : base(EStatusEffectType.Poison,oPType,amount, turns) { }
        public override void Apply(Character C) => Debug.Log("독 시작");
        public override void Turn(Character C) => C.BattleComp.TakeDamage(amount * 2 * 2,ActionEffect.GetHit_Poison);
        public override void Remove(Character C) => Debug.Log("독 끝");
        public override void AttachEffect(Character C)
        {
            effect = C.ActioneffectPool.PlayVFXAttached(ActionEffect.Aura_Poison, C.transform, new Vector3(0, 0, 0), Quaternion.identity, true);
        }
    }
}