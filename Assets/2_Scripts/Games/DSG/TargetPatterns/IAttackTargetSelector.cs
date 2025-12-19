using UnityEngine;
using System.Collections.Generic;

namespace LUP.DSG
{
    public enum TargetPatternType
    {
        None,
        Weak,
        HighestHp,
        Random,
    }
    public interface IAttackTargetSelector
    {
        TargetPatternType PatternType { get; }
        public List<LineupSlot> SelectEnemyTargets(Character Attacker,int count);
    }
}