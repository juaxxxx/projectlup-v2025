using UnityEngine;
using System.Collections;
using System;

namespace LUP.ST
{

    public class MeleeActions : MonoBehaviour
    {
        MeleeBlackBoard bb;
        StatComponent stats;
        VisualComponent visual;

        // 이동 제어용
        private bool isStopped = false;
        //private float stopDistanceBuffer = 0.1f; // 공격 진입 여유
        private float rotationSpeed = 10f;

        private bool hasAppliedHit = false;
        private const float HIT_RATIO = 0.55f;

        private Rigidbody rb;

        void Awake()
        {
            bb = GetComponent<MeleeBlackBoard>();
            stats = GetComponent<StatComponent>();
            visual = GetComponent<VisualComponent>();
            rb = GetComponent<Rigidbody>();
        }

        // 사망: 애니 등 실행. 트리는 Retire가 RUNNING이면 멈춘다고 가정
        public NodeState Retire()
        {
            Debug.Log($"{name} ▶ Retire (사망)");
            return NodeState.RUNNING;
        }

        // 엄폐(홈)으로 돌아가기: 단순 MoveTowards
        public NodeState Cover()
        {
            if (bb.IsAttackingFlag)
                return NodeState.FAILURE;

            Vector3 dst = bb.HomePos;
            float dist = Vector3.Distance(transform.position, dst);
            if (dist <= bb.CoverRadius)
            {
                if (!bb.InCover)
                {
                    Debug.Log($"{name} ▶ Cover 완료 (엄폐 도착)");
                }
                if (bb.HomeForward.sqrMagnitude > 0.001f)
                    transform.rotation = Quaternion.LookRotation(bb.HomeForward, Vector3.up);
                bb.InCover = true;
                return NodeState.SUCCESS;
            }

            // 이동
            visual?.SetMoving(true);
            bb.InCover = false;
            isStopped = false;
            MoveTowards(dst);
            //Debug.Log($"{name} ▶ Cover 이동 중");
            return NodeState.RUNNING;
        }

        // 즉시 리로드 (필요하면 Running 처리를 추가)
        public NodeState Reload()
        {
            bb.Ammo = bb.MaxAmmo;
            Debug.Log($"{name} ▶ Reload 완료 (Ammo={bb.Ammo})");
            return NodeState.SUCCESS;
        }

        // MoveToEnemy: 타겟 null -> FAILURE, 도달 시 공격 포함 또는 SUCCESS 처리
        public NodeState MoveToEnemy()
        {
            if (bb.IsAttackingFlag)
                return NodeState.FAILURE;

            if (bb.Target == null)
            {
                Debug.Log($"{name} ▶ MoveToEnemy 실패 (Target 없음)");
                visual?.SetMoving(false);
                return NodeState.FAILURE;
            }

            float attackRange = stats.AttackRange;
            float dist = bb.DistToTarget;

            // 목표를 바라보기 (부드럽게)
            Vector3 toTarget = (bb.Target.position - transform.position);
            toTarget.y = 0;
            if (toTarget.sqrMagnitude > 0.001f)
            {
                Quaternion targetRot = Quaternion.LookRotation(toTarget.normalized);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, rotationSpeed * Time.deltaTime);
            }

            // 공격 범위 이내면 공격 루틴으로 이동 (공격 중엔 정지)
            if (dist <= attackRange)
            {
                Debug.Log($"{name} ▶ 공격 사거리 진입");
                visual?.SetMoving(false);
                //return MeleeAttackLoop();
                return NodeState.SUCCESS;
            }

            // 아직 공격 거리 밖이면 이동
            visual?.SetMoving(true);
            isStopped = false;
            Vector3 targetPos = bb.Target.position;
            MoveTowards(targetPos);
            Debug.Log($"{name} ▶ 적에게 접근 중");
            return NodeState.RUNNING;
        }

        // 근접 공격 루프: 공격 중엔 정지, 히트 타이밍에서 데미지 적용
        public NodeState MeleeAttackLoop()
        {
            if (!stats.IsAttacking)
            {
                if (bb.Target == null || bb.DistToTarget > stats.AttackRange)
                {
                    bb.IsAttackingFlag = false;          
                    stats.CancelAttack();
                    if (rb != null) rb.isKinematic = false;
                    isStopped = false;
                    visual?.SetMoving(true);
                    return NodeState.FAILURE;
                }
            }

            // 정지(공격시 위치 고정)
            visual?.SetMoving(false);
            isStopped = true;

            // 공격 중엔 밀리지 않게
            if (rb != null)
            {
                rb.isKinematic = true;
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }

            if (stats.IsDead)
            {
                Debug.Log($"{name} ▶ 공격 중 사망");
                bb.IsAttackingFlag = false;
                return NodeState.FAILURE;
            }

            // 시작 가능한지 체크
            if (!stats.IsAttacking)
            {
                if (!stats.CanStartAttack()) return NodeState.RUNNING;
                Debug.Log($"{name} ▶ 공격 시작");
                stats.StartAttack();
                bb.IsAttackingFlag = true;
                hasAppliedHit = false;
            }

            float elapsed = Time.time - stats.attackStartTime;
            float hitTime = stats.AttackSpeed * HIT_RATIO;

            if (!hasAppliedHit && elapsed >= hitTime)
            {
                // 1회만 데미지 적용 (범위 내 다수 타격)
                hasAppliedHit = true;

                // 공격 범위 내의 모든 적 감지
                Collider[] hits = Physics.OverlapSphere(
                    transform.position,
                    stats.AttackRange,
                    LayerMask.GetMask("Enemy") 
                );

                int hitCount = 0;

                for (int i = 0; i < hits.Length; i++)
                {
                    IDamageable dmgable = hits[i].GetComponent<IDamageable>();
                    if (dmgable != null)
                    {
                        float dmg = stats.CalculateDamage();
                        dmgable.TakeDamage(dmg);
                        hitCount++;
                    }
                }

                Debug.Log($"{name} ▶ 공격 적중! {hitCount}명 타격 (범위 {stats.AttackRange:F1})");
            }


            // 진행/히트/종료 체크
            AttackState atkState = stats.UpdateAttack();
           
            if (atkState == AttackState.End)
            {
                // 한 회 공격 완료: 탄창 감소
                bb.Ammo = Mathf.Max(0, bb.Ammo - 1);
                Debug.Log($"{name} ▶ 공격 종료 (남은 탄창: {bb.Ammo})");
                bb.IsAttackingFlag = false;
                isStopped = false;
                if (rb != null) rb.isKinematic = false;
                // 공격 후 행동: 탄창 있으면 SUCCESS(상위에서 계속 공격/이동 판단)
                // 만약 Ammo==0이면 상위 시퀀스에서 Cover->Reload로 이동
                return NodeState.SUCCESS;
            }

            return NodeState.RUNNING;
        }

        // Idle: 전방 유지(움직임 금지 아님) — 여기서는 가만히 서서 탐지만 하게 둠
        public NodeState Idle()
        {
            if (!isStopped)
            {
                visual?.SetMoving(false);
                Debug.Log($"{name} ▶ Idle 진입");
            }
            isStopped = true; // Idle이면 정지 상태(필요 시 소폭 흔들기 추가)
            return NodeState.SUCCESS;
        }

        // 단순한 MoveTowards 래퍼
        private void MoveTowards(Vector3 destination)
        {
            if (bb.IsAttackingFlag)
                return;

            if (isStopped) return;

            // 목표를 바라보기 (부드럽게)
            Vector3 toTarget = (destination - transform.position);
            toTarget.y = 0;
            if (toTarget.sqrMagnitude > 0.001f)
            {
                Quaternion targetRot = Quaternion.LookRotation(toTarget.normalized);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, rotationSpeed * Time.deltaTime);
            }

            float step = stats.MoveSpeed * Time.deltaTime;

            // Y를 현재 위치로 고정 (바닥 높이 유지)
            Vector3 destFlat = new Vector3(destination.x, transform.position.y, destination.z);
            transform.position = Vector3.MoveTowards(transform.position, destFlat, step);
        }
    }
}