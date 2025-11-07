using UnityEngine;
using UnityEngine.AI;

namespace ES
{
    public class FindRandomLocationAction : BTNode
    {
        EnemyBlackboard blackboard;
        private const int MAX_ATTEMPTS = 30;

        public FindRandomLocationAction(EnemyBlackboard blackboard)
        {
            this.blackboard = blackboard;
        }
        public override NodeState Evaluate()
        {
            for (int i = 0; i < MAX_ATTEMPTS; i++)
            {
                Vector3 randomDirection = Random.insideUnitSphere * blackboard.patrolRadius;
                Vector3 randomPostion = blackboard.navMeshAgent.transform.position + randomDirection;

                NavMeshHit hit;
                if (NavMesh.SamplePosition(randomPostion, out hit, blackboard.patrolRadius, NavMesh.AllAreas))
                {
                    blackboard.targetMovePosition = hit.position;
                    return NodeState.Success;
                }
            }
            return NodeState.Failure;
        }

        public override void Reset()
        {
            
        }
    }

}
