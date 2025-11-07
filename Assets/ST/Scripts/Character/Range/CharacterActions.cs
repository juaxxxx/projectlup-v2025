using UnityEngine;
namespace ST
{

    public class CharacterActions : MonoBehaviour
    {
        private Renderer rend;
        private CharacterInfo character;
        private StatComponent stats;

        public GameObject bulletPrefab;
        public Transform firePoint;
        public Transform enemyTransform;

        //public float autoReloadDelay = 1f; // 1초 이상 사격 안하면 재장전
        //public float reloadTime = 1f; // 재장전 소요 시간
        private float lastFireTime = -999f;

        private bool isReloading = false;
        private float reloadStartTime;

        void Awake()
        {
            rend = GetComponent<Renderer>();
            character = GetComponent<CharacterInfo>();
            stats = GetComponent<StatComponent>();

            if (stats != null)
            {
                stats.OnDeath += HandleDeath;
            }
        }

        void Update()
        {
            CheckAutoReload();
        }

        void OnDestroy()
        {
            if (stats != null)
            {
                stats.OnDeath -= HandleDeath;
            }
        }

        private void CheckAutoReload()
        {
            if (isReloading || character.IsCurrentAmmoFull()) return;

            // 마지막 사격 후 1초 이상 지났으면 자동 재장전 시작
            if (Time.time - lastFireTime >= stats.AttackCooldown)
            {
                if (character.currentAmmo < character.maxAmmo)
                {
                    StartReload();
                }
            }
        }

        private void HandleDeath()
        {
            if (rend != null)
            {
                rend.material.color = Color.black;
            }

            if (character != null)
            {
                Debug.Log($"{character.characterName} 사망!");
            }
        }

        void SetColor(Color color)
        {
            if (rend != null)
                rend.material.color = color;
        }

        void ShootBullet(Vector3 direction)
        {
            CombatUtility.ShootBullet(
                stats,
                bulletPrefab,
                firePoint,
                direction,
                "Enemy"
            );
        }

        public NodeState Retire(CharacterInfo character)
        {
            SetColor(Color.black);
            return NodeState.SUCCESS;
        }

        public NodeState FireManual(CharacterInfo character)
        {
            // 재장전 중이면 발사 불가
            if (isReloading)
            {
                return NodeState.FAILURE;
            }

            // 탄약이 없으면 발사 불가
            if (!character.HasAmmo())
            {
                Debug.Log($"{character.characterName}: 탄약 없음! 재장전 대기 중...");
                return NodeState.FAILURE;
            }


            float fireRate = stats != null ? stats.AttackSpeed : 0.1f;
            // 자동 발사 연사 속도 제한 체크
            if (Time.time - lastFireTime < fireRate)
            {
                return NodeState.RUNNING; // 아직 발사 시간이 안됨
            }
            // 탄약 소모 및 발사
            character.currentAmmo--;
            lastFireTime = Time.time; // 사격 시간 기록
            Debug.Log($"{character.characterName}: 수동 발사! 남은 탄약: {character.currentAmmo}");

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Vector3 targetPoint;

            if (Physics.Raycast(ray, out RaycastHit hit))
                targetPoint = hit.point;
            else
                targetPoint = ray.GetPoint(100f);

            Vector3 direction = targetPoint - firePoint.position;
            ShootBullet(direction);

            SetColor(Color.blue);

            //한번만 발사하고 싶으면 아래 코드 활성화
            //character.playerInputExists = false;

            return NodeState.SUCCESS;
        }

        public NodeState FireAuto(CharacterInfo character)
        {
            // 재장전 중이면 발사 불가
            if (isReloading)
            {
                return NodeState.FAILURE;
            }

            // 탄약이 없으면 발사 불가
            if (!character.HasAmmo())
            {
                return NodeState.FAILURE;
            }

            float fireRate = stats != null ? stats.AttackSpeed : 0.1f;
            // 자동 발사 연사 속도 제한 체크
            if (Time.time - lastFireTime < fireRate)
            {
                return NodeState.RUNNING; // 아직 발사 시간이 안됨
            }

            character.currentAmmo--;
            lastFireTime = Time.time;

            Transform target = enemyTransform;

            if (target == null)
            {
                target = FindNearestEnemy();
            }

            if (target != null)
            {
                Vector3 direction = target.position - firePoint.position;
                ShootBullet(direction);
                Debug.Log($"{character.characterName}: 자동 발사! 남은 탄약: {character.currentAmmo}");
            }

            SetColor(Color.cyan);
            return NodeState.RUNNING;
        }

        public NodeState Cover(CharacterInfo character)
        {
            SetColor(Color.green);
            return NodeState.SUCCESS;
        }

        public NodeState Reload(CharacterInfo character)
        {
            // 이미 재장전 중이면 진행 상태 확인
            if (isReloading)
            {
                float elapsedTime = Time.time - reloadStartTime;

                if (elapsedTime >= stats.AttackCooldown)
                {
                    // 재장전 완료
                    CompleteReload();
                    return NodeState.SUCCESS;
                }
                else
                {
                    // 재장전 진행 중
                    SetColor(Color.yellow);
                    return NodeState.RUNNING;
                }
            }

            // 이미 탄약이 가득하면 재장전 불필요
            if (character.IsCurrentAmmoFull())
            {
                return NodeState.SUCCESS;
            }

            // 재장전 시작
            StartReload();
            return NodeState.RUNNING;
        }

        private void StartReload()
        {
            if (isReloading) return;

            isReloading = true;
            reloadStartTime = Time.time;
            SetColor(Color.yellow);
            Debug.Log($"{character.characterName}: 재장전 시작! (1초 소요)");
        }

        private void CompleteReload()
        {
            int oldAmmo = character.currentAmmo;
            character.currentAmmo = character.maxAmmo;
            isReloading = false;
            SetColor(Color.white);
            Debug.Log($"{character.characterName}: 재장전 완료! {oldAmmo} -> {character.currentAmmo}");
        }
        private Transform FindNearestEnemy()
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

            if (enemies.Length == 0) return null;

            Transform nearest = null;
            float minDistance = float.MaxValue;

            // StatComponent의 AttackRange를 탐지 범위로 사용
            float attackRange = stats != null ? stats.AttackRange : 10f;

            foreach (GameObject enemy in enemies)
            {
                if (!enemy.activeInHierarchy) continue;

                float distance = Vector3.Distance(transform.position, enemy.transform.position);

                // 사거리 내의 적만 대상으로 함
                if (distance <= attackRange && distance < minDistance)
                {
                    minDistance = distance;
                    nearest = enemy.transform;
                }
            }

            return nearest;
        }

        // 재장전 상태 확인용
        public bool IsReloading => isReloading;
    }
}