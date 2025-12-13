using LUP.ES;
using LUP.ST;
using UnityEngine;

namespace LUP.RL
{
    public class ActionAttack : LeafNode
    {
        bool isAnimOnPlayed = false;
        public ActionAttack(BlackBoard blackBoar, BaseBehaviorTree behaviorTree) : base(blackBoar, behaviorTree)
        {

        }
        public override NodeState Evaluate()
        {
            //UnityEngine.Debug.Log("Action Attack");
            if (isAnimOnPlayed)
            {
                nodeState = NodeState.Running;
                return nodeState;
            }

            isAnimOnPlayed = true;
            nodeState = NodeState.Running;


            TurnToTarget();
            behaviorTree.PlayAnimation(ActionState.Attack, this);
            blackBoard.OnAtk = true;
            blackBoard.InAtkState = true;
            blackBoard.AtkCollTime = blackBoard.AtkCooldownDuration;

            SetNavAgentDeActivate(true);

            return nodeState;
        }

        public override void OnAnimationEnd(AnimatorStateInfo animInfo)
        {
            //UnityEngine.Debug.Log("Hit Animation Ended");
            isAnimOnPlayed = false;
            nodeState = NodeState.Success;
            blackBoard.InAtkState = false;
            blackBoard.OnAtk = false;
        }

        public override void OnAnimationInTargetRate()
        {
            //현재 애니메이션이, AnimController의 TargetRate일 경우 한번 호출(다른 상태 진입시, 초기화)

            EnemyBlackBoard enemyBlackBoard = (EnemyBlackBoard)blackBoard;
            if (enemyBlackBoard)
            {

                if (enemyBlackBoard.shooter)
                {
                    enemyBlackBoard.shooter.TryAttack(blackBoard.targetPos, enemyBlackBoard.enemy.EnemyStats.Attack);
                }
            }

        }

        void TurnToTarget()
        {
            GameObject character = behaviorTree.thisCharacter;
            Transform targetPos = blackBoard.targetPos;
            Vector3 fireDir;

            fireDir = (targetPos.position - character.transform.position).normalized;

            fireDir.y = 0f;
            if (fireDir.sqrMagnitude > 0.01f)
            {
                //fireDir을 바라보는 회전값 쿼터니언 생성.
                Quaternion lookRot = Quaternion.LookRotation(fireDir);
                // 보간
                character.transform.rotation = Quaternion.Slerp(character.transform.rotation, lookRot, 1.5f);
            }
        }
    }
}

