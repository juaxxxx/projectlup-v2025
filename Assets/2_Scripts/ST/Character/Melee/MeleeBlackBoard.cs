using UnityEngine;
namespace LUP.ST
{
    public class MeleeBlackBoard : MonoBehaviour
    {
        public Transform Target;
        public float DistToTarget;
        public bool InCover;
        public Vector3 HomePos;
        public float CoverRadius = 0.7f;

        public int Ammo = 5, MaxAmmo = 5;

        public float noEnemyReturnDelay = 5f;      
        private float lastEnemySeenTime = 0f;

        private StatComponent stats;

        void Awake()
        {
            stats = GetComponent<StatComponent>();
            HomePos = transform.position;
            lastEnemySeenTime = Time.time;
        }

        void Update()
        {
            if (Target != null) DistToTarget = Vector3.Distance(transform.position, Target.position);
            InCover = Vector3.Distance(transform.position, HomePos) <= CoverRadius;
        }

        // 트리에서 쓰는 조건
        public bool IsHpZero() => stats.IsDead;
        public bool HasAttackChance() => Ammo > 0;
        public bool CanAttack()
        {
            if (Ammo <= 0 || Target == null) return false;
            bool inRange = DistToTarget <= stats.AttackRange;
            return inRange && (stats.CanStartAttack() || stats.IsAttacking);
        }
        public bool IsEnemyWithinDetectionRange()
        {
            return Target != null && DistToTarget <= /* 탐지반경 값 */ 15f;
        }

        public void ReportEnemySeen()
        {
            lastEnemySeenTime = Time.time;
        }

        // 5초 동안 아무도 못 봤고, 현재 엄폐 밖이면 복귀 필요
        public bool ShouldReturnByNoEnemy()
        {
            return !InCover && (Time.time - lastEnemySeenTime >= noEnemyReturnDelay);
        }
    }

}