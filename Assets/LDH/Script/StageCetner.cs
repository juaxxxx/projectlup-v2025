using System;
using System.Collections.Generic;
using TMPro;
using TMPro.Examples;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class StageController : MonoBehaviour
{
    [Header("방 프리팹 리스트 (설계도)")]
    public List<GameObject> roomPrefabs;

    [Header("맵 위치")]
    public Transform roomParent;

    [Header("플레이어 Transform")]
    public Transform player;

    [Header("UI 연결")]
    public TextMeshProUGUI stageText;

    private GameObject currentRoom;
    private Transform spawn = null;

    public UnityEvent onStageClear;

    private int currentStage = 0;
    private int MaxStage = 5;
    public bool GameClear = false;
    public void LoadNextRoom()
    {
        //방이 하나라도  있으면
        if (roomParent.childCount > 0)
        {
            foreach (Transform child in roomParent)
            {
                Destroy(child.gameObject);
            }
        }
        //max  stage초과하면 생성 X
        if (currentStage < MaxStage)
        {
            currentRoom = Instantiate(roomPrefabs[currentStage], Vector3.zero, Quaternion.identity, roomParent);
            currentStage++;
            if (stageText != null)
            {
                Debug.Log("스테이지   UI반영");
                stageText.text = $"Stage {currentStage}";
            }
        }

        else
        {

            if (onStageClear != null)
            {
                onStageClear.Invoke();
            }


            GameClear = true;
        }

        foreach (Transform t in currentRoom.GetComponentsInChildren<Transform>(true))
        {
            if (t.name == "PlayerStart")
            {
                spawn = t;
                break;
            }
        }
        if (spawn != null)
        {
            player.position = spawn.position;
            player.rotation = spawn.rotation;

        }

    }
    public int GetStageNum()
    {
        return currentStage;
    }
    public int GetMaxStageNum()
    {
        return MaxStage;
    }
}