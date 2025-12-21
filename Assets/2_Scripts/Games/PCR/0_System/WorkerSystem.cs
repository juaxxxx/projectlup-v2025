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

        public void InitWorkerSystem(BuildingSystem buildingSystem, TileMap tileMap)
        {
            this.tileMap = tileMap;
            curBuildings = buildingSystem.GetCurrentBuildingDictionary();

            ProductionStage stage = StageManager.Instance.GetCurrentStage() as ProductionStage;
            pcrRuntimeData = stage.productionRuntimeData;
            curReservedBuildingInfoList = pcrRuntimeData.ReservedBuildingIdList;
            curAssignedBuildingInfoList = pcrRuntimeData.AssignedBuildingIdList;
            curWorkerInfoList = pcrRuntimeData.WorkerInfoList;

            // 위 런타임 데이터 기반으로 초기화.

        }


    }
}
