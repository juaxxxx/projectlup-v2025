using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using TMPro; //@TODO : TMP 폰트로 변경
using System;


namespace LUP.PCR
{
    public class TaskAssignmentView : MonoBehaviour
    {
        [Header("UI 컨테이너")]
        [SerializeField] private Transform buildingListContent;
        [SerializeField] private Transform workerListContent;
        [SerializeField] private Text statusText;

        [Header("프리팹")]
        [SerializeField] BuildingUIItem buildingItemPrefab;
        [SerializeField] WorkerUIItem workerItemPrefab;

        [Header("아이콘")]
        [SerializeField] private Sprite iconHungry;
        [SerializeField] private Sprite iconIdle;

        public event Action<ProductableBuilding> OnBuildingClick;
        public event Action<WorkerAI> OnWorkerClick;

        public void UpdateStatusText(string message)
        {
            if(statusText != null)
            {
                statusText.text = message;
            }
        }

        public void RenderBuildingList(List<ProductableBuilding> buildings)
        {
            ClearContent(buildingListContent);

            for(int i = 0; i < buildings.Count; i++)
            {
                ProductableBuilding building = buildings[i];
                BuildingUIItem item = Instantiate(buildingItemPrefab, buildingListContent);

                item.Setup(building, () => OnBuildingClick?.Invoke(building));
            }
        }

        public void RenderWorkerList(List<WorkerAI> workers)
        {
            Debug.Log($"[View] 버튼 생성 시작: {workers.Count}개 생성 예정");
            ClearContent(workerListContent);

            for (int i = 0; i < workers.Count; i++)
            {
                WorkerAI worker = workers[i];

                Sprite icon = iconIdle;

                if (worker.IsHunger)
                {
                    icon = iconHungry;
                }

                WorkerUIItem item = Instantiate(workerItemPrefab, workerListContent);
                item.Setup(worker, icon, () => OnWorkerClick?.Invoke(worker));

            }
        }


        public void ClearWorkerList()
        {
            ClearContent(workerListContent);
        }

        private void ClearContent(Transform content)
        {
            for (int i = content.childCount - 1; i >= 0; i--)
            {
                Destroy(content.GetChild(i).gameObject);
            }
        }
    }
}

