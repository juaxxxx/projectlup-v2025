using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class PickRandomTarget : AttackTargetSelectorBase
{
    public PickRandomTarget(BattleSystem battle) : base(battle) { }
    public override LineupSlot SelectTarget(Character Attacker)
    {
        if (battle == null || Attacker == null)
            return null;

        List<LineupSlot> alive = GetAliveTargetList(Attacker);
        if (alive.Count <= 0)
            return null;

        int Target = UnityEngine.Random.Range(0, alive.Count);
        return alive[Target].GetComponent<LineupSlot>();
    }
}
