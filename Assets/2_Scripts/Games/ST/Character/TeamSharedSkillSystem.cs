using UnityEngine;
using System.Collections.Generic;

namespace LUP.ST
{
    public class TeamSharedSkillSystem : MonoBehaviour
    {
        [Header("Cooldowns (sec)")]
        [SerializeField] private float healCd = 20f;
        [SerializeField] private float buffCd = 30f;
        [SerializeField] private float aoeCd = 40f;

        [Header("Skill Params")]
        [SerializeField] private float healPercent = 0.5f;         
        [SerializeField] private float buffMultiplier = 1.5f;

        [Header("VFX Prefabs")]
        [SerializeField] private GameObject healVfxPrefab;   // one-shot, ally마다 1개
        [SerializeField] private GameObject buffVfxPrefab;   // duration, ally마다 1개 (10s)
        [SerializeField] private GameObject aoeVfxPrefab;    // one-shot, 맵 중앙 1개


        [Header("VFX Anchors")]
        [SerializeField] private Vector3 characterCenterOffset = Vector3.up * 1.0f; // 캐릭터 중앙 보정
        [SerializeField] private Transform mapCenterPoint; // 있으면 이걸 사용(없으면 아래 fallback)
        [SerializeField] private Vector3 mapCenterFallback = Vector3.zero;

        [Header("VFX Setting")]
        [SerializeField] private float healVfxLife = 1f;
        [SerializeField] private float aoeVfxLife = 5f; 
        [SerializeField] private float aoeVfxScale = 3.0f;

        // 팀(슬롯 0~4) 등록
        private readonly List<StatComponent> team = new List<StatComponent>(5);

        // 공용 쿨타임 종료 시각
        private float healReadyTime;
        private float buffReadyTime;
        private float aoeReadyTime;

        public void SetTeamCharacters(List<GameObject> spawnedInSlotOrder)
        {
            team.Clear();

            for (int i = 0; i < 5; i++)
            {
                StatComponent stat = null;
                if (spawnedInSlotOrder != null && i < spawnedInSlotOrder.Count && spawnedInSlotOrder[i] != null)
                    stat = spawnedInSlotOrder[i].GetComponent<StatComponent>(); // 루트

                team.Add(stat);
            }

            // 스테이지 시작 시 쿨 초기화
            healReadyTime = 0f;
            buffReadyTime = 0f;
            aoeReadyTime = 0f;
        }

        // =========================
        // 1) 힐: 아군 전체 즉시 50% 회복
        // =========================
        public bool TryHealAllies()
        {
            if (Time.time < healReadyTime) return false;

            bool applied = false;
            for (int i = 0; i < team.Count; i++)
            {
                var stat = team[i];
                if (stat == null || stat.IsDead) continue;

                stat.HealPercent(healPercent);
                applied = true;

                SpawnCenterVfxOnCharacter(healVfxPrefab, stat.transform, healVfxLife);
            }

            if (!applied) return false; // 살아있는 아군이 없으면 사용 실패 처리
            healReadyTime = Time.time + healCd;
            return true;
        }

        // =========================
        // 2) 버프: 아군 전체 10초 동안 공속/데미지 1.5배
        // =========================
        public bool TryBuffAllies()
        {
            if (Time.time < buffReadyTime) return false;

            bool applied = false;
            for (int i = 0; i < team.Count; i++)
            {
                var stat = team[i];
                if (stat == null || stat.IsDead) continue;

                stat.MultiplyAttackSpeed(buffMultiplier);
                stat.MultiplyAttackDamage(buffMultiplier);
                applied = true;

                SpawnOrRefreshBuffVfx(stat.transform);
            }

            if (!applied) return false;
            buffReadyTime = Time.time + buffCd;
            return true;
        }

        // =========================
        // 3) 광역: 맵의 모든 적에게 각 적 MaxHealth 50% 데미지
        // =========================
        public bool TryAoeAllEnemies()
        {
            if (Time.time < aoeReadyTime) return false;

            var enemies = GameObject.FindGameObjectsWithTag("Enemy"); // "맵에 존재하는 모든 적"
            bool applied = false;

            foreach (var e in enemies)
            {
                if (e == null) continue;

                var enemyStat = e.GetComponent<StatComponent>();
                if (enemyStat == null || enemyStat.IsDead) continue;

                enemyStat.TakeDamage(enemyStat.MaxHealth * 0.5f);
                applied = true;
            }

            // 적이 없으면 쿨을 돌릴지 말지는 정책임. 보통은 "적이 없으면 실패"가 UX 좋음.
            if (!applied) return false;

            SpawnMapCenterVfx();

            aoeReadyTime = Time.time + aoeCd;
            return true;
        }

        // UI용: 0=사용 가능, 1=쿨 진행 비율(남은시간/cd)
        public float GetHealCd01() => Cd01(healReadyTime, healCd);
        public float GetBuffCd01() => Cd01(buffReadyTime, buffCd);
        public float GetAoeCd01() => Cd01(aoeReadyTime, aoeCd);

        public float GetHealRemain() => Mathf.Max(0f, healReadyTime - Time.time);
        public float GetBuffRemain() => Mathf.Max(0f, buffReadyTime - Time.time);
        public float GetAoeRemain() => Mathf.Max(0f, aoeReadyTime - Time.time);

        private static float Cd01(float readyTime, float cd)
        {
            if (cd <= 0f) return 0f;
            float remain = readyTime - Time.time;
            return Mathf.Clamp01(remain / cd);
        }

        private void SpawnCenterVfxOnCharacter(GameObject prefab, Transform character, float life)
        {
            if (prefab == null || character == null) return;

            var vfx = Instantiate(prefab,
                character.position + characterCenterOffset,
                Quaternion.identity,
                character); // 따라다니게

            if (life > 0f) Destroy(vfx, life);
        }

        public class BuffVfxMarker : MonoBehaviour { }
        private void SpawnOrRefreshBuffVfx(Transform character)
        {
            if (buffVfxPrefab == null || character == null) return;

            // 기존 버프 VFX 있으면 제거
            var existing = character.GetComponentInChildren<BuffVfxMarker>();
            if (existing != null)
                Destroy(existing.gameObject);

            // 새 버프 VFX 생성 (캐릭터 중앙)
            var vfx = Instantiate(
                buffVfxPrefab,
                character.position + characterCenterOffset,
                Quaternion.identity,
                character
            );

            vfx.AddComponent<BuffVfxMarker>();

            // 버프 지속시간 10초
            Destroy(vfx, 10f);
        }

        private void SpawnMapCenterVfx()
        {
            if (aoeVfxPrefab == null) return;

            Vector3 pos = mapCenterPoint != null
                ? mapCenterPoint.position
                : mapCenterFallback;

            var vfx = Instantiate(aoeVfxPrefab, pos, Quaternion.identity);

            vfx.transform.localScale = Vector3.one * aoeVfxScale;

            if (aoeVfxLife > 0f)
                Destroy(vfx, aoeVfxLife);
        }
    }
}
