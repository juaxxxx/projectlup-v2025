using System.Collections.Generic;
using UnityEngine;

namespace LUP.DSG
{
    public class PickHighestHpTarget : AttackTargetSelectorBase
    {
        public PickHighestHpTarget(BattleSystem battle) : base(battle) { }
        public override LineupSlot SelectEnemyTarget(Character Attacker)
        {
            List<LineupSlot> Slot = GetAliveTargetList(Attacker);
            if (Slot.Count <= 0)
                return null;

            Slot.Sort((x, y) => y.character.BattleComp.currHp.CompareTo(x.character.BattleComp.currHp));

            return Slot[0];
        }
    }
}