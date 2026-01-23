using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using LUP.DSG.Utils.Enums;
using static LUP.DSG.Character;

namespace LUP.DSG
{
    public class StatusEffectComponent : MonoBehaviour
    {
        Character owner;

        public Action<StatusEffect> OnEffectAdded;
        public Action<StatusEffect> OnEffectEndTurn;
        public Action<StatusEffect> OnEffectRemoved;

        private readonly Dictionary<EStatusEffectType, StatusEffect> _effects = new();
        private readonly List<EStatusEffectType> _effectsRemoveList = new();

        private readonly StatusEffectFactory StatusEffectfactory = new StatusEffectFactory();

        private void Start()
        {
            owner = GetComponent<Character>();
        }
        public StatusEffect CreateStatusEffect(EStatusEffectType Type, EOperationType OpType,
            float Stack, int Turn)
        {
            return StatusEffectfactory.CreateStatusEffect(Type, OpType, Stack, Turn);
        }
        public void AddEffect(StatusEffect effect)
        {
            if (!owner.BattleComp.isAlive)
                return;

            effect.Apply(owner);

            if (_effects.TryGetValue(effect.effectType, out StatusEffect getEffect))
            {
                getEffect.amount += effect.amount;  // 내부 값 수정
                _effects[effect.effectType].amount = getEffect.amount;  // 다시 저장 이거 괜찮나

                int Turn = Math.Max(getEffect.remainingTurns, effect.remainingTurns);
                _effects[effect.effectType].remainingTurns = Turn;
            }
            else
            {
                _effects.Add(effect.effectType, effect);
                effect.AttachEffect(owner);
            }

            OnEffectAdded?.Invoke(_effects[effect.effectType]);
        }
        public void TurnAll()
        {
            foreach (StatusEffect effect in _effects.Values)
            {
                effect.Turn(owner);

                if (!owner.BattleComp.isAlive) break; 


                effect.remainingTurns--;
                OnEffectEndTurn?.Invoke(effect);

                if (effect.remainingTurns <= 0)
                {
                    _effectsRemoveList.Add(effect.effectType);
                    continue;
                }
            }
        }
        public void ClearRemoveList()
        {
            foreach (var key in _effectsRemoveList)
            {
                var e = _effects[key];
                RemoveEffect(e);
            }
            _effectsRemoveList.Clear();
        }
        public void RemoveEffect(StatusEffect effect)
        {
            _effects.Remove(effect.effectType);
            effect.Remove(owner);
            OnEffectRemoved?.Invoke(effect);
        }

        public void HandleOwnerDie(int battleindex)
        {
            foreach (var effect in _effects.Values)
            {
                _effectsRemoveList.Add(effect.effectType);
                OnEffectRemoved?.Invoke(effect);
            }
        }

        void OnDisable()
        {
            OnEffectAdded = null; // 모든 구독자 제거
            OnEffectRemoved = null;
        }
    }
}