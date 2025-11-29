using LUP.DSG;
using UnityEngine;
using LUP.DSG.Utils.Enums;
using System.Buffers;

namespace LUP.DSG
{
    public class BurnEffect : IStatusEffect
    {
        public BurnEffect(EOperationType opType, float amount, int turns)
            : base(EStatusEffectType.Burn,opType, amount, turns) { }
        public override void Apply(Character C) => Debug.Log("화상 시작");
        public override void Turn(Character C) => C.BattleComp.TakeDamage(1);
        public override void Remove(Character C) => Debug.Log("화상 끝");
    }
}