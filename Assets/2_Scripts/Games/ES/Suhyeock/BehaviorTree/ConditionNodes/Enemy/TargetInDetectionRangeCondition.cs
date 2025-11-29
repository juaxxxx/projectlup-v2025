using Unity.VisualScripting;
using UnityEngine;

namespace LUP.ES
{
    public class TargetInDetectionRangeCondition : BTNode
    {
        EnemyBlackboard blackboard;

        public TargetInDetectionRangeCondition(EnemyBlackboard blackboard)
        {
            this.blackboard = blackboard;
        }

        public override NodeState Evaluate()
        {
            Vector3 enemyPos = blackboard.transform.position;
            Vector3 targetPos = blackboard.playerTransform.position;

            float distance = Vector3.Distance(enemyPos, targetPos);
            
            if(distance <= blackboard.detectionRange)
            {
                blackboard.isDetected = true;
                return NodeState.Success;
            }
            return NodeState.Failure;
        }

        public override void Reset()
        {
            blackboard.isDetected = false;
        }
    }
}
