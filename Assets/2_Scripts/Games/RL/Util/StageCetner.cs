using LUP.RL;
using OpenCvSharp.Flann;
using System;
using System.Collections.Generic;
using TMPro;
using TMPro.Examples;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
namespace LUP.RL
{

    public class StageController : MonoBehaviour
    {
        [SerializeField]
        public List<StageData> stageData = new();


        [Header("맵 위치")]
        public Transform roomParent;

        [Header("플레이어 Transform")]
        public Transform player;

        [Header("UI 연결")]
        public TextMeshProUGUI stageText;

        public GameObject enemySpawnerPrefab;
        public GameObject obstaclePrefab;
        private GameObject currentRoom;
        private PlayerBlackBoard bb;
        public UnityEvent onStageClear;
        public GridGenerator gridSystem;
        private int currentStage = 0;
        public bool GameClear = false;
        private EnemySpawner currentSpawner;

        public void Start()
        {
        
        }
        public void LoadNextRoom()
        {
            ClearPreviousRoom();

            if (!HasMoreStages())
            {
                HandleStageClear();
                return;
            }

            if (player == null) return;

            StageData data = stageData[currentStage];

            CreateRoom(data);
            UpdateStageUI();
            MovePlayerToSpawn(data);
            SpawnEnemies(data);
            SpawnObstacles(data);

            Debug.Log($"Stage {currentStage} ({data.StageName}) 로드 완료");
            currentStage++;
        }
        private void ClearPreviousRoom()
        {
            if (roomParent.childCount == 0) return;

            foreach (Transform child in roomParent)
            {
                Destroy(child.gameObject);
            }
        }


        private void HandleStageClear()
        {
            onStageClear.Invoke();
            GameClear = true;
        }


        private void CreateRoom(StageData data)
        {
            currentRoom = Instantiate(data.roomprefab, Vector3.zero, Quaternion.identity, roomParent);

            var bb = player.GetComponent<PlayerBlackBoard>();
            if (bb != null)
                bb.SetCurrentRoom(currentRoom.transform);
        }


        private void UpdateStageUI()
        {
            if (stageText != null)
                stageText.text = $"Stage {currentStage}";
        }


        private void MovePlayerToSpawn(StageData data)
        {
            var tile = gridSystem.GetTile(data.playerSpawn.x, data.playerSpawn.y);
            if (tile == null) return;

            Vector3 spawnPos = tile.worldPos;
            spawnPos.y = 1.5f;
            player.position = spawnPos;
        }
        private void SpawnEnemies(StageData data)
        {
            GameObject spawnerObj = Instantiate(enemySpawnerPrefab, Vector3.zero, Quaternion.identity, currentRoom.transform);
            EnemySpawner spawner = spawnerObj.GetComponent<EnemySpawner>();

            spawner.Init(data);
        }
        private void SpawnObstacles(StageData data)
        {
            foreach (var pos in data.obstacles)
            {
                var t = gridSystem.GetTile(pos.x, pos.y);
                if (t == null) continue;

                Vector3 spawnPos = t.worldPos + Vector3.up * 1.3f;
                Instantiate(obstaclePrefab, spawnPos, Quaternion.identity, currentRoom.transform);
            }
        }
        public bool IsCurrentRoomCleared()
        {
            if (currentSpawner == null)
                return true;
            // Destroy이 된 Enemy들이 null로 남아있을 수 있기때문에.
            currentSpawner.spawnedEnemies.RemoveAll(e => e == null);

            return currentSpawner.spawnedEnemies.Count == 0;
        }
        public int GetStageNum()
        {
            return currentStage;
        }
        private bool HasMoreStages()
        {
            return currentStage < stageData.Count;
        }
        public int GetMaxStageNum()
        {
            return stageData.Count;
        }
    }
}