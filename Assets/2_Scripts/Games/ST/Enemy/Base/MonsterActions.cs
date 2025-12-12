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
            Debug.Log($"{data.name}: Attack 진입!");
            if (data.target == null) return NodeState.FAILURE;

            RangeBlackBoard targetInfo = data.target.GetComponent<RangeBlackBoard>();
            if (targetInfo != null && targetInfo.IsHpZero())
            {
                return NodeState.FAILURE;  // 죽은 플레이어 공격 안 함!
            }

            data.Visual?.SetMoving(false);

            data.transform.LookAt(new Vector3(data.target.position.x, data.transform.position.y, data.target.position.z));

            // 원거리 공격
            if (data.bulletPrefab != null && data.firePoint != null)
            {
                Debug.Log($"{data.name}: 공격 시작!");
                if (!data.Stats.IsAttacking && data.Stats.CanStartAttack())
                {
                    data.Stats.StartAttack();
                    data.Visual?.PlayAttackAnimation();
                    data.SetColor(Color.yellow);
                }

                AttackState attackState = data.Stats.UpdateAttack();

                if (attackState == AttackState.Hit)
                {
                    Vector3 direction = (data.target.position - data.firePoint.position).normalized;

                    CombatUtility.ShootBullet(
                        data.Stats,
                        data.bulletPrefab,
                        data.firePoint,
                        direction,
                        "Player"
                    );

                    data.SetColor(Color.cyan);
                }
                else if (attackState == AttackState.End)
                {
                    data.ResetColor();
                }
            }
            // 근접 공격
            else
            {
                Debug.Log($"{data.name}: 공격 시작!");
                if (!data.Stats.IsAttacking && data.Stats.CanStartAttack())
                {
                    data.Stats.StartAttack();
                    data.Visual?.PlayAttackAnimation();
                    data.SetColor(Color.red);
                }

                AttackState attackState = data.Stats.UpdateAttack();

                if (attackState == AttackState.Hit)
                {
                    IDamageable damageable = data.target.GetComponent<IDamageable>();
                    if (damageable != null)
                    {
                        float damage = data.Stats.CalculateDamage();
                        damageable.TakeDamage(damage);
                    }

                    data.SetColor(Color.magenta);
                }
                else if (attackState == AttackState.End)
                {
                    data.ResetColor();
                }
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