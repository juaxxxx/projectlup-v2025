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

        public void Start()
        {
            bb = player.GetComponent<PlayerBlackBoard>();
            if (bb == null)
            {
                return;
            }
            bb.Initialize(player.gameObject);
        }
        public void LoadNextRoom()
        {
            //방이 하나라도  있으면 다  삭제
            if (roomParent.childCount > 0)
            {
                foreach (Transform child in roomParent)
                {
                    Destroy(child.gameObject);
                }
            }

            if (currentStage < stageData.Count)
            {
                if (player == null) return;
                StageData data = stageData[currentStage];
                currentRoom = Instantiate(data.roomprefab, Vector3.zero, Quaternion.identity, roomParent);
                var bb = player.GetComponent<PlayerBlackBoard>();
                if(bb != null)
                bb.SetCurrentRoom(currentRoom.transform);
                //UI 갱신
                if (stageText != null)
                {
                    stageText.text = $"Stage {currentStage}";
                }

                //플레이어 찾기
                var tile = gridSystem.GetTile(data.playerSpawn.x, data.playerSpawn.y);
                if (tile != null)
                {
                    Vector3 spawnPos = tile.worldPos;
                    spawnPos.y = 1.5f;
                    player.position = spawnPos;
                }
                GameObject spawnerObj = Instantiate(enemySpawnerPrefab, Vector3.zero, Quaternion.identity, currentRoom.transform);

                EnemySpawner spawner = spawnerObj.GetComponent<EnemySpawner>();

                spawner.Init(data);    // 여기서 StageData 전달!

                //장애물 배치
                foreach (var pos in data.obstacles)
                {

                    var t = gridSystem.GetTile(pos.x, pos.y);
                    Vector3 spawnPos = t.worldPos + Vector3.up * 1.3f;
                    Instantiate(obstaclePrefab, spawnPos, Quaternion.identity, currentRoom.transform);
                }
                Debug.Log($" Stage {currentStage} ({data.StageName}) 로드 완료");
                currentStage++;
            }
            else
            {
                onStageClear.Invoke();
                GameClear = true;
            }

        }
        public int GetStageNum()
        {
            return currentStage;
        }
        public int GetMaxStageNum()
        {
            return stageData.Count;
        }
    }
}