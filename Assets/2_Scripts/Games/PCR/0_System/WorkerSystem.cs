using System.Collections.Generic;
using UnityEngine;

namespace LUP.PCR
{
    public class WorkerSystem : MonoBehaviour
    {
        private ProductionRuntimeData pcrRuntimeData;

        private List<int> curReservedBuildingInfoList;
        private List<int> curAssignedBuildingInfoList;
        private List<WorkerInfo> curWorkerInfoList;
        private Dictionary<int, BuildingBase> curBuildings; // 건물 Id로 BuildingBase 읽기전용
        private TileMap tileMap;

        private BuildingBase restaurant;
        private List<BuildingBase> workStationList;

        public void InitWorkerSystem(BuildingSystem buildingSystem, TileMap tileMap)
        {
            this.tileMap = tileMap;
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
            curReservedBuildingInfoList = pcrRuntimeData.ReservedBuildingIdList;
            curAssignedBuildingInfoList = pcrRuntimeData.AssignedBuildingIdList;
            curWorkerInfoList = pcrRuntimeData.WorkerInfoList;

            // 위 데이터 기반으로 초기화.


            Debug.Log("WorkerSystem Init");
        }


    }
}
