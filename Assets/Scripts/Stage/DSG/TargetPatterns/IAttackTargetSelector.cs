using UnityEngine;
namespace DSG
{
    public interface IAttackTargetSelector
    {
        public LineupSlot SelectTarget(Character Attacker);
    }
}