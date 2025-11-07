using UnityEngine;

namespace ES
{
    public class MoveToTargetAction : BTNode
    {
        private const float REACHED_DISTANCE = 0.5f;
        EnemyBlackboard blackboard;

        public MoveToTargetAction(EnemyBlackboard blackboard)
        {
            this.blackboard = blackboard;
        }

        public override NodeState Evaluate()
        {
            blackboard.navMeshAgent.SetDestination(blackboard.targetMovePosition);

            if (blackboard.navMeshAgent.remainingDistance <= REACHED_DISTANCE && !blackboard.navMeshAgent.pathPending)
            {
                return NodeState.Success;
            }

            return NodeState.Running;
        }

        public override void Reset()
        {
            blackboard.navMeshAgent.ResetPath();
        }
    }
}


