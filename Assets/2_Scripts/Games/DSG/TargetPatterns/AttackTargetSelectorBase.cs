using UnityEngine;
using System.Collections.Generic;

namespace LUP.DSG
{
    public abstract class AttackTargetSelectorBase : IAttackTargetSelector
    {
        protected readonly BattleSystem battle;
        protected AttackTargetSelectorBase(BattleSystem battlesystem) => battle = battlesystem;
        public abstract List<LineupSlot> SelectEnemyTargets(Character Attacker, int count);
        public virtual TargetPatternType PatternType => TargetPatternType.None;


        protected List<LineupSlot> GetAliveTargetList(Character character)
        {
            if (character == null && battle == null)
                return null;

            List<LineupSlot> slots = new List<LineupSlot>();
            GameObject[] otherSlots = character.isEnemy ? battle.friendlySlots : battle.enemySlots;

            int length = otherSlots.Length;

            for (int i = 0; i < length; i++)
            {
                LineupSlot slot = otherSlots[i].GetComponent<LineupSlot>();
                if (IsAlive(slot.character))
                {
                    slots.Add(slot);
                }
            }

            return slots;
        }
        protected bool IsAlive(Character character)
        {
            if (character == null) return false;

            return character.characterData != null &&
                   character.BattleComp.isAlive;
        }
    }
}