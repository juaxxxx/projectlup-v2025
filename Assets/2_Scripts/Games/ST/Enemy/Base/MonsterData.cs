
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LUP.ST
{

    public class MonsterData : MonoBehaviour, IPoolable
    {
        [Header("몬스터 타입")]
        public MonsterType monsterType = MonsterType.Grunt;

        [Header("드롭")]
        public DropTable dropTable;
        public GameObject dropPrefab;

        [Header("타겟")]
        public Transform target;
        public float retargetInterval = 3f;
        private float lastRetargetTime = 0f;

        [Header("원거리 공격 (선택)")]
        public GameObject bulletPrefab;
        public Transform firePoint;

        [Header("상태 지속시간")]
        public float stunDuration = 2f;
        public float skillDuration = 2f;

        [Header("사망 설정")]
        public float deathAnimationDuration = 2f;  // 죽는 애니메이션 길이
        private bool isDying = false;  // 중복 호출 방지

        // 상태 플래그
        [HideInInspector] public bool isStunned = false;
        [HideInInspector] public float stunEndTime = 0f;
        [HideInInspector] public bool isUsingSkill = false;
        [HideInInspector] public float skillEndTime = 0f;
        //[HideInInspector] public bool isHiding = false;
        [HideInInspector] public Vector3 hidePosition;
        [HideInInspector] public bool isAttackingFlag = false;
        [HideInInspector] public bool hasAppliedHit = false;

        private MonsterSpawner spawner;
        private StatComponent stats;
        private VisualComponent visual;
        private Renderer rend;
        private Color originalColor;

        void Awake()
        {
            stats = GetComponent<StatComponent>();
            visual = GetComponent<VisualComponent>();
            if (stats == null)
            {
                Debug.LogError($"{gameObject.name}: StatComponent가 없습니다!");
            }
            else
            {
                stats.OnDeath += HandleDeath;
            }


            rend = GetComponent<Renderer>();
            if (rend != null)
            {
                originalColor = rend.material.color;
            }
        }

        void OnDestroy()
        {
            if (stats != null)
            {
                stats.OnDeath -= HandleDeath;
            }
        }

        void Update()
        {
            //주기적으로 타겟 재선택
            if (Time.time - lastRetargetTime >= retargetInterval)
            {
                FindNearestPlayer();
                lastRetargetTime = Time.time;
            }
            //타겟 없어지면 바로 다른 타겟 잡기
            if (target != null)
            {
                StatComponent targetStats = target.GetComponent<StatComponent>();
                if (targetStats != null && targetStats.IsDead)
                {
                    Debug.Log($"{gameObject.name}: 타겟 {target.name} 사망! 다른 타겟 찾기...");
                    FindNearestPlayer();
                }
            }
        }

        public void OnSpawn()
        {
            isStunned = false;
            isUsingSkill = false;
            //isHiding = false;
            lastRetargetTime = 0f;
            isDying = false;

            // 체력바 리셋
            var healthBar = GetComponent<MonsterHealthBar>();
            if (healthBar != null)
            {
                healthBar.ResetHealthBar();
            }

            var bt = GetComponent<MonsterBTBase>();
            if (bt != null) bt.enabled = true;

            Visual?.ResetDeathAnimation();

            if (stats != null)
            {
                stats.ResetStats();
            }

            ResetColor();

            FindNearestPlayer();
        }

        public void OnDespawn()
        {
        }

        public void SetSpawner(MonsterSpawner spawner)
        {
            this.spawner = spawner;
        }

        private void HandleDeath()
        {
            Die();
        }

        public void Die()
        {
            //DropRewards();
            if (isDying) return;  // 이미 죽는 중이면 무시

            StartCoroutine(DieCoroutine());
            if (spawner != null)
            {
                spawner.ReturnToPool(this);
            }
            else
            {
                // 만약 스패너 참조가 없다면 그냥 비활성화
                gameObject.SetActive(false);
            }
        }

        private IEnumerator DieCoroutine()
        {
            isDying = true;

            // 죽음 애니메이션 재생
            Visual?.PlayDeathAnimation();

            // BT 멈추기 (더 이상 행동 안 하게)
            var bt = GetComponent<MonsterBTBase>();
            if (bt != null) bt.enabled = false;

            // 애니메이션 대기
            yield return new WaitForSeconds(deathAnimationDuration);

            // 드롭 및 풀 반환
            DropRewards();

            if (spawner != null)
            {
                spawner.ReturnToPool(this);
            }
            else
            {
                gameObject.SetActive(false);
            }
        }

        public void SetColor(Color color)
        {
            if (rend != null)
            {
                rend.material.color = color;
            }
        }

        public void ResetColor()
        {
            if (rend != null)
            {
                rend.material.color = originalColor;
            }
        }

        public StatComponent Stats => stats;
        public VisualComponent Visual => visual;
        public bool IsDead => stats != null && stats.IsDead;

        public void ApplyStun()
        {
            if (!IsDead)
            {
                isStunned = true;
                stunEndTime = Time.time + stunDuration;

                if (stats != null)
                {
                    stats.CancelAttack();
                }
            }
        }

        public void UseSkill()
        {
            if (!IsDead && !isUsingSkill)
            {
                isUsingSkill = true;
                skillEndTime = Time.time + skillDuration;
            }
        }

        private void FindNearestPlayer()
        {
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

            if (players.Length == 0) return;
            if (players.Length == 1)
            {
                StatComponent stats = players[0].GetComponent<StatComponent>();
                if (stats != null && stats.IsDead)
                {
                    target = null;
                    return;
                }
                target = players[0].transform;
                return;
            }

            Transform nearest = null;
            float minDistance = float.MaxValue;

            foreach (GameObject player in players)
            {
                StatComponent playerStats = player.GetComponent<StatComponent>();
                if (playerStats != null && playerStats.IsDead)
                {
                    continue;  // 죽었으면 스킵!
                }

                float distance = Vector3.Distance(transform.position, player.transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearest = player.transform;
                }
            }

            if (nearest == null)
            {
                target = null;
                Debug.Log($"{gameObject.name}: 살아있는 플레이어가 없습니다!");
            }
            else
            {
                target = nearest;
            }
        }
        private void DropRewards()
        {
            if (dropTable == null || dropPrefab == null) return;

            Vector3 dropPosition = transform.position;

            List<DropTable.DropItem> drops = dropTable.GetDrops();

            foreach (DropTable.DropItem drop in drops)
            {
                GameObject dropObj = Instantiate(dropPrefab, dropPosition, Quaternion.identity);
                ItemDrop itemDrop = dropObj.GetComponent<ItemDrop>();

                if (itemDrop != null)
                {
                    itemDrop.itemData = drop.item;
                    itemDrop.goldAmount = drop.goldAmount;
                }
            }
        }

        void OnDrawGizmos()
        {
            if (target != null && gameObject.activeSelf && stats != null)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawLine(transform.position, target.position);

                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(transform.position, stats.AttackRange);


            }
        }
    }
}