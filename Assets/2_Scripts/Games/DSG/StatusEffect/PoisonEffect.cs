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
        public override void Turn(Character C) => C.BattleComp.TakeDamage(1,ActionEffect.GetHit_Poison);
        public override void Remove(Character C) => Debug.Log("독 끝");
    }
}