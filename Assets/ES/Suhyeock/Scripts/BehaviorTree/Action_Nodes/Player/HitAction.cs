using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class HitAction : BTNode
{
    PlayerBlackboard Blackboard;

    public HitAction(PlayerBlackboard blackboard)
    {
        this.Blackboard = blackboard;
    }

    public override NodeState Evaluate()
    {
        Blackboard.isHit = false;
        Blackboard.eventBroker.CloseLootDisplay();
        Blackboard.eventBroker.HandleIventoryVisibility(false);
        Blackboard.playerOverheadUI.UpdateHPUI();
        return NodeState.Success;
    }
}
