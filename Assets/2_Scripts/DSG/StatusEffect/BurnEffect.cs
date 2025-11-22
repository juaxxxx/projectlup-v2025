using LUP.DSG;
using UnityEngine;
using LUP.DSG.Utils.Enums;

namespace LUP.DSG
{
    public class BurnEffect : IStatusEffect
    {
        public BurnEffect(float amount = 1f, int turns = 1)
            : base(EStatusEffectType.Burn, amount, turns) { }
        public override void Apply(Character C) => Debug.Log("화상 시작");
        public override void Turn(Character C) => C.BattleComp.TakeDamage(1);
        public override void Remove(Character C) => Debug.Log("화상 끝");
    }
}