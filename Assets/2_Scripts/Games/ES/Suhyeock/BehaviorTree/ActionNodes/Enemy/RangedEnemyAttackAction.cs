using UnityEngine;

namespace LUP.ES
{
    public class RangedEnemyAttackAction : BTNode
    {
        RangedEnemyBlackboard blackboard;
        int shotsPerBurst = 5;
        int totalShotsFired = 0;

        private float aimDelay = 1f;
        private float currentAimTime = 0f;

        public RangedEnemyAttackAction(RangedEnemyBlackboard blackboard, int shotsPerBurst)
        {
            this.blackboard = blackboard;
            this.shotsPerBurst = shotsPerBurst;
        }

        public override NodeState Evaluate()
        {
            blackboard.ChangeState(EnemyState.Attack);

            if (totalShotsFired == 0)
            {
                if (currentAimTime < aimDelay)
                {
                    currentAimTime += Time.deltaTime;
                    return NodeState.Running;
                }
            }

            if (blackboard.gun.Fire())
            {
                totalShotsFired++;
            }
            
            if(totalShotsFired >= shotsPerBurst)
            {
                blackboard.ChangeState(EnemyState.Idle);
                totalShotsFired = 0;
                currentAimTime = 0f;
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

