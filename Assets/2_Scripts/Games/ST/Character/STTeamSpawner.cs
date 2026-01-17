using System.Collections.Generic;
using UnityEngine;

namespace LUP.ST
{
    public class STTeamSpawner : MonoBehaviour
    {
        [SerializeField] private Transform[] spawnPoints; // 5개
        private bool spawned = false;

        public List<GameObject> Spawn(ShootingRuntimeData srd)
        {
            var spawnedCharacters = new List<GameObject>();

            if (spawned)
            {
                Debug.LogWarning("[STTeamSpawner] Already spawned.");
                return spawnedCharacters;
            }
            spawned = true;

            if (srd == null)
            {
                Debug.LogError("[STTeamSpawner] ShootingRuntimeData is null.");
                return spawnedCharacters;
            }

            if (spawnPoints == null || spawnPoints.Length < 5)
            {
                Debug.LogError("[STTeamSpawner] spawnPoints must have 5 elements.");
                return spawnedCharacters;
            }

            var team = srd.Team; // STCharacterData[5]
            if (team == null || team.Length < 5)
            {
                Debug.LogError("[STTeamSpawner] Team array missing/invalid.");
                return spawnedCharacters;
            }

            // GameResult에 출전 캐릭터 ID 저장
            GameResult.ParticipatingCharacterIds.Clear();

            for (int i = 0; i < 5; i++)
            {
                var data = team[i];

                // 빈 슬롯이면 null 추가하고 넘어가기
                if (data == null || data.prefab == null)
                {
                    Debug.Log($"[STTeamSpawner] Slot {i} is empty.");
                    spawnedCharacters.Add(null);
                    continue;
                }

                // 출전 캐릭터 ID 저장
                GameResult.ParticipatingCharacterIds.Add(data.characterId);

                var sp = spawnPoints[i];
                var go = Instantiate(data.prefab, sp.position, sp.rotation);
                go.name = $"{data.name}_Slot{i}";

                // 레벨 적용
                int characterLevel = srd.GetCharacterLevel(data.characterId);
                Debug.Log($"[STTeamSpawner] Slot {i}: {data.characterName}, Lv.{characterLevel}");

                var stats = go.GetComponent<StatComponent>();
                if (stats != null)
                {
                    stats.ApplyLevelStats(characterLevel);
                }

                spawnedCharacters.Add(go);
            }

            Debug.Log($"[STTeamSpawner] Spawn complete. 출전 캐릭터: {GameResult.ParticipatingCharacterIds.Count}명");
            return spawnedCharacters;
        }
    }
}