using UnityEngine;

public interface IAttackTargetSelector
{
    public LineupSlot SelectTarget(Character Attacker);
}
