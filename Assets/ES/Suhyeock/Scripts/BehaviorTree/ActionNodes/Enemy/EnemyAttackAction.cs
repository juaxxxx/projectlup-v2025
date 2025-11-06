using UnityEngine;

namespace ES
{
    public class EnemyAttackAction : BTNode
    {
        EnemyBlackboard blackboard;
        public EnemyAttackAction(EnemyBlackboard blackboard)
        {
            this.blackboard = blackboard;
        }

        public override NodeState Evaluate()
        {
            blackboard.navMeshAgent.ResetPath();

            Vector3 enemyPosition = blackboard.transform.position;
            Vector3 enemyForward = blackboard.transform.forward;

            Quaternion enemyQuaternion = blackboard.transform.rotation;

            Vector3 attackPoint = enemyPosition + (enemyForward * blackboard.attackSize * 0.7f);

            Collider[] hitTargets = Physics.OverlapSphere(
                attackPoint,
                blackboard.attackSize,
                blackboard.LayerMask
            );

            for (int i = 0; i < hitTargets.Length; i++)
            {
                HealthComponent health = hitTargets[i].GetComponent<HealthComponent>();

                if (health)
                {
                    health.TakeDamage(10.0f);
                    Debug.Log("Attack");
                }
            }

            return NodeState.Success;
        }

        public override void Reset()
        {

        }
    }
}

    
