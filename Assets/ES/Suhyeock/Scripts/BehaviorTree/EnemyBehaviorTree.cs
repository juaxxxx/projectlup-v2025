using System.Collections.Generic;
using UnityEngine;

namespace ES
{
    public class EnemyBehaviorTree : MonoBehaviour
    {
        private BTNode rootNode;
        private EnemyBlackboard blackboard;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            blackboard = GetComponent<EnemyBlackboard>();
            SetupBehaviorTree();
        }

        // Update is called once per frame
        void Update()
        {
            rootNode.Evaluate();
        }

        private void SetupBehaviorTree()
        {

            DeadCondition deadCondition = new DeadCondition(blackboard);
            EnemyDeathAction deathAction = new EnemyDeathAction(blackboard);
            Sequence handleDeathSequence = new Sequence(new List<BTNode> { deadCondition, deathAction });

            TargetInAttackRangeCondition targetInAttackRangeCondition = new TargetInAttackRangeCondition(blackboard);
            EnemyAttackAction enemyAttackAction = new EnemyAttackAction(blackboard);
            Sequence attackSequence = new Sequence(new List<BTNode> { enemyAttackAction, new WaitAction(2.0f) });
            Sequence handleAttackSequence = new Sequence(new List<BTNode> { targetInAttackRangeCondition , attackSequence });

            TargetInDetectionRangeCondition targetInDetectionRangeCondition = new TargetInDetectionRangeCondition(blackboard);
            ChaseTargetAction chaseTargetAction = new ChaseTargetAction(blackboard);
            Sequence handleMoveSequence = new Sequence(new List<BTNode> { targetInDetectionRangeCondition, chaseTargetAction });

            rootNode = new Selector(new List<BTNode> 
            { 
                handleDeathSequence,
                handleAttackSequence,
                handleMoveSequence,
            });
        }

        private void OnDrawGizmos()
        {
            if (Application.isPlaying && blackboard != null && blackboard.transform != null)
            {
                Vector3 attackPoint = blackboard.transform.position + (blackboard.transform.forward * blackboard.attackSize * 0.7f);
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(attackPoint, blackboard.attackSize);
            }
        }
    }
}
