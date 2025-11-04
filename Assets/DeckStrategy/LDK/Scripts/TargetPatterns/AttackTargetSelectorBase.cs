using UnityEngine;
using System.Collections.Generic;

public abstract class AttackTargetSelectorBase : IAttackTargetSelector
{
    protected readonly BattleSystem battle;
    protected AttackTargetSelectorBase(BattleSystem battlesystem) => battle = battlesystem;
    public abstract LineupSlot SelectTarget(Character attacker);

    protected List<LineupSlot> GetAliveTargetList(Character character)
    {
        if (character == null && battle == null)
            return null;

        List<LineupSlot> slots = new List<LineupSlot>();

        if (character.isEnemy)
        {
            int length = battle.friendlySlots.Length;

            for (int i = 0; i < length; i++)
            {
                LineupSlot slot = battle.friendlySlots[i].GetComponent<LineupSlot>();
                if (IsAlive(slot.character))
                {
                    slots.Add(slot);
                }
            }
        }
        else
        {
            int length = battle.enemySlots.Length;

            for (int i = 0; i < length; i++)
            {
                LineupSlot slot = battle.enemySlots[i].GetComponent<LineupSlot>();
                if (IsAlive(slot.character))
                {
                    slots.Add(slot);
                }
            }
        }

        return slots;
    }
    protected bool IsAlive(Character character)
    {
        return character != null &&
               character.BattleComp != null &&
               character.BattleComp.isAlive;
    }
}
