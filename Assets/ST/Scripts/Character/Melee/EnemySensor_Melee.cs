using UnityEngine;


namespace ST
{
    [DefaultExecutionOrder(-50)] // 블랙보드보다 먼저 갱신

    public class EnemySensor_Melee : MonoBehaviour
    {
        [Header("Sensor Settings")]
        [Tooltip("탐지할 Enemy Layer (여러 레이어 지정 가능)")]
        public LayerMask enemyLayer;

        [Tooltip("탐지 반경 (일반적으로 블랙보드.MaxMoveDistance와 동일)")]
        public float detectRadius = 8f;

        [Tooltip("공격 반경 (일반적으로 블랙보드.AttackRange와 동일)")]
        public float attackRadius = 2f;

        [Tooltip("한 번에 감지할 최대 적 수")]
        public int maxColliders = 16;

        private Collider[] hitBuffer;
        private MeleeBlackboard blackboard;

        [Header("Current State (ReadOnly)")]
        public bool EnemyInDetection { get; private set; }
        public bool EnemyInAttackRange { get; private set; }
        public Transform CurrentTarget { get; private set; }

        void Awake()
        {
            if (maxColliders < 1) maxColliders = 1;
            hitBuffer = new Collider[maxColliders];

            blackboard = GetComponent<MeleeBlackboard>();
            if (blackboard != null)
            {
                blackboard.sensor = this;
            }
        }

        void Update()
        {
            ScanEnemies();
        }

        private void ScanEnemies()
        {
            EnemyInDetection = false;
            EnemyInAttackRange = false;
            CurrentTarget = null;

            int count = Physics.OverlapSphereNonAlloc(
                transform.position,
                detectRadius,
                hitBuffer,
                enemyLayer
            );

            float minDistance = float.MaxValue;

            for (int i = 0; i < count; i++)
            {
                Collider col = hitBuffer[i];
                if (col == null || !col.gameObject.activeInHierarchy)
                    continue;

                // IDamageable이면서 살아있는 대상만 추적
                IDamageable damageable;
                if (col.TryGetComponent<IDamageable>(out damageable))
                {
                    if (damageable.IsDead())
                        continue;
                }

                float dist = Vector3.Distance(transform.position, col.transform.position);

                // 탐지 범위 안
                if (dist <= detectRadius)
                {
                    EnemyInDetection = true;

                    // 가장 가까운 적 갱신
                    if (dist < minDistance)
                    {
                        minDistance = dist;
                        CurrentTarget = col.transform;
                    }
                }

                // 공격 범위 안
                if (dist <= attackRadius)
                {
                    EnemyInAttackRange = true;
                }
            }
        }

#if UNITY_EDITOR
        void OnDrawGizmosSelected()
        {
            // 탐지 반경 (노랑)
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, detectRadius);

            // 공격 반경 (빨강)
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackRadius);

            // 현재 타깃 표시
            if (CurrentTarget != null)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawLine(transform.position, CurrentTarget.position);
                Gizmos.DrawSphere(CurrentTarget.position, 0.1f);
            }
        }
#endif
    }

}