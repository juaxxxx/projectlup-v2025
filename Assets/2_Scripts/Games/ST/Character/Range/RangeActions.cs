using UnityEngine;
namespace LUP.ST
{

    public class RangeActions : MonoBehaviour
    {
        private Renderer rend;
        private RangeBlackBoard character;
        private StatComponent stats;
        private VisualComponent visual;

        public GameObject bulletPrefab;
        public Transform firePoint;
        public Transform enemyTransform;
        public LayerMask aimMask;

        //public float autoReloadDelay = 1f; // 1초 이상 사격 안하면 재장전
        //public float reloadTime = 1f; // 재장전 소요 시간
        private float lastFireTime = -999f;
        public float maxManualAimAngle = 90f;

        private bool isReloading = false;
        private float reloadStartTime;

        private Rigidbody rb;

        private bool deathStarted = false;
        private int deathStateHash = 0;
        private const int AnimLayer = 0;

        private Animator anim;

        void Awake()
        {
            rend = GetComponent<Renderer>();
            character = GetComponent<RangeBlackBoard>();
            stats = GetComponent<StatComponent>();
            rb = GetComponent<Rigidbody>();
            visual = GetComponent<VisualComponent>();
        }

        void Update()
        {
            if (!character.manualMode)  // 자동 모드일 때만 자동 리로드
            {
                CheckAutoReload();
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

        void SetColor(Color color)
        {
            if (rend != null)
                rend.material.color = color;
        }

        void ShootBullet(Vector3 direction)
        {
            visual?.PlayAttackAnimation();
            CombatUtility.ShootBullet(
                stats,
                bulletPrefab,
                firePoint,
                direction,
                "Enemy"
            );
        }

        public NodeState Retire(RangeBlackBoard character)
        {
            if (!deathStarted)
            {
                deathStarted = true;

                if (character != null)
                {
                    character.manualMode = false;
                    character.playerInputExists = false;
                    character.enemyInRange = false;
                }

                if (TryGetComponent<PlayerController>(out var pc))
                {
                    pc.enabled = false;
                }

                if (TryGetComponent<EnemyDetector>(out var detector))
                {
                    detector.enabled = false;
                }

                gameObject.tag = "Untagged";
                gameObject.layer = LayerMask.NameToLayer("Dead");

                foreach (var col in GetComponentsInChildren<Collider>())
                    col.enabled = false;

                if (rb != null)
                {
                    rb.linearVelocity = Vector3.zero;
                    rb.angularVelocity = Vector3.zero;
                    rb.isKinematic = true;
                }

                visual?.SetMoving(false);
            }

            if (anim == null)
                return NodeState.SUCCESS;

            AnimatorStateInfo info = anim.IsInTransition(AnimLayer)
                ? anim.GetNextAnimatorStateInfo(AnimLayer)
                : anim.GetCurrentAnimatorStateInfo(AnimLayer);

            if (deathStateHash == 0)
                deathStateHash = info.shortNameHash;

            if (info.shortNameHash == deathStateHash && info.normalizedTime >= 1f)
            {
                Debug.Log($"{name} Retire");
                return NodeState.SUCCESS;
            }

            return NodeState.RUNNING;
        }

        public NodeState FireManual(RangeBlackBoard character)
        {
            // 재장전 중이면 발사 불가
            if (isReloading)
                return NodeState.FAILURE;

            // 탄약이 없으면 발사 불가
            if (!character.HasAmmo())
            {
                Debug.Log($"{character.characterName}: 탄약 없음! 재장전 대기 중...");
                return NodeState.FAILURE;
            }

            float fireRate = stats != null ? stats.AttackSpeed : 0.1f;
            if (Time.time - lastFireTime < fireRate)
                return NodeState.RUNNING;

            // 1) 마우스 레이 → targetPoint 구하기 (현재 쓰는 aimMask 사용)
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Vector3 targetPoint;

            if (Physics.Raycast(ray, out RaycastHit hit, 100f, aimMask, QueryTriggerInteraction.Ignore))
                targetPoint = hit.point;
            else
                targetPoint = ray.GetPoint(100f);

            // 2) 시야각 제한 체크 (Y는 무시하고 수평 기준)
            Vector3 dirWorld = targetPoint - firePoint.position;
            Vector3 dirFlat = new Vector3(dirWorld.x, 0f, dirWorld.z);

            if (dirFlat.sqrMagnitude < 0.0001f)
                return NodeState.FAILURE; // 너무 가까우면 무시

            dirFlat.Normalize();

            Vector3 forwardFlat = new Vector3(transform.forward.x, 0f, transform.forward.z).normalized;
            float angle = Vector3.Angle(forwardFlat, dirFlat);

            if (angle > maxManualAimAngle)
            {
                // 시야각 밖 → 입력 무시
                // Debug.Log($"{character.characterName}: 시야각 밖 입력 (angle={angle:F1})");
                return NodeState.FAILURE;
            }

            // 3) 실제 발사 처리
            character.currentAmmo--;
            lastFireTime = Time.time;

            Vector3 direction = dirWorld;  // 위에서 만든 dirWorld 그대로 사용
            ShootBullet(direction);

            SetColor(Color.blue);
            return NodeState.SUCCESS;
        }


        public NodeState FireAuto(RangeBlackBoard character)
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

            Vector3 direction;

            if (character.manualMode && character.playerInputExists)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                Vector3 targetPoint;

                if (Physics.Raycast(ray, out RaycastHit hit, 100f, aimMask, QueryTriggerInteraction.Ignore))
                    targetPoint = hit.point;
                else
                    targetPoint = ray.GetPoint(100f);

                Vector3 dirWorld = targetPoint - firePoint.position;
                Vector3 dirFlat = new Vector3(dirWorld.x, 0f, dirWorld.z);

                if (dirFlat.sqrMagnitude < 0.0001f)
                    return NodeState.FAILURE;

                dirFlat.Normalize();
                Vector3 forwardFlat = new Vector3(transform.forward.x, 0f, transform.forward.z).normalized;
                float angle = Vector3.Angle(forwardFlat, dirFlat);

                if (angle > maxManualAimAngle)
                    return NodeState.FAILURE;

                direction = dirWorld;
            }
            else
            {
                // 평소처럼 자동 타겟팅
                Transform target = enemyTransform;

                if (target == null)
                {
                    target = FindNearestEnemy();
                }

                if (target == null)
                {
                    // 타겟이 아예 없으면 이번 발사는 실패로 처리
                    SetColor(Color.green);
                    return NodeState.FAILURE;
                }

                direction = target.position - firePoint.position;
            }

            ShootBullet(direction);
//            Debug.Log($"{character.characterName}: 자동 발사! 남은 탄약: {character.currentAmmo}");

            SetColor(Color.cyan);
            return NodeState.SUCCESS;
        }

        public NodeState Cover(RangeBlackBoard character)
        {
            SetColor(Color.green);
            return NodeState.SUCCESS;
        }

        public NodeState Reload(RangeBlackBoard character)
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
            visual?.PlayReloadAnimation();
            isReloading = true;
            reloadStartTime = Time.time;
            SetColor(Color.yellow);
            //Debug.Log($"{character.characterName}: 재장전 시작! (1초 소요)");
        }

        private void CompleteReload()
        {
            int oldAmmo = character.currentAmmo;
            character.currentAmmo = character.maxAmmo;
            isReloading = false;
            SetColor(Color.white);
            //Debug.Log($"{character.characterName}: 재장전 완료! {oldAmmo} -> {character.currentAmmo}");
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

        public void OnEnterManualMode()
        {
            // 1. 재장전 강제 종료 (원한다면 유지해도 되지만, 입력 무시 안 되게 하려면 끄는 게 편함)
            if (isReloading)
            {
                isReloading = false;
               // Debug.Log($"{character.characterName}: 수동 모드 진입 - 재장전 상태 해제");
            }

            // 2. 쿨타임 초기화: 수동 모드로 바뀐 순간 바로 쏠 수 있게
            if (stats != null)
            {
                lastFireTime = Time.time - stats.AttackSpeed;
            }
            else
            {
                lastFireTime = -999f;
            }

            if (!character.HasAmmo())
            {
                StartReload();
            }
            

           // Debug.Log($"{character.characterName}: 수동 모드 진입 - 쿨타임/재장전 상태 리셋");
        }
    }
}