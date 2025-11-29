using UnityEngine;

namespace LUP.ES
{
    public class RangedEnemyAttackAction : BTNode
    {
        RangedEnemyBlackboard blackboard;
        private const float TURN_SPEED = 500.0F;
        int shotsPerBurst = 5;
        int totalShotsFired = 0;

        public RangedEnemyAttackAction(RangedEnemyBlackboard blackboard, int shotsPerBurst)
        {
            this.blackboard = blackboard;
            this.shotsPerBurst = shotsPerBurst;
        }

        public override NodeState Evaluate()
        {
            //Transform enemyTransform = blackboard.navMeshAgent.transform;
            //Vector3 targetPosition = blackboard.playerTransform.position;

            //Vector3 direction = (targetPosition - enemyTransform.position);
            //direction.y = 0;
            //direction.Normalize();

            //Quaternion targetRotation = Quaternion.LookRotation(direction);

            //enemyTransform.rotation = Quaternion.RotateTowards(enemyTransform.rotation, targetRotation, TURN_SPEED * Time.deltaTime);


            if (blackboard.gun.Fire())
            {
                totalShotsFired++;
            }
            
            if(totalShotsFired >= shotsPerBurst)
            {
                totalShotsFired = 0;
                return NodeState.Success;
            }

            return NodeState.Running;
        }

        public override void Reset()
        {
            totalShotsFired = 0;
        }
    }
}

