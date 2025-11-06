using OpenCvSharp.Flann;
using System;
using System.Collections.Generic;
using TMPro;
using TMPro.Examples;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
namespace RL
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

        public UnityEvent onStageClear;
        public GridGenerator gridSystem;
        private int currentStage = 0;
        public bool GameClear = false;
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
                StageData data = stageData[currentStage];
                currentRoom = Instantiate(data.roomprefab, Vector3.zero, Quaternion.identity, roomParent);

                //UI 갱신
                if (stageText != null)
                {
                    stageText.text = $"Stage {currentStage}";
                }
                //플레이어 찾기
                var tile = gridSystem.GetTile(data.playerSpawn.x, data.playerSpawn.y);
                if (tile != null)
                    player.position = tile.worldPos;

                //몬스터 스포너  
                foreach (var pos in data.enemySpawn)
                {
                    var t = gridSystem.GetTile(pos.x, pos.y);
                    Instantiate(enemySpawnerPrefab, t.worldPos, Quaternion.identity, currentRoom.transform);
                }
                //장애물 배치
                foreach (var pos in data.obstacles)
                {
                    var t = gridSystem.GetTile(pos.x, pos.y);
                    Instantiate(obstaclePrefab, t.worldPos, Quaternion.identity, currentRoom.transform);
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