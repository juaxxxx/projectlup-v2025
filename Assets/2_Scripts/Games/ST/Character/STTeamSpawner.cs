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

            for (int i = 0; i < 5; i++)
            {
                var data = team[i];
                if (data == null || data.prefab == null)
                {
                    Debug.LogWarning($"[STTeamSpawner] Slot {i} data/prefab null.");
                    spawnedCharacters.Add(null);
                    continue;
                }

                var sp = spawnPoints[i];
                var go = Instantiate(data.prefab, sp.position, sp.rotation);
                go.name = $"{data.name}_Slot{i}";

                spawnedCharacters.Add(go); 
            }

            Debug.Log("[STTeamSpawner] Spawn complete.");
            return spawnedCharacters;
        }
    }
}
