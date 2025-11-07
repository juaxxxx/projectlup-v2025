using UnityEngine;

namespace ES
{
    public class HitAction : BTNode
    {
        PlayerBlackboard Blackboard;

        public HitAction(PlayerBlackboard blackboard)
        {
            this.Blackboard = blackboard;
        }

        public override NodeState Evaluate()
        {
            Blackboard.healthComponent.isHit = false;
            Blackboard.eventBroker.CloseLootDisplay();
            Blackboard.eventBroker.HandleIventoryVisibility(false);
            Blackboard.playerOverheadUI.UpdateHPUI();
            return NodeState.Success;
        }

        public override void Reset()
        {

        }
    }
}
