using UnityEngine;

namespace ES
{
    public class HitCondition : BTNode
    {
        PlayerBlackboard blackboard;

        public HitCondition(PlayerBlackboard blackboard)
        {
            this.blackboard = blackboard;
        }

        public override NodeState Evaluate()
        {
            if (blackboard.healthComponent.isHit == true)
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
