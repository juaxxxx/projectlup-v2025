using System.Collections.Generic;
using UnityEngine;


namespace LUP.ES
{ 
    public class RangedEnemyBehaviorTree : MonoBehaviour
    {
        private BTNode rootNode;
        private RangedEnemyBlackboard blackboard;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            blackboard = GetComponent<RangedEnemyBlackboard>();
            SetupBehaviorTree();
        }

        // Update is called once per frame
        void Update()
        {
            if(blackboard.isDetected == true && blackboard.healthComponent.isDead == false)
            {
                TurnToPlayer();
            }
            rootNode.Evaluate();
        }

        

        private void SetupBehaviorTree()
        {

            DeadCondition deadCondition = new DeadCondition(blackboard);
            EnemyDeathAction deathAction = new EnemyDeathAction(blackboard);
            Sequence handleDeathSequence = new Sequence(new List<BTNode> { deadCondition, deathAction });

            TooFarFromHomeCondition tooFarFromHomeCondition = new TooFarFromHomeCondition(blackboard);
            ReturnToInitialPositionAction returnToInitialPositionAction = new ReturnToInitialPositionAction(blackboard);
            Sequence OutOfRangeReturnSequence = new Sequence(new List<BTNode> { tooFarFromHomeCondition, returnToInitialPositionAction });

            TargetInAttackRangeCondition targetInAttackRangeCondition = new TargetInAttackRangeCondition(blackboard);


            RangedEnemyAttackAction rangedEnemyAttackAction = new RangedEnemyAttackAction(blackboard, 5);
            Sequence attackSequence = new Sequence(new List<BTNode> { rangedEnemyAttackAction, new WaitAction(2.0f) });
            Sequence handleAttackSequence = new Sequence(new List<BTNode> { targetInAttackRangeCondition, attackSequence });

            TargetInDetectionRangeCondition targetInDetectionRangeCondition = new TargetInDetectionRangeCondition(blackboard);
            ChaseTargetAction chaseTargetAction = new ChaseTargetAction(blackboard);
            Sequence handleMoveSequence = new Sequence(new List<BTNode> { targetInDetectionRangeCondition, chaseTargetAction });

            FindRandomLocationAction findRandomLocationAction = new FindRandomLocationAction(blackboard);
            MoveToTargetAction moveToTargetAction = new MoveToTargetAction(blackboard);
            Sequence patrolSequence = new Sequence(new List<BTNode> { findRandomLocationAction, moveToTargetAction, new WaitAction(3.0f) });


            rootNode = new Selector(new List<BTNode>
            {
                handleDeathSequence,
                OutOfRangeReturnSequence,
                handleAttackSequence,
                handleMoveSequence,
                patrolSequence,
            });
        }

        void TurnToPlayer()
        {
            Transform enemyTransform = blackboard.navMeshAgent.transform;
            Vector3 targetPosition = blackboard.playerTransform.position;

            Vector3 direction = (targetPosition - enemyTransform.position);
            direction.y = 0;
            direction.Normalize();

            float angleToTarget = Vector3.Angle(enemyTransform.forward, direction);

            Quaternion targetRotation = Quaternion.LookRotation(direction);

            enemyTransform.rotation = Quaternion.RotateTowards(enemyTransform.rotation, targetRotation, 700.0f * Time.deltaTime);
        }
    }
}

