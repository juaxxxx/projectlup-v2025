using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace LUP.ST
{

    public class MonsterSpawner : MonoBehaviour
    {
        [Header("몬스터 설정")]
        [SerializeField] private MonsterData[] monsterPrefabs;
        [SerializeField] private int poolSize = 50;

        [Header("스폰 방식 선택")]
        [SerializeField] private SpawnMode spawnMode = SpawnMode.Area;

        [Header("스폰 포인트 (Point 모드)")]
        [SerializeField] private List<SpawnPoint> spawnPoints = new List<SpawnPoint>();

        [Header("스폰 영역 (Area 모드)")]
        [SerializeField] private List<SpawnArea> spawnAreas = new List<SpawnArea>();

        [Header("웨이브 설정")]
        [SerializeField] private List<WaveData> waves = new List<WaveData>();
        [SerializeField] private bool autoStartWaves = true;

        [Header("디버그")]
        [SerializeField] private bool showDebugLogs = true;

        private Dictionary<MonsterData, ObjectPool<MonsterData>> monsterPools = new Dictionary<MonsterData, ObjectPool<MonsterData>>();

        private int currentWaveIndex = 0;
        private bool isSpawning = false;

        public enum SpawnMode
        {
            Point,
            Area
        }

        void Start()
        {
            InitializePool();

            if (autoStartWaves && waves.Count > 0)
            {
                StartCoroutine(StartWaves());
            }
        }

        private void InitializePool()
        {
            if (monsterPrefabs == null || monsterPrefabs.Length == 0)
            {
                Debug.LogError("MonsterPrefabs가 설정되지 않았습니다!");
                return;
            }

            foreach (MonsterData prefab in monsterPrefabs)
            {
                if (prefab == null) continue;

                GameObject poolParent = new GameObject($"{prefab.name}_Pool");
                poolParent.transform.SetParent(transform);

                ObjectPool<MonsterData> pool = new ObjectPool<MonsterData>(prefab, poolSize / monsterPrefabs.Length, poolParent.transform);
                monsterPools[prefab] = pool;

                if (showDebugLogs)
                    Debug.Log($"{prefab.name} Pool 초기화: {poolSize / monsterPrefabs.Length}개");
            }
        }

        private IEnumerator StartWaves()
        {
            while (currentWaveIndex < waves.Count)
            {
                WaveData wave = waves[currentWaveIndex];

                if (showDebugLogs)
                    Debug.Log($"=== {wave.waveName} 시작! ===");

                yield return StartCoroutine(SpawnWave(wave));

                currentWaveIndex++;

                if (currentWaveIndex < waves.Count)
                {
                    if (showDebugLogs)
                        Debug.Log($"다음 웨이브까지 {wave.delayBeforeNextWave}초 대기...");

                    yield return new WaitForSeconds(wave.delayBeforeNextWave);
                }
            }

            if (showDebugLogs)
                Debug.Log("=== 모든 웨이브 완료! ===");
        }

        private IEnumerator SpawnWave(WaveData wave)
        {
            isSpawning = true;

            for (int i = 0; i < wave.monsterCount; i++)
            {
                SpawnMonster();
                yield return new WaitForSeconds(wave.spawnInterval);
            }

            isSpawning = false;
        }

        public void SpawnMonster()
        {
            if (monsterPrefabs.Length == 0)
            {
                Debug.LogError("MonsterPrefabs가 없습니다!");
                return;
            }

            Vector3 spawnPosition;
            Quaternion spawnRotation;

            if (spawnMode == SpawnMode.Point)
            {
                if (spawnPoints.Count == 0)
                {
                    Debug.LogError("SpawnPoint가 없습니다!");
                    return;
                }

                SpawnPoint spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];
                spawnPosition = spawnPoint.GetSpawnPosition();
                spawnRotation = spawnPoint.GetSpawnRotation();
            }
            else
            {
                if (spawnAreas.Count == 0)
                {
                    Debug.LogError("SpawnArea가 없습니다!");
                    return;
                }

                SpawnArea spawnArea = spawnAreas[Random.Range(0, spawnAreas.Count)];
                spawnPosition = spawnArea.GetRandomPosition();
                spawnRotation = spawnArea.GetSpawnRotation();
            }

            MonsterData randomPrefab = monsterPrefabs[Random.Range(0, monsterPrefabs.Length)];
            ObjectPool<MonsterData> pool = monsterPools[randomPrefab];

            MonsterData monster = pool.Get(spawnPosition, spawnRotation);
            monster.SetSpawner(this);
        }

        public void ReturnToPool(MonsterData monster)
        {

            foreach (var kvp in monsterPools)
            {
                if (monster.name.StartsWith(kvp.Key.name))
                {
                    kvp.Value.Return(monster);
                    return;
                }
            }

            Debug.LogWarning($"몬스터 {monster.name}의 Pool을 찾을 수 없습니다!");
        }

        public void ClearAllMonsters()
        {
            foreach (var pool in monsterPools.Values)
            {
                pool.ReturnAll();
            }
        }
        [ContextMenu("Start Next Wave")]
        public void StartNextWave()
        {
            if (!isSpawning && currentWaveIndex < waves.Count)
            {
                StartCoroutine(SpawnWave(waves[currentWaveIndex]));
                currentWaveIndex++;
            }
        }
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SpawnMonster();
            }

            if (Input.GetKeyDown(KeyCode.C))
            {
                ClearAllMonsters();
            }
        }
        void OnGUI()
        {
            if (showDebugLogs && monsterPools != null)
            {
                int y = 10;
                int totalActive = 0;
                int totalAvailable = 0;
                int totalCount = 0;

                foreach (var kvp in monsterPools)
                {
                    ObjectPool<MonsterData> pool = kvp.Value;
                    GUI.Label(new Rect(10, y, 300, 20),
                        $"{kvp.Key.name} - Active: {pool.ActiveCount} | Available: {pool.AvailableCount}");

                    totalActive += pool.ActiveCount;
                    totalAvailable += pool.AvailableCount;
                    totalCount += pool.TotalCount;

                    y += 20;
                }

                GUI.Label(new Rect(10, y, 300, 20),
                    $"Total - {totalCount} | Active: {totalActive} | Available: {totalAvailable}");
            }
        }
    }
}