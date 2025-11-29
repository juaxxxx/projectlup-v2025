using UnityEngine;


namespace LUP.ES
{
    public class TurnToPlayerAction : BTNode
    {
        EnemyBlackboard blackboard;
        private const float ACCEPTABLE_ANGLE = 5.0f;
        private const float TURN_SPEED = 500.0F;
        public TurnToPlayerAction(EnemyBlackboard blackboard)
        {
            this.blackboard = blackboard;
        }

        public override NodeState Evaluate()
        {
            Transform enemyTransform = blackboard.navMeshAgent.transform;
            Vector3 targetPosition = blackboard.playerTransform.position;

            Vector3 direction = (targetPosition - enemyTransform.position);
            direction.y = 0;
            direction.Normalize();

            float angleToTarget = Vector3.Angle(enemyTransform.forward, direction);

            if (angleToTarget <= ACCEPTABLE_ANGLE)
            {
                return NodeState.Success;
            }

            Quaternion targetRotation = Quaternion.LookRotation(direction);

            enemyTransform.rotation = Quaternion.RotateTowards(enemyTransform.rotation, targetRotation, TURN_SPEED * Time.deltaTime);

            return NodeState.Running;
        }

        public override void Reset()
        {
            
        }
    }

}
