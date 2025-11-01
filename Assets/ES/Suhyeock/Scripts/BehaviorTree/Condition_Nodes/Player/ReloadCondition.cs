using UnityEngine;

public class ReloadCondition : BTNode
{
    PlayerBlackboard blackboard;
    public ReloadCondition(PlayerBlackboard blackboard)
    {
        this.blackboard = blackboard;
    }

    public override NodeState Evaluate()
    {
        if (blackboard.gun.magAmmo == blackboard.gun.weapon.magCapacity)
        {
            blackboard.isReloadButtonPressed = false;
            return NodeState.Failure;
        }

        if (blackboard.gun.state == GunState.RELOADING)
        {
            return NodeState.Running;
        }
        if (blackboard.gun.magAmmo <= 0 || blackboard.isReloadButtonPressed == true)
        {
            return NodeState.Success;
        }

        return NodeState.Failure;
    }

}
