using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace LUP.DSG
{
    public class PickHighestHpTarget : AttackTargetSelectorBase
    {
        public PickHighestHpTarget(BattleSystem battle) : base(battle) { }
        public override TargetPatternType PatternType => TargetPatternType.HighestHp;
        public override List<LineupSlot> SelectEnemyTargets(Character Attacker, int count)
        {
            List<LineupSlot> Alive = GetAliveTargetList(Attacker);
            List<LineupSlot> slots = new List<LineupSlot>();

            if (Alive.Count <= 0)
                return null;

            int mincount = Mathf.Min(Alive.Count, count);

            Alive.Sort((x, y) => y.character.BattleComp.currHp.CompareTo(x.character.BattleComp.currHp));

            for (int i = 0; i < mincount; i++)
            {
                LineupSlot slot = Alive[i];
                slots.Add(slot);
            }

            return slots;
        }
    }
}