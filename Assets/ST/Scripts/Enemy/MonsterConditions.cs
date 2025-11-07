using UnityEngine;
namespace ST
{

    public static class MonsterConditions
    {
        public static bool CheckHPZero(MonsterData data)
        {
            return data.IsDead;
        }

        public static bool CheckIsStunned(MonsterData data)
        {
            if (data.isStunned && Time.time >= data.stunEndTime)
                data.isStunned = false;

            return data.isStunned;
        }

        public static bool CheckIsUsingSkill(MonsterData data)
        {
            if (data.isUsingSkill && Time.time >= data.skillEndTime)
                data.isUsingSkill = false;

            return data.isUsingSkill;
        }

        public static bool CheckInAttackRange(MonsterData data)
        {
            if (data.target == null) return false;

            float distance = Vector3.Distance(data.transform.position, data.target.position);
            return distance <= data.Stats.AttackRange;
        }

        public static bool CheckHPBelow30(MonsterData data)
        {
            return data.Stats.CurrentHealth <= data.Stats.MaxHealth * 0.3f;
        }

        public static bool CheckCoverAhead(MonsterData data)
        {
            if (data.target == null) return false;

            Vector3 dir = (data.target.position - data.transform.position).normalized;
            RaycastHit hit;

            if (Physics.Raycast(data.transform.position, dir, out hit, data.coverCheckDistance, data.coverLayer))
            {
                data.hidePosition = hit.point - dir * 1f;
                return true;
            }

            return false;
        }

        public static bool CheckHideCooldownOK(MonsterData data)
        {
            return (Time.time - data.lastHideTime) >= data.hideCooldown;
        }
    }
}