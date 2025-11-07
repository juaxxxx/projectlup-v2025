using System.Collections.Generic;
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

            Collider[] hitColliders = Physics.OverlapSphere(
                attackPoint,
                blackboard.attackSize,
                blackboard.LayerMask
            );
            HashSet<GameObject> hitTargetsOnce = new HashSet<GameObject>();

            for (int i = 0; i < hitColliders.Length; i++)
            {
                GameObject rootObject = hitColliders[i].gameObject;
                if (hitTargetsOnce.Add(rootObject))
                {
                    HealthComponent health = hitColliders[i].GetComponent<HealthComponent>();

                    if (health)
                    {
                        health.TakeDamage(10.0f);
                        Debug.Log("Attack");
                    }
                }
            }

            return NodeState.Success;
        }

        public override void Reset()
        {

        }
    }
}

    
