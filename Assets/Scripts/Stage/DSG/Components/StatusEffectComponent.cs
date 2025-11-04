using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using static DSG.Character;

namespace DSG
{
    public interface IStatusEffect
    {
        void Apply(Character target);
        void Turn(Character target);
        void Remove(Character target);
    }

    public class BurnEffect : IStatusEffect
    {
        public void Apply(Character C) => Debug.Log("화상 시작");
        public void Turn(Character C) => C.BattleComp.TakeDamage(1);
        public void Remove(Character C) => Debug.Log("화상 끝");
    }

    public class PoisonEffect : IStatusEffect
    {
        public void Apply(Character C) => Debug.Log("독 시작");
        public void Turn(Character C) => C.BattleComp.TakeDamage(1);
        public void Remove(Character C) => Debug.Log("독 끝");
    }
    public class StatusEffectComponent : MonoBehaviour
    {
        Character owner;

        public Action<StatusEffect> OnEffectAdded;
        public Action<StatusEffect> OnEffectRemoved;

        private readonly Dictionary<string, StatusEffect> _effects = new();

        private void Start()
        {
            owner = GetComponent<Character>();
        }
        public void AddEffect(StatusEffect effect)
        {
            if (!owner.BattleComp.isAlive)
                return;

            if (_effects.TryGetValue(effect.Name, out StatusEffect getEffect))
            {
                getEffect.Stack += effect.Stack;  // 내부 값 수정
                _effects[effect.Name] = getEffect;  // 다시 저장
            }
            else
            {
                _effects.Add(effect.Name, effect);
            }

            effect.statusEffect.Apply(owner);
            OnEffectAdded?.Invoke(_effects[effect.Name]);
        }
        public void TurnAll()
        {
            foreach (StatusEffect effect in _effects.Values) effect.statusEffect.Turn(owner);
        }
        public void RemoveEffect(StatusEffect effect)
        {
            _effects.Remove(effect.Name);
            effect.statusEffect.Remove(owner);
            OnEffectRemoved?.Invoke(effect);
        }

        void OnDisable()
        {
            OnEffectAdded = null; // 모든 구독자 제거
            OnEffectRemoved = null;
        }
    }
}