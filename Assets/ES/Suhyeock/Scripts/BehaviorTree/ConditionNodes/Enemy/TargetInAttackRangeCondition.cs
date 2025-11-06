using UnityEngine;

namespace ES
{
    public class TargetInAttackRangeCondition : BTNode
    {
        EnemyBlackboard blackboard;
        
        public TargetInAttackRangeCondition(EnemyBlackboard blackboard)
        {
            this.blackboard = blackboard;
        }

        public override NodeState Evaluate()
        {
            Vector3 enemyPos = blackboard.transform.position;
            Vector3 targetPos = blackboard.playerTransform.position;

            float distance = Vector3.Distance(enemyPos, targetPos);
            
            if(distance <= blackboard.attackRange)
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
