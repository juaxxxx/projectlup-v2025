using UnityEngine;

namespace LUP.DSG
{
    public interface IAttackTargetSelector
    {
        public LineupSlot SelectTarget(Character Attacker);
    }
}