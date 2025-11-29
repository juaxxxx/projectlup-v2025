using System.Collections.Generic;
using UnityEngine;

namespace LUP.ES
{
    public class MeleeEnemyBehaviorTree : MonoBehaviour
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
            // 1. 죽음
            DeadCondition deadCondition = new DeadCondition(blackboard);
            EnemyDeathAction deathAction = new EnemyDeathAction(blackboard);
            Sequence handleDeathSequence = new Sequence(new List<BTNode> { deadCondition, deathAction });

            // 2. 최대 이동거리 확인
            TooFarFromHomeCondition tooFarFromHomeCondition = new TooFarFromHomeCondition(blackboard);
            ReturnToInitialPositionAction returnToInitialPositionAction = new ReturnToInitialPositionAction(blackboard);
            Sequence OutOfRangeReturnSequence = new Sequence(new List<BTNode> { tooFarFromHomeCondition, returnToInitialPositionAction });

            // 3. 공격
            TargetInAttackRangeCondition targetInAttackRangeCondition = new TargetInAttackRangeCondition(blackboard);
            TurnToPlayerAction turnToPlayerAction = new TurnToPlayerAction(blackboard);
            EnemyAttackAction enemyAttackAction = new EnemyAttackAction(blackboard);
            Sequence attackSequence = new Sequence(new List<BTNode> { turnToPlayerAction, enemyAttackAction, new WaitAction(2.0f) });
            Sequence handleAttackSequence = new Sequence(new List<BTNode> { targetInAttackRangeCondition, attackSequence });

            // 4. 추격
            TargetInDetectionRangeCondition targetInDetectionRangeCondition = new TargetInDetectionRangeCondition(blackboard);
            ChaseTargetAction chaseTargetAction = new ChaseTargetAction(blackboard);
            Sequence handleMoveSequence = new Sequence(new List<BTNode> { targetInDetectionRangeCondition, chaseTargetAction });


            // 5. 패트롤
            FindRandomLocationAction findRandomLocationAction = new FindRandomLocationAction(blackboard);
            MoveToTargetAction moveToTargetAction = new MoveToTargetAction(blackboard);
            Sequence patrolSequence = new Sequence(new List<BTNode> { findRandomLocationAction, moveToTargetAction, new WaitAction(3.0f)});

            
            rootNode = new Selector(new List<BTNode> 
            { 
                handleDeathSequence,
                OutOfRangeReturnSequence,
                handleAttackSequence,
                handleMoveSequence,
                patrolSequence,
            });
        }

        private void OnDrawGizmos()
        {
            if (Application.isPlaying && blackboard != null && blackboard.transform != null)
            {
                Vector3 attackPoint = blackboard.transform.position + (blackboard.transform.forward * blackboard.attackSize);
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(attackPoint, blackboard.attackSize);

                Gizmos.color = Color.cyan;
                Gizmos.DrawWireSphere(transform.position, blackboard.attackRange);

                Gizmos.color = Color.yellow;
                Gizmos.DrawWireSphere(transform.position, blackboard.detectionRange);

                Gizmos.color = Color.blue;
                Gizmos.DrawWireSphere(blackboard.initialPosition, blackboard.maxRange);
            }
        }
    }
}
