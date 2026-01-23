using LUP.DSG;
using UnityEngine;
using LUP.DSG.Utils.Enums;
using System.Buffers;

namespace LUP.DSG
{
    public class BurnEffect : StatusEffect
    {
        public BurnEffect(EOperationType opType, float amount, int turns)
            : base(EStatusEffectType.Burn,opType, amount, turns) { }
        public override void Apply(Character C)
        {
            Debug.Log("화상 시작");
        }
        public override void Turn(Character C) => C.BattleComp.TakeDamage(amount * 5, ActionEffect.GetHit_Burn);
        public override void Remove(Character C)
        {
            Debug.Log("화상 끝");
            C.ActioneffectPool.StopLoopVFX(effect.particlePrefab, effect.name);
        }
        public override void AttachEffect(Character C)
        {
            effect = C.ActioneffectPool.PlayVFXAttached(ActionEffect.Aura_Burn, C.transform, new Vector3(0, 0, 0), Quaternion.identity, true);
        }
    }
}