using System.Collections.Generic;
using UnityEngine;
namespace LUP.ST
{

    [System.Serializable]
    public class WaveMonsterEntry
    {
        [Tooltip("스폰할 몬스터 프리팹")]
        public MonsterData prefab;

        [Tooltip("스폰할 수량")]
        public int count = 1;

        [Tooltip("스폰 딜레이 (이전 몬스터 스폰 후 대기 시간)")]
        public float spawnDelay = 0f;

    }
    [System.Serializable]
    public class WaveData
    {
        public string waveName = "Wave 1";
        public int monsterCount = 5;           // 이 웨이브에서 스폰할 몬스터 수
        public float spawnInterval = 2f;       // 몬스터 스폰 간격 (초)
        public float delayBeforeNextWave = 5f; // 다음 웨이브까지 대기 시간
        public List<WaveMonsterEntry> monsters = new List<WaveMonsterEntry>();  // 이 웨이브에서 스폰할 몬스터 목록
        public bool useRandomSpawn = false; // 랜덤 스폰 여부
        public int randomSpawnCount = 5;      // 랜덤 스폰 시 스폰할 몬스터 수
    

     public int TotalMonsterCount
        {
            get
            {
                if (useRandomSpawn) return randomSpawnCount;

                int total = 0;
                foreach (var entry in monsters)
                {
                    total += entry.count;
                }
                return total;
            }
        }

        /// <summary>
        /// 스폰 순서대로 몬스터 엔트리 반환 (순차 스폰용)
        /// </summary>
        public IEnumerable<(MonsterData prefab, float delay)> GetSpawnSequence()
        {
            foreach (var entry in monsters)
            {
                for (int i = 0; i < entry.count; i++)
                {
                    float delay = (i == 0) ? entry.spawnDelay : spawnInterval;
                    yield return (entry.prefab, delay);
                }
            }
        }

        /// <summary>
        /// 랜덤으로 몬스터 선택 (랜덤 스폰용)
        /// </summary>
        public MonsterData GetRandomMonster()
        {
            if (monsters.Count == 0) return null;

            // 가중치 기반 랜덤 (count가 높으면 더 자주 등장)
            int totalWeight = 0;
            foreach (var entry in monsters)
            {
                totalWeight += entry.count;
            }

            int randomValue = Random.Range(0, totalWeight);
            int currentWeight = 0;

            foreach (var entry in monsters)
            {
                currentWeight += entry.count;
                if (randomValue < currentWeight)
                {
                    return entry.prefab;
                }
            }

            return monsters[0].prefab;
        }
    }
}