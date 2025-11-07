using UnityEngine;

namespace ES
{
    public class DeathAction : BTNode
    {
        PlayerBlackboard blackboard;

        public DeathAction(PlayerBlackboard blackboard)
        {
            this.blackboard = blackboard;
        }

        public override NodeState Evaluate()
        {
            blackboard.playerOverheadUI.UpdateHPUI();
            blackboard.eventBroker.ReportGameFinish(false);
            return NodeState.Success;
        }

        public override void Reset()
        {
            
        }
    }
}
