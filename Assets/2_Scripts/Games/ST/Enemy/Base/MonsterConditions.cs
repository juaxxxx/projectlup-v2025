using UnityEngine;
namespace LUP.ST
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
            if (data.target == null)
            {
//                Debug.Log($"{data.name}: 타겟 없음");
                return false;
            }

            float distance = Vector3.Distance(data.transform.position, data.target.position);
            float attackRange = data.Stats.AttackRange;
            bool inRange = distance <= attackRange;

            Debug.Log($"{data.name}: 거리={distance:F2}, 공격범위={attackRange}, 범위내={inRange}");

            return distance <= data.Stats.AttackRange;
        }

        public static bool CheckHPBelow30(MonsterData data)
        {
            return data.Stats.CurrentHealth <= data.Stats.MaxHealth * 0.3f;
        }


    }
}