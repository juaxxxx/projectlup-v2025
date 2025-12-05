using LUP.RL;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
namespace LUP.RL
{
    public class EnemySpawner : MonoBehaviour
    {
        public StageData stageData;
        public GameObject enemyprefab;
        public GameObject hpbarPrefab;
        public List<Enemy> spawnedEnemies = new();
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        //private void Awake()
        //{
        //    spawnedEnemies = new List<Enemy>();
        //}
        public void Init(StageData data)
        {
            stageData = data;
            SpawnEnemies();
        }
        void SpawnEnemies()
        {
            if (stageData == null) return;
            Transform roomParent = transform.parent;

            foreach (Vector2Int pos in stageData.enemySpawn)
            {
                Vector3 worldPos = new Vector3(pos.x, 1.5f, pos.y);
                Enemy enemy = Instantiate(enemyprefab, worldPos, Quaternion.identity, roomParent)
                  .GetComponent<Enemy>();
                enemy.HpbarPrefab = hpbarPrefab;
                spawnedEnemies.Add(enemy);
                Debug.Log($"{spawnedEnemies.Count}付府狼 利 积己 肯丰 Enemy::{spawnedEnemies.Count})");
            }

   
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

            spawnedEnemies.Remove(enemy);
            Debug.Log($"{enemy.name} enemy, {spawnedEnemies.Count}");       
        }

    }
}