using UnityEngine;
namespace ST
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
            return NodeState.RUNNING;
        }

        public static NodeState Attack(MonsterData data)
        {
            if (data.target == null) return NodeState.FAILURE;

            CharacterInfo targetInfo = data.target.GetComponent<CharacterInfo>();
            if (targetInfo != null && targetInfo.IsHpZero())
            {
                return NodeState.FAILURE;  // 죽은 플레이어 공격 안 함!
            }

            data.transform.LookAt(new Vector3(data.target.position.x, data.transform.position.y, data.target.position.z));

            // 원거리 공격
            if (data.bulletPrefab != null && data.firePoint != null)
            {
                if (!data.Stats.IsAttacking && data.Stats.CanStartAttack())
                {
                    data.Stats.StartAttack();
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
                if (!data.Stats.IsAttacking && data.Stats.CanStartAttack())
                {
                    data.Stats.StartAttack();
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

        public static NodeState Hide(MonsterData data)
        {
            if (!data.isHiding)
            {
                data.isHiding = true;
                data.lastHideTime = Time.time;
            }

            float distance = Vector3.Distance(data.transform.position, data.hidePosition);

            if (distance > 0.5f)
            {
                Vector3 direction = (data.hidePosition - data.transform.position).normalized;
                data.transform.position += direction * data.Stats.MoveSpeed * Time.deltaTime;
            }

            if (Time.time - data.lastHideTime >= 3f)
            {
                data.isHiding = false;
                return NodeState.SUCCESS;
            }

            return NodeState.RUNNING;
        }

        public static NodeState MoveToPlayer(MonsterData data)
        {

            if (data.target == null) return NodeState.FAILURE;

            data.ResetColor();

            Vector3 direction = (data.target.position - data.transform.position).normalized;
            data.transform.position += direction * data.Stats.MoveSpeed * Time.deltaTime;

            data.transform.LookAt(new Vector3(data.target.position.x, data.transform.position.y, data.target.position.z));

            return NodeState.RUNNING;
        }
    }
}