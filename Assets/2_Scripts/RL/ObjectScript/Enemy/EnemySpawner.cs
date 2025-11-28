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

        private List<GameObject> spawnedEnemies = new();
        // Start is called once before the first execution of Update after the MonoBehaviour is created

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
                GameObject enemyobj = Instantiate(enemyprefab, worldPos, Quaternion.identity, roomParent);

                Enemy enemy = enemyobj.GetComponent<Enemy>();
                enemy.HpbarPrefab = hpbarPrefab;
                spawnedEnemies.Add(enemyobj);
            }

            Debug.Log($"{spawnedEnemies.Count}付府狼 利 积己 肯丰 (何葛: {roomParent.name})");
        }

    }

}