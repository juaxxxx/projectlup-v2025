using UnityEngine;

namespace LUP.ES
{ 
    public class TooFarFromHomeCondition : BTNode
    {
        EnemyBlackboard blackboard;
        
        public TooFarFromHomeCondition(EnemyBlackboard blackboard)
        {
            this.blackboard = blackboard;
        }

        public override NodeState Evaluate()
        {
            float distanceToHome = Vector3.Distance(blackboard.navMeshAgent.transform.position, blackboard.initialPosition);
            if (distanceToHome > blackboard.maxRange)
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
