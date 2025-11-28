using UnityEngine;

namespace LUP.DSG
{
    public interface IAttackTargetSelector
    {
        public LineupSlot SelectEnemyTarget(Character Attacker);
    }
}