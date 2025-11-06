using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
namespace RL
{
    public class EnemySpawner : MonoBehaviour
    {
        public StageData stageData;
        [SerializeField]
        public GameObject enemyprefab;


        private List<GameObject> spawnedEnemies = new();
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {

            SpawnEnemies();
        }
        void SpawnEnemies()
        {
            if (stageData == null)
            {
                Debug.LogError("StageData가 연결되지 않았습니다!");
                return;
            }

            Transform roomParent = transform.parent;

            foreach (Vector2Int pos in stageData.enemySpawn)
            {
                Vector3 worldPos = new Vector3(pos.x, 1.5f, pos.y);

                GameObject enemy = Instantiate(enemyprefab, worldPos, Quaternion.identity, roomParent);
                spawnedEnemies.Add(enemy);
            }

            Debug.Log($"{spawnedEnemies.Count}마리의 적 생성 완료 (부모: {roomParent.name})");
        }

    }

}