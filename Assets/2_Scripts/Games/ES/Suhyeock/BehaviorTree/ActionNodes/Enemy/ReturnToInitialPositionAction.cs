using UnityEngine;

namespace LUP.ES
{
    public class ReturnToInitialPositionAction : BTNode
    {
        EnemyBlackboard blackboard;
        private const float REACHED_DISTANCE = 0.5f;
        public ReturnToInitialPositionAction(EnemyBlackboard blackboard)
        {
            this.blackboard = blackboard;
        }

        public override NodeState Evaluate()
        {
            blackboard.navMeshAgent.SetDestination(blackboard.initialPosition);

            if (blackboard.navMeshAgent.remainingDistance <= REACHED_DISTANCE && !blackboard.navMeshAgent.pathPending)
            {
                return NodeState.Success;
            }

            return NodeState.Running;
        }

        public override void Reset()
        {
            
        }
    }
}

