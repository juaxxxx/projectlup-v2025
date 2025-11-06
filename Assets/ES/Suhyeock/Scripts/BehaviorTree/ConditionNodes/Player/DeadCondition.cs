using UnityEngine;

namespace ES
{
    public class DeadCondition : BTNode
    {
        BaseBlackboard blackboard;

        public DeadCondition(BaseBlackboard blackboard)
        {
            this.blackboard = blackboard;
        }

        public override NodeState Evaluate()
        {
            if(blackboard.healthComponent.isDead == true)
            {
                return NodeState.Success;
            } 
            return NodeState.Failure;
        }

        public override void Reset()
        {

        }
    }
}
