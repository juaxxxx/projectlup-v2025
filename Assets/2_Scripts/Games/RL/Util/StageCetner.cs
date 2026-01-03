using LUP.RL;
using OpenCvSharp.Flann;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using TMPro.Examples;
using Unity.AI.Navigation;
using Unity.VisualScripting;
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
        private PlayerMove[] players;

        [Header("UI 연결")]
        public TextMeshProUGUI stageText;

        public GameObject enemySpawnerPrefab;
        public GameObject obstaclePrefab;
        private GameObject currentRoom;
        private PlayerBlackBoard bb;
        public UnityEvent onStageClear;
        public UnityEvent onMoveToNextRoom;
        public GridGenerator gridSystem;
        private int currentStage = 0;
        public bool GameClear = false;
        public EnemySpawner currentSpawner;

        private void Start()
        {
            FindFirstObjectByType<InGameCenter>().OnPlayerCharacterSpawned += OnPlayerCharacterSpanwed;
        }

        void OnPlayerCharacterSpanwed(GameObject playerObj)
        {
            player = playerObj.GetComponent<Transform>();
        }

        public void LoadNextRoom()
        {
            ClearPreviousRoom();

            if (!HasMoreStages())
            {
                HandleStageClear();
                return;
            }

            //if (player == null)
            //{
            //    player = FindFirstObjectByType<PlayerMove>().gameObject.GetComponent<Transform>();
            //}

            StageData data = stageData[currentStage];

            CreateRoom(data);
            UpdateStageUI();
            MovePlayerToSpawn(data);
            SpawnEnemies(data);
            SpawnObstacles(data);

            Debug.Log($"Stage {currentStage} ({data.StageName}) 로드 완료");
            currentStage++;

            onMoveToNextRoom.Invoke();
            //StartCoroutine(InvokeOnMoveToNextRoomDelayed());
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
            Debug.Log($"스테이지클리어");
            onStageClear.Invoke();
            GameClear = true;
        }


        private void CreateRoom(StageData data)
        {
            currentRoom = Instantiate(data.roomprefab, Vector3.zero, Quaternion.identity, roomParent);

            players = FindObjectsByType<PlayerMove>(FindObjectsSortMode.None);

            for(int i = 0; i < players.Length; i++)
            {
                var bb = players[i].gameObject.GetComponent<PlayerBlackBoard>();
                if (bb != null)
                    bb.SetCurrentRoom(currentRoom.transform);
            }

        }


        private void UpdateStageUI()
        {
            if (stageText != null)
                stageText.text = $"Stage {currentStage}";
        }


        private void MovePlayerToSpawn(StageData data)
        {
            var tile = gridSystem.GetTile(data.playerSpawnPoint.x, data.playerSpawnPoint.y);

            if (tile == null) return;

            Vector3 spawnPos = tile.worldPos;
            spawnPos.y = 0.5f;
            player.position = spawnPos;
        }
        private void SpawnEnemies(StageData data)
        {
            GameObject spawnerObj = Instantiate(enemySpawnerPrefab, Vector3.zero, Quaternion.identity, currentRoom.transform);
            EnemySpawner spawner = spawnerObj.GetComponent<EnemySpawner>();
            currentSpawner = spawner;   
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
            if (currentStage == 0)
            {
                return true;
            }
       
            currentSpawner.spawnedEnemies.RemoveAll(e => e == null || e.Equals(null));
            if (currentSpawner.spawnedEnemies.Count == 0)
            {
                Debug.Log($"몬스터 개수 : {currentSpawner.spawnedEnemies.Count}");
                return true;
            }
            else
            {
                //@TODO
                Debug.Log($"false {currentSpawner.spawnedEnemies.Count}");
                return false;
            }
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

        private IEnumerator InvokeOnMoveToNextRoomDelayed()
        {
            yield return new WaitForSeconds(0.1f);
            onMoveToNextRoom.Invoke();
        }

    }


}