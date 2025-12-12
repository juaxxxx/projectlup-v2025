using UnityEngine;

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
        public LineupSlot SelectEnemyTarget(Character Attacker);
    }
}