using UnityEngine;
namespace ST
{

    public class CharacterActions_Melee : MonoBehaviour
    {
        private Renderer rend;
        private CharacterInfo_Melee character;
        private StatComponent stats;

        private Vector3 moveTargetPos;
        private bool isMoving = false;
        private Transform currentTarget;  // 현재 공격 대상
        private bool isAttacking = false;

        [Header("근접 공격 설정")]
        public float slashRange = 2f;       // 칼 휘두르는 범위
        public float slashAngle = 90f;      // 칼 휘두르는 각도
        public LayerMask enemyLayer;

        void Awake()
        {
            rend = GetComponent<Renderer>();
            character = GetComponent<CharacterInfo_Melee>();
            stats = GetComponent<StatComponent>();

            if (stats != null)
            {
                stats.OnDeath += HandleDeath;
            }
        }

        void Update()
        {
            // 이동 중
            if (isMoving && !isAttacking)
            {
                MoveToTarget();
            }
        }

        void OnDestroy()
        {
            if (stats != null)
            {
                stats.OnDeath -= HandleDeath;
            }
        }

        private void HandleDeath()
        {
            if (rend != null)
                rend.material.color = Color.black;
            if (character != null)
                Debug.Log($"{character.characterName} 사망!");
        }

        void SetColor(Color color)
        {
            if (rend != null)
                rend.material.color = color;
        }

        //이동 처리 적이 가까우면 멈춤
        private void MoveToTarget()
        {
            float step = stats.MoveSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, moveTargetPos, step);

            Vector3 dir = moveTargetPos - transform.position;
            if (dir != Vector3.zero)
                transform.rotation = Quaternion.LookRotation(dir);

            // 목표 도착 또는 적이 공격 범위에 들어오면 멈춤
            if (Vector3.Distance(transform.position, moveTargetPos) < 0.1f ||
                (currentTarget != null && Vector3.Distance(transform.position, currentTarget.position) <= stats.AttackRange))
            {
                isMoving = false;
                Debug.Log($"{character.characterName} 이동 완료");
            }
        }

        // 근접 공격 
        public NodeState MeleeAttack(CharacterInfo_Melee character)
        {
            if (!character.HasAttackChance())
            {
                return NodeState.FAILURE;
            }

            if (!isAttacking)
            {
                // 공격 시작
                isAttacking = true;
                character.ConsumeAttackChance();
                SetColor(Color.red);

                // 칼 휘두르기 (범위 공격!)
                PerformSlash();

                Debug.Log($"{character.characterName} 근접 공격! (남은 횟수: {character.HasAttackChance()})");

                // 0.5초 후 공격 종료
                Invoke(nameof(EndAttack), 0.5f);

                return NodeState.RUNNING;
            }

            return NodeState.RUNNING;
        }

        private void EndAttack()
        {
            isAttacking = false;
            SetColor(Color.white);
        }

        // 칼 휘두르기 판정
        private void PerformSlash()
        {
            Collider[] hits = Physics.OverlapSphere(transform.position, slashRange, enemyLayer);

            foreach (Collider hit in hits)
            {
                Vector3 directionToEnemy = (hit.transform.position - transform.position).normalized;
                float angle = Vector3.Angle(transform.forward, directionToEnemy);

                // 앞쪽 각도 내에만 공격 적용
                if (angle <= slashAngle / 2f)
                {
                    IDamageable damageable = hit.GetComponent<IDamageable>();
                    if (damageable != null)
                    {
                        float damage = stats.CalculateDamage();
                        damageable.TakeDamage(damage);
                        Debug.Log($"{character.characterName}가 {hit.name}에게 {damage} 데미지!");
                    }
                }
            }
        }

        // MANUAL모드- 클릭 방향으로 최대 이동거리만큼 이동
        public NodeState MoveByMaxDistance(CharacterInfo_Melee character)
        {
            if (isMoving || isAttacking) return NodeState.RUNNING;

            if (!character.IsPlayerInputExists())
                return NodeState.FAILURE;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Vector3 direction = (hit.point - character.InitialPosition).normalized;
                float distanceToClick = Vector3.Distance(character.InitialPosition, hit.point);

                // 초기 위치에서 최대 이동거리만큼만 이동
                float actualDistance = Mathf.Min(distanceToClick, character.MaxMoveDistance);
                moveTargetPos = character.InitialPosition + direction * actualDistance;

                isMoving = true;
                currentTarget = null;

                SetColor(Color.yellow);
                Debug.Log($"{character.characterName} 클릭 방향으로 이동 시작");
                character.playerInputExists = false;

                return NodeState.RUNNING;
            }

            return NodeState.FAILURE;
        }

        //  AUTO모드 - 가장 가까운 적에게 이동 공격
        public NodeState MoveToEnemy(CharacterInfo_Melee character)
        {
            if (isMoving || isAttacking) return NodeState.RUNNING;

            currentTarget = character.FindNearestEnemy();
            if (currentTarget == null)
            {
                return NodeState.FAILURE;
            }

            // 적 위치로 이동 공격 범위 고려
            Vector3 directionToEnemy = (currentTarget.position - transform.position).normalized;
            float distanceToEnemy = Vector3.Distance(transform.position, currentTarget.position);

            // 공격 범위 밖이면 이동
            if (distanceToEnemy > stats.AttackRange)
            {
                moveTargetPos = currentTarget.position - directionToEnemy * (stats.AttackRange * 0.8f);
                isMoving = true;

                SetColor(Color.blue);
                Debug.Log($"{character.characterName} 적 위치로 이동 중");
                return NodeState.RUNNING;
            }

            // 이미 공격 범위 안
            return NodeState.SUCCESS;
        }

        // 원래 위치로 복귀 (재장전)
        public NodeState Cover(CharacterInfo_Melee character)
        {
            // 이미 원래 위치면 재장전
            if (Vector3.Distance(transform.position, character.InitialPosition) < 0.5f)
            {
                if (!character.HasAttackChance())
                {
                    character.ReloadAttackChances();
                }
                SetColor(Color.cyan);
                return NodeState.SUCCESS;
            }

            // 이동 중
            if (isMoving) return NodeState.RUNNING;

            // 복귀 시작
            moveTargetPos = character.InitialPosition;
            isMoving = true;
            currentTarget = null;

            SetColor(Color.magenta);
            Debug.Log($"{character.characterName} 원래 위치로 복귀 중");

            return NodeState.RUNNING;
        }

        public NodeState Rest(CharacterInfo_Melee character)
        {
            SetColor(Color.cyan);
            return NodeState.SUCCESS;
        }

        public NodeState Retire(CharacterInfo_Melee character)
        {
            SetColor(Color.black);
            return NodeState.SUCCESS;
        }

        // 공격 범위 표시
        void OnDrawGizmos()
        {
            if (stats == null) return;

            // 칼 휘두르는 범위 (녹색 부채꼴)
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, slashRange);
        }
    }
}