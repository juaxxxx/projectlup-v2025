using UnityEngine;
namespace LUP.ST
{

    public class CharacterInfo_Melee : MonoBehaviour
    {
        public string characterName = "";
        public bool manualMode = false;
        public bool playerInputExists = false;

        private StatComponent stats;
        private Vector3 initialPosition;

        [Header("근접 전투 설정")]
        public int maxAttackChances = 10;
        private int currentAttackChances;

        void Awake()
        {
            stats = GetComponent<StatComponent>();
            initialPosition = transform.position;
            currentAttackChances = maxAttackChances;

            if (stats == null)
            {
                Debug.LogError($"{gameObject.name}: StatComponent가 없습니다!");
            }
        }

        // 기본 정보
        public bool IsManualMode() => manualMode;
        public bool IsHpZero() => stats != null && stats.IsDead;
        public bool IsPlayerInputExists() => playerInputExists;
        public StatComponent Stats => stats;
        public Vector3 InitialPosition => initialPosition;
        public float MaxMoveDistance => stats != null ? stats.MaxMoveDistance : 8f;
        public float AttackRange => stats != null ? stats.AttackRange : 2f;

        // 공격 횟수
        public bool HasAttackChance() => currentAttackChances > 0;

        public void ConsumeAttackChance()
        {
            if (currentAttackChances > 0)
                currentAttackChances--;
        }

        public void ReloadAttackChances()
        {
            currentAttackChances = maxAttackChances;
            Debug.Log($"{characterName} 재장전 완료! (공격 횟수: {currentAttackChances})");
        }

        // 탐지 범위 내 적 확인 (초기 위치 기준)
        public bool IsEnemyWithinDetectionRange()
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

            foreach (GameObject enemy in enemies)
            {
                if (!enemy.activeInHierarchy) continue;

                float distanceFromInitial = Vector3.Distance(enemy.transform.position, initialPosition);
                if (distanceFromInitial <= MaxMoveDistance)
                {
                    return true;
                }
            }

            return false;
        }

        // 공격 범위 내 적 확인 (현재 위치 기준)
        public bool IsWithinAttackRange()
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

            foreach (GameObject enemy in enemies)
            {
                if (!enemy.activeInHierarchy) continue;

                float distance = Vector3.Distance(transform.position, enemy.transform.position);
                if (distance <= AttackRange)
                {
                    return true;
                }
            }

            return false;
        }

        // 가장 가까운 적 찾기 (초기 위치 기준 최대 이동거리 내)
        public Transform FindNearestEnemy()
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            if (enemies.Length == 0) return null;

            Transform nearest = null;
            float minDistance = float.MaxValue;

            foreach (GameObject enemy in enemies)
            {
                if (!enemy.activeInHierarchy) continue;

                // 초기 위치 기준 최대 이동거리 내에 적만 탐지
                float distanceFromInitial = Vector3.Distance(enemy.transform.position, initialPosition);

                if (distanceFromInitial <= MaxMoveDistance)
                {
                    float distance = Vector3.Distance(transform.position, enemy.transform.position);
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        nearest = enemy.transform;
                    }
                }
            }

            return nearest;
        }

        // 이동 가능 범위 표시
        void OnDrawGizmos()
        {
            if (stats == null) return;

            // 초기 위치에서 최대 이동거리 표시 (노란색)
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(initialPosition, MaxMoveDistance);

            // 현재 위치에서 공격 범위 표시 (빨간색)
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, AttackRange);
        }
    }
}