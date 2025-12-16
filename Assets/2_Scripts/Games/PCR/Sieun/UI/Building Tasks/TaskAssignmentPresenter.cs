using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace LUP.PCR
{
    public class TaskAssignmentPresenter : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private TaskAssignmentView view;
        [SerializeField] private WorkerDataCenter dataCenter;

        [Header("Data Source")]
        // @TODO : BuildingSystem 에서 가져와야 함
        
        [SerializeField] private GameObject buildingGroup;
        [SerializeField] private List<ProductableBuilding> allBuildings;

        private ProductableBuilding currentSelectedBuilding;

        private void Awake()
        {
            view = GetComponentInChildren<TaskAssignmentView>();
            dataCenter = this.transform.root.GetComponent<WorkerDataCenter>();

            if (buildingGroup != null)
            {
                allBuildings = new List<ProductableBuilding>();

                allBuildings.Clear();

                buildingGroup.GetComponentsInChildren<ProductableBuilding>(true, allBuildings);
            }
        }
        //private void OnEnable()
        //{
        //    view.OnBuildingClick += HandleProductionRequest;
        //}
        //private void OnDisable()
        //{
        //    view.OnBuildingClick -= HandleProductionRequest;
        //}


        private void Start()
        {
            view.OnBuildingClick += HandleBuildingSelected;
            view.OnWorkerClick += HandleWorkerSelected;

            view.UpdateStatusText("작업을 지시할 건물을 선택하세요.");
            view.RenderBuildingList(allBuildings);
            view.ClearWorkerList();
        }


        // @TODO : 실제 생산 시스템과 연동
        //private void HandleProductionRequest(ProductableBuilding building)
        //{
        //    WorkerAI worker = dataCenter.GetBestWorker(building.entrancePos);

        //    if(worker != null)
        //    {
        //        worker.AssignTask(currentSelectedBuilding);
        //        Debug.Log($"[AutoAssign] {worker.name} -> {building.name} (가장 오래 쉼: {Time.time - worker.LastWorkEndTime:F1}초)");
        //    }
        //    else
        //    {
        //        Debug.LogWarning($"[AutoAssign] {building.name}에 보낼 작업자가 없습니다!");
        //    }
        //}

        private void HandleBuildingSelected(ProductableBuilding building)
        {
            currentSelectedBuilding = building;

            view.UpdateStatusText($"선택됨: {building.name}\n투입할 작업자를 선택하세요.");

            List<WorkerAI> idleWorkers = dataCenter.GetIdleWorkers();

            if (idleWorkers.Count == 0)
            {
                view.UpdateStatusText($"선택됨: {building.name}\n(가용한 작업자가 없습니다)");
                view.ClearWorkerList();
            }
            else
            {
                Debug.Log($"[Presenter] 뷰에게 작업자 {idleWorkers.Count}명 표시 요청");
                view.RenderWorkerList(idleWorkers);
            }
        }

        private void HandleWorkerSelected(WorkerAI worker)
        {
            if (currentSelectedBuilding == null) return;

            // AssignTask 내부에서 HasTask = true가 되면서 상태 변경됨
            worker.AssignTask(currentSelectedBuilding);

            view.UpdateStatusText($"할당 완료!\n{worker.name} -> {currentSelectedBuilding}");

            currentSelectedBuilding = null;

            view.ClearWorkerList();
            view.UpdateStatusText("작업을 지시할 건물을 선택하세요.");
        }




    }
}