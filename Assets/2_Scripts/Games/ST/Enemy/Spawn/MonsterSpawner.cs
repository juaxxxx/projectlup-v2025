using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace LUP.ST
{

    public class MonsterSpawner : MonoBehaviour
    {
        [Header("몬스터 설정")]
        [SerializeField] private MonsterData[] monsterPrefabs;
        [SerializeField] private int poolSize = 20;

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
        private int aliveMonsterCount = 0;

        public int CurrentWave => currentWaveIndex;
        public int AliveMonsters => aliveMonsterCount;
        public bool IsSpawning => isSpawning;

        // 이벤트
        public System.Action<int> OnWaveStart;      // 웨이브 번호
        public System.Action<int> OnWaveComplete;   // 웨이브 번호
        public System.Action OnAllWavesComplete;

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

                yield return StartCoroutine(SpawnWave(wave));

                currentWaveIndex++;

                if (currentWaveIndex < waves.Count)
                {
                    yield return new WaitForSeconds(wave.delayBeforeNextWave);
                }
            }
        }
        private IEnumerator SpawnWave(WaveData wave)
        {
            isSpawning = true;

            if (wave.useRandomSpawn)
            {
                // 랜덤 스폰 모드
                for (int i = 0; i < wave.randomSpawnCount; i++)
                {
                    MonsterData prefab = wave.GetRandomMonster();
                    if (prefab != null)
                    {
                        SpawnMonster(prefab);
                    }
                    yield return new WaitForSeconds(wave.spawnInterval);
                }
            }
            else
            {
                // 순차 스폰 모드
                foreach (var (prefab, delay) in wave.GetSpawnSequence())
                {
                    if (delay > 0)
                    {
                        yield return new WaitForSeconds(delay);
                    }

                    SpawnMonster(prefab);

                    yield return new WaitForSeconds(wave.spawnInterval);
                }
            }

            isSpawning = false;
        }

        /// <summary>
        /// 특정 프리팹으로 몬스터 스폰
        /// </summary>
        public void SpawnMonster(MonsterData prefab)
        {
            if (prefab == null)
            {
                Debug.LogError("스폰할 몬스터 프리팹이 null입니다!");
                return;
            }

            if (!monsterPools.ContainsKey(prefab))
            {
                Debug.LogError($"{prefab.name}의 Pool이 없습니다! monsterPrefabs에 추가하세요.");
                return;
            }

            Vector3 spawnPosition;
            Quaternion spawnRotation;
            GetSpawnTransform(out spawnPosition, out spawnRotation);

            ObjectPool<MonsterData> pool = monsterPools[prefab];
            MonsterData monster = pool.Get(spawnPosition, spawnRotation);
            monster.SetSpawner(this);

            SetMonsterStats(monster);

            aliveMonsterCount++;

            if (showDebugLogs)
                Debug.Log($"스폰: {monster.name} at {spawnPosition}");
        }

        /// <summary>
        /// 랜덤 몬스터 스폰 (기존 방식 호환)
        /// </summary>
        public void SpawnRandomMonster()
        {
            if (monsterPrefabs.Length == 0)
            {
                Debug.LogError("MonsterPrefabs가 없습니다!");
                return;
            }

            MonsterData randomPrefab = monsterPrefabs[Random.Range(0, monsterPrefabs.Length)];
            SpawnMonster(randomPrefab);
        }

        private void GetSpawnTransform(out Vector3 position, out Quaternion rotation)
        {
            if (spawnMode == SpawnMode.Point)
            {
                if (spawnPoints.Count == 0)
                {
                    Debug.LogError("SpawnPoint가 없습니다!");
                    position = transform.position;
                    rotation = Quaternion.identity;
                    return;
                }

                SpawnPoint spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];
                position = spawnPoint.GetSpawnPosition();
                rotation = spawnPoint.GetSpawnRotation();
            }
            else
            {
                if (spawnAreas.Count == 0)
                {
                    Debug.LogError("SpawnArea가 없습니다!");
                    position = transform.position;
                    rotation = Quaternion.identity;
                    return;
                }

                SpawnArea spawnArea = spawnAreas[Random.Range(0, spawnAreas.Count)];
                position = spawnArea.GetRandomPosition();
                rotation = spawnArea.GetSpawnRotation();
            }
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
                    OnMonsterDeath();
                    return;
                }
            }
            
            OnMonsterDeath();
            monster.gameObject.SetActive(false);
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
        [ContextMenu("Skip To Next Wave")]
        public void SkipToNextWave()
        {
            ClearAllMonsters();
            if (currentWaveIndex < waves.Count)
            {
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
        private float GetDifficultyMultiplier()
        {
            /*
            int totalLevel = 0;

            // 1. 세이브 데이터에서 팀원들의 레벨을 모두 더함
            if (STSaveHandler.CurrentData != null && STSaveHandler.CurrentData.characterList != null)
            {
                foreach (var charData in STSaveHandler.CurrentData.characterList)
                {
                    totalLevel += charData.level;
                }
            }

            // 2. 만약 데이터가 없어서 0이라면 기본값 5(모두 1렙)로 설정
            if (totalLevel < 5) totalLevel = 5;

            // 3. 공식: 레벨 합 10이 기준(1.0)이므로 0.1을 곱함
            // 합이 5(시작 시) -> 0.5배
            // 합이 10(평균 2렙) -> 1.0배
            // 합이 50(평균 10렙) -> 5.0배
            return totalLevel * 0.1f;*/
            return 1.0f; // 일단 고정값으로
        }

        // 몬스터를 실제로 생성(또는 풀에서 꺼낼 때) 호출하는 부분
        private void SetMonsterStats(MonsterData monster)
        {
            float multiplier = GetDifficultyMultiplier();

            var stats = monster.GetComponent<StatComponent>();
            if (stats != null)
            {
                // 속성에 직접 대입하는 대신, 함수를 호출해서 한 번에 해결!
                stats.ScaleStats(multiplier);

                if (showDebugLogs)
                    Debug.Log($"[Spawner] {monster.name} 난이도 적용: {multiplier}배");
            }
        }
        [System.Obsolete]
        public void OnMonsterDeath()
        {
            aliveMonsterCount--;

            // 체크: 모든 웨이브가 끝났고, 남은 몬스터가 0마리라면?
            if (currentWaveIndex >= waves.Count && aliveMonsterCount <= 0)
            {
                Object.FindAnyObjectByType<SceneChanger>().LoadResultScene();
            }
        }
        void OnGUI()
        {
            if (showDebugLogs && monsterPools != null)
            {
                int y = 10;

                // 웨이브 정보
                string waveInfo = currentWaveIndex < waves.Count
                    ? $"Wave: {currentWaveIndex + 1}/{waves.Count} - {waves[currentWaveIndex].waveName}"
                    : "All Waves Complete!";
                GUI.Label(new Rect(10, y, 400, 20), waveInfo);
                y += 20;

                GUI.Label(new Rect(10, y, 300, 20), $"Alive Monsters: {aliveMonsterCount}");
                y += 25;

                // 풀 정보
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