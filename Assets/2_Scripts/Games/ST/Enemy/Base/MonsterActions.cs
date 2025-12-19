using UnityEngine;
namespace LUP.ST
{

    public static class MonsterActions
    {
        public static NodeState Dead(MonsterData data)
        {
            data.Die();
            return NodeState.SUCCESS;
        }

        public static NodeState Idle(MonsterData data)
        {
            data.ResetColor();
            data.Visual?.SetMoving(false);
            return NodeState.RUNNING;
        }

        public static NodeState Attack(MonsterData data)
        {
            if (data.target == null) return NodeState.FAILURE;

            StatComponent targetStats = data.target.GetComponent<StatComponent>();
            if (targetStats != null && targetStats.IsDead)
            {
                return NodeState.FAILURE;
            }

            // 공격 범위 벗어나면 실패
            float distance = Vector3.Distance(data.transform.position, data.target.position);
            if (distance > data.Stats.AttackRange)
            {
                data.isAttackingFlag = false;
                data.Stats.CancelAttack();
                return NodeState.FAILURE;
            }

            data.Visual?.SetMoving(false);
            data.transform.LookAt(new Vector3(data.target.position.x, data.transform.position.y, data.target.position.z));

            // 공격 시작
            if (!data.Stats.IsAttacking)
            {
                if (!data.Stats.CanStartAttack()) return NodeState.RUNNING;

                Debug.Log($"{data.name}: 공격 시작!");
                data.Stats.StartAttack();
                data.Visual?.PlayAttackAnimation();
                data.isAttackingFlag = true;
                data.hasAppliedHit = false;  // 리셋
            }

            AttackState attackState = data.Stats.UpdateAttack();

            // Hit 타이밍에 한 번만 데미지
            if (attackState == AttackState.Hit && !data.hasAppliedHit)
            {
                data.hasAppliedHit = true;

                if (data.bulletPrefab != null && data.firePoint != null)
                {
                    // 원거리 공격
                    Vector3 targetCenter = data.target.position + Vector3.up * 1f;  //중앙 조준
                    Vector3 direction = (data.target.position - data.firePoint.position).normalized;
                    CombatUtility.ShootBullet(
                        data.Stats,
                        data.bulletPrefab,
                        data.firePoint,
                        direction,
                        "Player"
                    );
                }
                else
                {
                    // 근접 공격
                    IDamageable damageable = data.target.GetComponent<IDamageable>();
                    if (damageable != null)
                    {
                        float damage = data.Stats.CalculateDamage();
                        damageable.TakeDamage(damage);
                        Debug.Log($"{data.name}: 데미지 {damage} 적용!");
                    }
                }
            }

            // 공격 종료
            if (attackState == AttackState.End)
            {
                Debug.Log($"{data.name}: 공격 종료");
                data.isAttackingFlag = false;
                return NodeState.SUCCESS;
            }

            return NodeState.RUNNING; 
        }

        public static NodeState MoveToPlayer(MonsterData data)
        {

            if (data.target == null) return NodeState.FAILURE;

            // 공격 범위 도달하면 SUCCESS (플레이어처럼)
            float distance = Vector3.Distance(data.transform.position, data.target.position);
            if (distance <= data.Stats.AttackRange)
            {
                data.Visual?.SetMoving(false);
                return NodeState.SUCCESS;
            }

            data.ResetColor();
            data.Visual?.SetMoving(true);
            Vector3 direction = (data.target.position - data.transform.position).normalized;
            data.transform.position += direction * data.Stats.MoveSpeed * Time.deltaTime;

            data.transform.LookAt(new Vector3(data.target.position.x, data.transform.position.y, data.target.position.z));

            return NodeState.RUNNING;
        }
    }
}