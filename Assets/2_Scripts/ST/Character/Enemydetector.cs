using UnityEngine;
namespace LUP.ST
{

    public class EnemyDetector : MonoBehaviour
    {
        [Header("적 탐지 설정")]
        public string enemyTag = "Enemy";
        public bool showDebugInfo = false;

        private RangeBlackBoard characterInfo;
        private StatComponent stats;

        void Awake()
        {
            characterInfo = GetComponent<RangeBlackBoard>();
            stats = GetComponent<StatComponent>();

            if (showDebugInfo)
            {
                Debug.Log($"EnemyDetector 초기화: {gameObject.name}");
            }
        }

        void Update()
        {
            UpdateEnemyDetection();
        }

        private void UpdateEnemyDetection()
        {
            bool enemyFound = IsEnemyInRange();

            // 원거리 캐릭터 업데이트
            if (characterInfo != null)
            {
                bool wasInRange = characterInfo.enemyInRange;
                characterInfo.enemyInRange = enemyFound;

                // 상태 변화 시 로그 출력
                if (showDebugInfo && wasInRange != enemyFound)
                {
                    Debug.Log($"{characterInfo.characterName}: 적 탐지 상태 변화 {wasInRange} -> {enemyFound}");
                }
            }
        }

        private bool IsEnemyInRange()
        {
            // StatComponent의 AttackRange를 탐지 범위로 사용
            float detectionRange = stats != null ? stats.AttackRange : 10f;

            // 태그로 적 찾기
            GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);

            if (showDebugInfo && enemies.Length == 0)
            {
                Debug.LogWarning($"{gameObject.name}: '{enemyTag}' 태그를 가진 게임오브젝트를 찾을 수 없습니다!");
                return false;
            }

            foreach (GameObject enemy in enemies)
            {
                if (!enemy.activeInHierarchy) continue;

                float distance = Vector3.Distance(transform.position, enemy.transform.position);
                if (distance <= detectionRange)
                {
                    if (showDebugInfo)
                    {
                        Debug.Log($"{gameObject.name}: 적 발견! {enemy.name} (거리: {distance:F1})");
                    }
                    return true;
                }
            }

            return false;
        }

        /*
        // 현재 상태 출력 (F2 키로 호출)
        void LateUpdate()
        {
            if (Input.GetKeyDown(KeyCode.F2))
            {
                DebugEnemyDetection();
            }
        }

        private void DebugEnemyDetection()
        {
            float detectionRange = stats != null ? stats.AttackRange : 10f;
            GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);

            Debug.Log($"=== {gameObject.name} 적 탐지 상태 ===");
            Debug.Log($"적 태그: '{enemyTag}'");
            Debug.Log($"탐지 범위: {detectionRange}");
            Debug.Log($"발견된 적 수: {enemies.Length}");
            Debug.Log($"현재 enemyInRange: {characterInfo?.enemyInRange}");
            Debug.Log($"현재 manualMode: {characterInfo?.manualMode}");

            if (enemies.Length > 0)
            {
                Debug.Log("--- 모든 적과의 거리 ---");
                foreach (GameObject enemy in enemies)
                {
                    if (!enemy.activeInHierarchy) continue;

                    float distance = Vector3.Distance(transform.position, enemy.transform.position);
                    bool inRange = distance <= detectionRange;
                    Debug.Log($"{enemy.name}: {distance:F1} {(inRange ? "(범위 내)" : "(범위 밖)")}");
                }
            }
        }
        */
        // 탐지 범위 표시
        void OnDrawGizmosSelected()
        {
            float detectionRange = stats != null ? stats.AttackRange : 10f;

            // 탐지 범위 (하늘색)
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, detectionRange);

            // 캐릭터 이름 표시
            if (characterInfo != null)
            {
                Gizmos.color = characterInfo.enemyInRange ? Color.red : Color.white;
                Gizmos.DrawCube(transform.position + Vector3.up * 2, Vector3.one * 0.5f);
            }
        }
    }
}