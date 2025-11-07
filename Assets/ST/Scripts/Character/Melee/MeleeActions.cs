using UnityEngine;
using System.Collections;
namespace ST
{

    public class MeleeActions : MonoBehaviour
    {
        [Header("Refs")]
        public MeleeBlackboard bb;          // 블랙보드
        public StatComponent stats;         // 스탯

        [Header("Attack Settings")]
        public LayerMask enemyLayer;        // 타격 판정용 레이어(센서와 동일)
        public float slashAngle = 120f;     // 전방 각도 제한(도)
        public int maxHitBuffer = 24;       // NonAlloc 버퍼

        [Header("Movement Settings")]
        public float faceTurnSpeed = 12f;   // 회전 보간 속도

        // 내부 상태
        private bool isMoving = false;
        private bool isAttacking = false;
        private Vector3 currentMoveTarget;
        private Collider[] hitBuffer;
        public bool IsMoving { get { return isMoving; } }
        public bool HasReachedMoveTarget()
        {
            // 이동이 끝났고, 목표 지점에 충분히 근접했는지
            return !isMoving && Reached(currentMoveTarget, bb.closeEnough);
        }

        private void Awake()
        {
            if (bb == null) bb = GetComponent<MeleeBlackboard>();
            if (stats == null) stats = GetComponent<StatComponent>();
            if (maxHitBuffer < 1) maxHitBuffer = 1;
            hitBuffer = new Collider[maxHitBuffer];
        }

        // ====== 공통: 협조적 중단 체크 ======
        private bool ShouldAbortForHigherPriority()
        {
            // 사망, 또는 공격기회 0이면 상위 분기로 넘겨야 함
            if (bb.IsDead)
            {
                Debug.Log("[BT] Abort: 캐릭터 사망");
                return true;
            }
            return false;
        }

        // ====== 이동 공통 ======
        private void BeginMoveTo(Vector3 worldTarget)
        {
            Debug.Log("[BT] 이동 시작 → " + worldTarget);
            currentMoveTarget = worldTarget;
            isMoving = true;
        }

        private bool Reached(Vector3 worldTarget, float epsilon)
        {
            float dist = Vector3.Distance(transform.position, worldTarget);
            return dist <= epsilon;
        }

        public bool ReachedInitialPosition()
        {
            return Reached(bb.InitialPosition, bb.closeEnough);
        }

        private void TickMove()
        {
            // 상위 우선순위 사유로 양보
            if (ShouldAbortForHigherPriority())
            {
                isMoving = false;
                return;
            }

            // 이동 도중 전투 상황 변화에 대한 협조적 중단
            // 1) 공격 기회가 0 → 즉시 이동 중단 (커버로 전환하게 하려는 의도)
            if (!bb.HasAttackChance())
            {
                Debug.Log("[BT] 이동 중단: 공격 스톡 없음");
                isMoving = false;
                return;
            }

            // 2) 사거리 진입 + 공격 기회 있음 → 이동 즉시 종료(공격 분기로 넘기기 위함)
            if (bb.HasAttackChance() && bb.EnemyInAttackRange)
            {
                Debug.Log("[BT] 이동 중단: 공격 사거리 진입");
                isMoving = false;
                return;
            }

            // 실제 이동
            Vector3 pos = transform.position;
            Vector3 delta = currentMoveTarget - pos;
            float step = stats.MoveSpeed * Time.deltaTime;

            if (delta != Vector3.zero)
            {
                Quaternion targetRot = Quaternion.LookRotation(delta);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * faceTurnSpeed);
            }

            transform.position = Vector3.MoveTowards(pos, currentMoveTarget, step);

            // 목표 도달 시 이동 종료
            if (Reached(currentMoveTarget, bb.closeEnough))
            {
                Debug.Log("[BT] 이동 완료 → 목표 도달");
                isMoving = false;
            }
        }

        // ====== 액션: 이동 실행 (isMoving == true일 때 호출되는 리프) ======
        public NodeState MoveExecution()
        {
            // 이동 중이 아니라면 이 노드가 처리할 일이 없음 → 실패로 양보
            if (!isMoving)
            {
                Debug.Log("[BT] MoveExecution 실패: isMoving=false");
                return NodeState.FAILURE;
            }

            // 한 틱 이동
            TickMove();

            // 이동이 계속된다 → RUNNING
            if (isMoving)
            {
                Debug.Log("[BT] MoveExecution: RUNNING");
                return NodeState.RUNNING;
            }

            bool reached = Reached(currentMoveTarget, bb.closeEnough);
            Debug.Log("[BT] MoveExecution 종료 → " + (reached ? "SUCCESS(목표도달)" : "FAILURE(상황변화)"));
            return reached ? NodeState.SUCCESS : NodeState.FAILURE;
        }

        // ====== 액션: 휴식 ======
        public NodeState Idle()
        {
            if (!bb.HasAttackChance())
            {
                bb.ReloadAttackChances();
                Debug.Log("[BT] Idle: 재장전 완료");
            }
            return NodeState.SUCCESS;
        }

        // ====== 액션: 리타이어 ======
        public NodeState Retire()
        {
            // 필요한 비주얼/컴포넌트 비활성 처리 지점
            Debug.Log("[BT] Retire 실행 (사망)");
            return NodeState.SUCCESS;
        }

        // ====== 액션: 수동 - 입력 방향으로 최대거리 이동 (규칙: 수동 2, 6) ======
        public NodeState MoveToMaxDistanceByInput()
        {
            if (ShouldAbortForHigherPriority()) return NodeState.FAILURE;
            if (isAttacking) return NodeState.RUNNING;

            // 입력이 없으면 실패 (상위 Selector에서 Idle 등으로 분기)
            if (!bb.PlayerInputExists)
            {
                Debug.Log("[BT] 수동 이동 실패: 입력 없음");
                return NodeState.FAILURE;
            }

            // 초기 위치에서 클릭 방향으로 최대 거리 목적지 산출
            Vector3 start = bb.InitialPosition;
            Vector3 dir = (bb.clickWorldPoint - start).normalized;
            float rawDist = Vector3.Distance(bb.clickWorldPoint, start);
            float dist = Mathf.Min(rawDist, bb.MaxMoveDistance);
            Vector3 target = start + dir * dist;

            Debug.Log("[BT] 수동 이동 시작 → " + target);
            // 이동 개시
            BeginMoveTo(target);

            // 1회성 입력 플래그 소모
            bb.ClearManualInput();

            // 이동 중에는 RUNNING, 사거리 진입시 TickMove에서 isMoving=false 처리됨
            return NodeState.RUNNING;
        }

        // ====== 액션: 자동 - 적 방향으로 최대거리 이동 (규칙: 자동 2, 6) ======
        public NodeState MoveToMaxDistanceToEnemy()
        {
            if (ShouldAbortForHigherPriority()) return NodeState.FAILURE;
            if (isAttacking) return NodeState.RUNNING;

            // 탐지된 적이 없다면 실패 (상위에서 Idle 등으로)
            if (!bb.EnemyInDetection || bb.CurrentTarget == null)
            {
                Debug.Log("[BT] 자동 이동 실패: 적 탐지 없음");
                return NodeState.FAILURE;
            }

            // 초기 위치에서 "현재 타깃 방향"으로 최대 거리 목적지 산출
            Vector3 start = bb.InitialPosition;
            Vector3 toEnemy = (bb.CurrentTarget.position - start);
            if (toEnemy == Vector3.zero) toEnemy = transform.forward;
            Vector3 dir = toEnemy.normalized;
            Vector3 target = start + dir * bb.MaxMoveDistance;

            Debug.Log("[BT] 자동 이동 시작 → " + target);
            BeginMoveTo(target);
            return NodeState.RUNNING;
        }

        // ====== 액션: 커버(엄폐 위치 복귀 + 재장전) (규칙: 수/자 5, 7) ======
        public NodeState Cover()
        {
            if (ShouldAbortForHigherPriority()) return NodeState.FAILURE;
            if (isAttacking) return NodeState.RUNNING;

            // 이미 도달 → 재장전 후 성공
            if (ReachedInitialPosition())
            {
                Debug.Log("[BT] Cover: 초기 위치 도달 완료");
                return NodeState.SUCCESS;
            }

            if (!isMoving)
            {
                Debug.Log("[BT] 커버 이동 시작 → 초기 위치로 복귀");
                BeginMoveTo(bb.InitialPosition);
            }

            return NodeState.RUNNING;
        }

        // ====== 액션: 근접 공격 루프 (규칙: 수/자 3,4) ======
        public NodeState MeleeAttackLoop()
        {
            // 상위 사유(사망) / 공격기회 없음 / 사거리 이탈 / 적 소실 → 즉시 실패(상위 Selector 전환)
            if (bb.IsDead)
            {
                Debug.Log("[BT] 공격 중단: 사망");
                return NodeState.FAILURE;
            }
            if (!bb.HasAttackChance())
            {
                Debug.Log("[BT] 공격 중단: 스톡 0");
                return NodeState.FAILURE;
            }
            if (!bb.EnemyInAttackRange)
            {
                Debug.Log("[BT] 공격 중단: 사거리 이탈");
                return NodeState.FAILURE;
            }
            if (!bb.EnemyInDetection)
            {
                Debug.Log("[BT] 공격 중단: 적 소실");
                return NodeState.FAILURE;
            }

            if (isAttacking)
            {
                AttackState st = stats.UpdateAttack();
                if (st == AttackState.Hit)
                {
                    Debug.Log("[BT] 공격 히트 타이밍 → PerformSlash()");
                    PerformSlash();
                    return NodeState.RUNNING;
                }
                if (st == AttackState.End)
                {
                    Debug.Log("[BT] 공격 종료 → SUCCESS 반환");
                    isAttacking = false;
                    return NodeState.SUCCESS;
                }
                return NodeState.RUNNING;
            }

            if (!stats.CanStartAttack())
            {
                Debug.Log("[BT] 공격 대기: 쿨타임 중");
                return NodeState.RUNNING;
            }

            Debug.Log("[BT] 공격 시작 → StartAttack() 호출");
            isAttacking = true;
            stats.StartAttack();
            bb.ConsumeAttackChance();
            return NodeState.RUNNING;
        }

        // ====== 타격 판정 (NonAlloc + 전방 각도 제한) ======
        private void PerformSlash()
        {
            int count = Physics.OverlapSphereNonAlloc(transform.position, stats.AttackRange, hitBuffer, enemyLayer);
            Debug.Log("[BT] PerformSlash() → " + count + "개 충돌체 탐지");

            for (int i = 0; i < count; i++)
            {
                Collider col = hitBuffer[i];
                if (col == null) continue;

                Vector3 to = col.transform.position - transform.position;
                float angle = Vector3.Angle(transform.forward, to);
                if (angle > slashAngle * 0.5f) continue;

                IDamageable dmg = col.GetComponent<IDamageable>();
                if (dmg == null || dmg.IsDead()) continue;

                float damage = stats.CalculateDamage();
                dmg.TakeDamage(damage);
                Debug.Log("[BT] → " + col.name + " 에 " + damage + " 데미지 적용");
            }
        }
    }


}