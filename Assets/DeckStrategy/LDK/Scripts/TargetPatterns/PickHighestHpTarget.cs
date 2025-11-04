using System.Collections.Generic;
using UnityEngine;

public class PickHighestHpTarget : AttackTargetSelectorBase
{
    public PickHighestHpTarget(BattleSystem battle) : base(battle) { }
    public override LineupSlot SelectTarget(Character Attacker)
    {
        List<LineupSlot> Slot = GetAliveTargetList(Attacker);
        if (Slot.Count <= 0)
            return null;

        Slot.Sort((x, y) => y.character.BattleComp.currHp.CompareTo(x.character.BattleComp.currHp));

        return Slot[0];
    }
}
