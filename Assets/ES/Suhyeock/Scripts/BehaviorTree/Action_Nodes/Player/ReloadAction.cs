using System.Collections;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class ReloadAction : BTNode
{
    PlayerBlackboard blackboard;

    public ReloadAction(PlayerBlackboard blackboard)
    {
        this.blackboard = blackboard;
    }

    public override NodeState Evaluate()
    {
        if (blackboard.gun.state == GunState.RELOADING)
        {
            return NodeState.Running;
        }
        blackboard.gun.Reload();
        blackboard.isReloadButtonPressed = false;
        return NodeState.Running;
    }
}
