using UnityEngine;
namespace ST
{

    /// 근접 캐릭터용 블랙보드
    /// - 트리에서 쓰는 모든 조건/상태/공용 데이터의 "단일 출처"
    /// - Sensor/StatComponent가 채운 값을 읽어서, BT에서 바로 쓸 수 있는 Bool 함수 제공
    [RequireComponent(typeof(StatComponent))]
    public class MeleeBlackboard : MonoBehaviour
    {
        [Header("References")]
        public StatComponent stats;
        public EnemySensor_Melee sensor;      // 감지 모듈(가장 가까운 적/탐지/사거리 갱신)

        [Header("Mode/Input")]
        public bool manualMode = false;     // 수동 모드 여부
        public bool playerInputExists = false;  // 1회성 입력 플래그(소비형)
        public Vector3 clickWorldPoint;     // 수동 이동 클릭 좌표(월드)

        [Header("Chances (공격 스톡)")]
        public int maxAttackChances = 10;   // AttackChance 
        [SerializeField] private int currentAttackChances;

        [Header("Movement")]
        [Tooltip("스폰 시점의 초기 위치")]
        public Vector3 initialPosition;
        [Tooltip("초기 위치 기준 최대 이동반경(미지정 시 StatComponent.MaxMoveDistance 사용)")]
        public float overrideMaxMoveDistance = -1f;
        [Tooltip("목표 도달 판단 허용 오차")]
        public float closeEnough = 0.15f;

        // === 읽기 전용 프로퍼티 ===
        public bool IsDead => stats && stats.IsDead;
        public bool ManualMode => manualMode;
        public bool PlayerInputExists => playerInputExists;
        public int CurrentAttackChances => currentAttackChances;
        public float MaxMoveDistance => overrideMaxMoveDistance > 0f ? overrideMaxMoveDistance : (stats ? stats.MaxMoveDistance : 5f);
        public float AttackRange => stats ? stats.AttackRange : 2f;
        public Vector3 InitialPosition => initialPosition;

        // 센서 연동 (EnemySensor가 매 프레임 갱신)
        public bool EnemyInDetection => sensor && sensor.EnemyInDetection;
        public bool EnemyInAttackRange => sensor && sensor.EnemyInAttackRange;
        public Transform CurrentTarget => sensor ? sensor.CurrentTarget : null;

        // ===== 생명주기 =====
        void Awake()
        {
            if (!stats) stats = GetComponent<StatComponent>();
            if (initialPosition == Vector3.zero) initialPosition = transform.position;
            currentAttackChances = Mathf.Max(maxAttackChances, 0);
        }

        // ===== 외부 세터(입력/스폰/리셋 등) =====
        public void SetManualInput(Vector3 worldPoint)
        {
            clickWorldPoint = worldPoint;
            playerInputExists = true; // 소비형 플래그: Actions에서 처리 후 false로 돌리기
        }

        public void ClearManualInput()
        {
            playerInputExists = false;
        }

        public void ResetAtSpawn(Vector3 spawnPos)
        {
            initialPosition = spawnPos;
            currentAttackChances = Mathf.Max(maxAttackChances, 0);
            manualMode = false;
            playerInputExists = false;
        }

        // ===== 공격 스톡 =====
        public bool HasAttackChance() => currentAttackChances > 0;

        public void ConsumeAttackChance()
        {
            if (currentAttackChances > 0) currentAttackChances--;
        }

        public void ReloadAttackChances()
        {
            currentAttackChances = Mathf.Max(maxAttackChances, 0);
        }

        // ===== BT에서 바로 쓰는 조건 함수들(이미지 매핑) =====
        // 기존 네 코드와 호환되는 시그니처 이름 유지

        public bool IsHpZero() => IsDead;

        public bool IsManualMode() => ManualMode;

        public bool IsPlayerInputExists() => PlayerInputExists;

        public bool HasAttackChance_Bool() => HasAttackChance(); // ConditionNode에 던지기 용

        public bool IsEnemyWithinDetectionRange() => EnemyInDetection;

        public bool IsWithinAttackRange() => EnemyInAttackRange;

        /// 이미지의 "CanAttack == true" 해석:
        /// - 공격 스톡 있음
        /// - 공격 사거리 안
        /// - StatComponent 쿨타임/상태상 공격 시작 가능
        public bool CanAttack()
        {
            if (!stats) return false;
            return HasAttackChance() && EnemyInAttackRange && (stats.CanStartAttack() || stats.IsAttacking);
        }

        /// 이미지의 "IsNeedToMove == true" 해석:
        /// - 탐지 범위에 적이 있으나, 아직 공격 사거리는 아님
        public bool IsNeedToMove()
        {
            return EnemyInDetection && !EnemyInAttackRange;
        }

#if UNITY_EDITOR
        // ===== 디버그 시각화 =====
        void OnDrawGizmosSelected()
        {
            // 초기 위치 기준 최대 이동 반경
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(initialPosition == Vector3.zero ? transform.position : initialPosition, MaxMoveDistance);

            // 현재 위치 기준 공격 반경
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, AttackRange);

            // 클릭 위치
            if (playerInputExists)
            {
                Gizmos.color = Color.cyan;
                Gizmos.DrawSphere(clickWorldPoint, 0.1f);
                Gizmos.DrawLine(transform.position, clickWorldPoint);
            }
        }
#endif
    }

}