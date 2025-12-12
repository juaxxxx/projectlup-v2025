using LUP.RL;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;
namespace LUP.RL
{
    public class EnemySpawner : MonoBehaviour
    {
        [Header("StageData")]
        public StageData stageData;

        public GameObject hpbarPrefab;


        private List<Enemy> SpawnedEnemies = new();
        public List<Enemy> spawnedEnemies => SpawnedEnemies;

        private InGameCenter InGameCenter;
    
        private void Start()
        {
            InGameCenter = FindFirstObjectByType<InGameCenter>();
        }
        public void Init(StageData data)
        {
            stageData = data;
            SpawnEnemies();
        }
        void SpawnEnemies()
        {
            if(stageData == null)
            {
                Debug.Log("spawnEnemy  : null stageData");
                return;
            }

            Transform roomParent = transform.parent;

            foreach (var entry in stageData.enemySpawn)
            {
                Vector3 worldPos = GridToWorld(entry.gridPos);

                Enemy enemy = Instantiate(entry.enemy.prefab, worldPos, Quaternion.identity, roomParent.transform)
                  .GetComponent<Enemy>();

                enemy.HpbarPrefab = hpbarPrefab;
                SpawnedEnemies.Add(enemy);
                Debug.Log("spawn success");
            }
   
        }
        //ÁÂÇ¥º¯È¯
        Vector3 GridToWorld(Vector2Int grid)
        {
            return new Vector3(grid.x, 1.5f, grid.y);
        }
        private void OnEnable()
        {
            Enemy.ObjectOnEnemyDied += HandleEnemyDeath;
        }

        private void OnDisable()
        {
            Enemy.ObjectOnEnemyDied -= HandleEnemyDeath;
        }

        private void HandleEnemyDeath(Enemy enemy)
        {
            if (InGameCenter)
                InGameCenter.OnEnemyDie(enemy.transform);

            spawnedEnemies.Remove(enemy);
            Debug.Log($"{enemy.name} enemy, {spawnedEnemies.Count}");

            InGameCenter.itemSpawner.SpawnItem(enemy.transform);

            if(spawnedEnemies.Count == 0)
            {
                InGameCenter.RoomClear();
            }

        }

    }
}