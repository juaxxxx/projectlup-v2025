using LUP.DSG;
using LUP.DSG.Utils.Enums;
using UnityEngine;

namespace LUP.DSG
{
    public class PoisonEffect : IStatusEffect
    {
        public PoisonEffect(float amount = 1f, int turns = 1)
           : base(EStatusEffectType.Poison, amount, turns) { }
        public override void Apply(Character C) => Debug.Log("독 시작");
        public override void Turn(Character C) => C.BattleComp.TakeDamage(1);
        public override void Remove(Character C) => Debug.Log("독 끝");
    }
}