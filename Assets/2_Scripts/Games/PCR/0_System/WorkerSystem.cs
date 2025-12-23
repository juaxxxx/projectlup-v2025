using System.Collections.Generic;
using UnityEngine;
using static System.Collections.Specialized.BitVector32;

namespace LUP.PCR
{
    public class WorkerSystem : MonoBehaviour
    {
        private ProductionRuntimeData pcrRuntimeData;

        private List<int> curReservedBuildingIdList;
        private List<int> curAssignedBuildingIdList;
        private List<WorkerInfo> curWorkerInfoList;
        private Dictionary<int, BuildingBase> curBuildings; // 건물 Id로 BuildingBase 읽기전용
        private TileMap tileMap;

        private BuildingBase restaurant;
        private List<BuildingBase> workStationList;

        // Worker Logic
        [SerializeField] private AGridMap aGrid;


        public void InitWorkerSystem(BuildingSystem buildingSystem, TileMap tileMap)
        {
            this.tileMap = tileMap;
            aGrid.InitMap(tileMap.tiles);


            curBuildings = buildingSystem.GetCurrentBuildingDictionary();

            // Restaurant를 buildingId: 1로 배정 예정. 추후 바뀔 수 있음.
            if (curBuildings[1] is BuildingRestaurant)
            {
                restaurant = curBuildings[1];
            }
            else
            {
                Debug.Log("Restaurant is empty!");
            }

            foreach (BuildingBase building in curBuildings.Values)
            {
                if (building is BuildingWorkStation)
                {
                    workStationList.Add(building);
                }
            }

            ProductionStage stage = StageManager.Instance.GetCurrentStage() as ProductionStage;
            pcrRuntimeData = stage.productionRuntimeData;
            curReservedBuildingIdList = pcrRuntimeData.ReservedBuildingIdList;
            curAssignedBuildingIdList = pcrRuntimeData.AssignedBuildingIdList;
            curWorkerInfoList = pcrRuntimeData.WorkerInfoList;

            // 위 데이터 기반으로 초기화.

        //    for (int i = 0; i < workers.Count; i++)
        //    {
        //        workers[i].InitWorkerData(i, $"Chulsoo_{i + 1}");
        //        workers[i].InitBTReferences();
        //    }

        //    Debug.Log("WorkerSystem Init");
        }

        //// 외부에서 워커를 등록하는 함수
        //public void RegisterWorker(WorkerAI newWorker)
        //{
        //    if (!isInitialized)
        //    {
        //        InitializeReferences();
        //    }

        //    if (!workers.Contains(newWorker))
        //    {
        //        workers.Add(newWorker);

        //        newWorker.InitBTReferences();
        //        newWorker.SetGlobalBuildings(restaurant, station);

        //        curWorkerInfoList.Add(newWorker);
        //    }
        //}

        //public void InitWorkers()
        //{
        //    // 테스트용
            
        //}

        //public void SetupTestProfiles()
        //{
        //    for (int i = 0; i < workers.Count; i++)
        //    {
        //        workers[i].InitWorkerData(i, $"Worker_{i + 1}");
        //    }
        //}


    }
}
