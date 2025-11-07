using System.Collections.Generic;
using UnityEngine;
namespace ST
{


    public class CharacterBT_Melee : BehaviorTreeBase
    {
        private MeleeBlackboard bb;
        private MeleeActions act;

        protected override BaseNode SetupTree()
        {
            bb = GetComponent<MeleeBlackboard>();
            act = GetComponent<MeleeActions>();

            // ====== 액션 래핑 ======
            NodeState Retire() { return act.Retire(); }
            NodeState MeleeAttack() { return act.MeleeAttackLoop(); }
            NodeState MoveExecution() { return act.MoveExecution(); }   // 이동 실행
            NodeState CoverAction() { return act.Cover(); }           // 엄폐 복귀
            NodeState IdleAction() { return act.Idle(); }            // 휴식
            NodeState MoveByInputStart() { return act.MoveToMaxDistanceByInput(); } // 입력 방향 이동
            NodeState MoveToEnemyStart() { return act.MoveToMaxDistanceToEnemy(); } // 적 방향 이동

            // ============================================================
            // Root
            // ============================================================
            Selector root = new Selector(new List<BaseNode>
        {
            // 1. HP <= 0 → 리타이어 (최우선)
            new Decorator(bb.IsHpZero, new ActionNode(Retire)),

            // 2. CanAttack == true → 근접공격 (공격 우위)
            new Decorator(bb.CanAttack, new ActionNode(MeleeAttack)),
            
            // 3. (추가) 공격 기회 == 0 & 초기 위치 도달 안 됨 → 엄폐 복귀 (규칙 5, 7)
            // 공격 기회가 0이 되는 순간, 이동 중이든 아니든 커버로 전환
            new Decorator(() => !bb.HasAttackChance() && !act.ReachedInitialPosition(), new ActionNode(CoverAction)),

            // 4. isMoving == true → 이동 실행 (이미 시작된 이동 보호)
            new Decorator(() => act.IsMoving, new ActionNode(MoveExecution)),
            
            // 5. isManualMode == true → 수동 모드 분기
            new Decorator(bb.IsManualMode, new Selector(new List<BaseNode>
            {
                // (5-1) 입력이 있으면 → 입력 방향으로 이동 목표 설정 (START)
                new Decorator(bb.IsPlayerInputExists, new ActionNode(MoveByInputStart)),
                
                // (5-2) 공격 기회 == 0 → 엄폐 복귀 (4번에서 처리했으나 안전장치)
                new Decorator(() => !bb.HasAttackChance(), new ActionNode(CoverAction)),
                
                // (5-3) 기본 휴식 (입력 대기)
                new ActionNode(IdleAction)
            })),

            // 6. isEnemyInDetectionRange == true → 자동 모드 분기
            new Decorator(bb.IsEnemyWithinDetectionRange, new Selector(new List<BaseNode>
            {
                // (6-1) IsNeedToMove == true (공격 기회 있음 & 사거리 밖) → 적 방향 이동 목표 설정 (START)
                new Decorator(bb.IsNeedToMove, new ActionNode(MoveToEnemyStart)),

                // (6-2) 공격 기회 == 0 → 엄폐 복귀 (4번에서 처리했으나 안전장치)
                new Decorator(() => !bb.HasAttackChance(), new ActionNode(CoverAction)),

                // (6-3) 기본 휴식 (적이 탐지됐으나 공격/이동 불필요)
                new ActionNode(IdleAction)
            })),

            // 7. 기본 → 휴식 (최종 안전망)
            new ActionNode(IdleAction)
        });

            return root;
        }
    }

}