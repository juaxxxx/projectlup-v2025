using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

namespace LUP.DSG
{
    public class PickRandomTarget : AttackTargetSelectorBase
    {
        public PickRandomTarget(BattleSystem battle) : base(battle) { }

        public override TargetPatternType PatternType => TargetPatternType.Random;
        public override List<LineupSlot> SelectEnemyTargets(Character Attacker, int count)
        {
            if (battle == null || Attacker.characterData == null)
                return null;

            List<LineupSlot> alive = GetAliveTargetList(Attacker);
            List<LineupSlot> targets = new List<LineupSlot>();
            int mincount = Mathf.Min(alive.Count, count);

            for (int i = 0; i < mincount; i++)
            {
                int targetnum = UnityEngine.Random.Range(0, alive.Count);
                targets.Add(alive[targetnum]);
                alive.Remove(alive[targetnum]);
            }

            return targets;
        }
    }
}