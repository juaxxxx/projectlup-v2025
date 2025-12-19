using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LUP.PCR
{
    public class ProductionStage : BaseStage
    {
        private PCRGameSystem gameSystem;

        public BaseRuntimeData productionRuntimeData;

        public List<BuildingStaticData> buildingDataList;
        public List<PCRConstructionStaticData> constructionDataList;
        public List<PCRProductionStaticData> productionDataList;
        public List<InitialBuildingStaticData> initialBuildingDataList;
        public List<InitialWallStaticData> initialWallDataList;


        // 변수명은 예시이니 바꾸셔도 됩니다.
        public Inventory PCRInven;

        protected override void Awake()
        {
            base.Awake();
            StageKind = Define.StageKind.PCR;

            gameSystem = GetComponent<PCRGameSystem>();
        }

        void Start()
        {

        }

        void Update()
        {

        }
        public override IEnumerator OnStageEnter()
        {
            yield return base.OnStageEnter();

            LoadFirstGameData();

            // InventoryManager를 통해 PCR 인벤토리 로드 및 등록
            PCRInven = InventoryManager.Instance.LoadOrCreateInventory("PCR", "PCRInventory.json");

            gameSystem.InitPCRGameSystem();

            yield return null;
        }
        public override IEnumerator OnStageStay()
        {
            yield return base.OnStageStay();
            //일단 납두기
            yield return null;
        }
        public override IEnumerator OnStageExit()
        {
            yield return base.OnStageExit();
            //구현부


            yield return null;
        }
        protected override void LoadResources()
        {
            //resource = ResourceManager.Instance.Load...
        }

        protected override void GetDatas()
        {
            List<BaseStaticDataLoader> loaders = base.GetStaticData(this, 1);
            List<BaseRuntimeData> runtimeDatas = base.GetRuntimeData(this, 1);

            // static
            if (loaders != null && loaders.Count > 0)
            {
                foreach (var loader in loaders)
                {
                    if (loader is PCRConstructionStaticDataLoader pcrConstructionLoader)
                    {
                        constructionDataList = pcrConstructionLoader.GetDataList();
                    }
                    else if (loader is PCRProductionStaticDataLoader pcrProductionLoader)
                    {
                        productionDataList = pcrProductionLoader.GetDataList();
                    }
                    else if (loader is BuildingStaticDataLoader pcrBuildingLoader)
                    {
                        buildingDataList = pcrBuildingLoader.GetDataList();
                    }
                    else if (loader is InitialBuildingStaticDataLoader pcrInitialBuildingLoader)
                    {
                        initialBuildingDataList = pcrInitialBuildingLoader.GetDataList();
                    }
                    else if (loader is InitialWallStaticDataLoader pcrInitialWallLoader)
                    {
                        initialWallDataList = pcrInitialWallLoader.GetDataList();
                    }
                }
            }

            // runtime
            if (runtimeDatas != null && runtimeDatas.Count > 0)
            {
                foreach (var runtimeData in runtimeDatas)
                {
                    if (runtimeData is ProductionRuntimeData pcrRuntimeData)
                    {
                        productionRuntimeData = pcrRuntimeData;
                    }
                }
            }
        }

        protected override void SaveDatas()
        {
            List<BaseRuntimeData> runtimeDataList = new List<BaseRuntimeData>();

            if (productionRuntimeData != null)
            {
                runtimeDataList.Add(productionRuntimeData);
            }

            base.SaveRuntimeDataList(runtimeDataList);
        }

        private void LoadFirstGameData()
        {
            ProductionRuntimeData runtimeData = productionRuntimeData as ProductionRuntimeData;
            if (runtimeData == null)
            {
                Debug.LogWarning("[PCRStage] runtimeData가 없습니다.");
                return;
            }
            if (runtimeData.HasSavedGame)
            {
                Debug.LogWarning("[PCRStage] 이미 저장된 게임 데이터가 존재합니다.");
                return;
            }

            // 건물 초기 데이터
            {
                Debug.LogWarning("[PCRStage] BuildingInfoList First Load");

                List<BuildingInfo> newBuildingDataList = new List<BuildingInfo>();

                runtimeData.BuildingId = 1;

                foreach (InitialBuildingStaticData initialBuildingData in initialBuildingDataList)
                {
                    BuildingInfo newBuildingInfo = new BuildingInfo(runtimeData.GenerateId(), 1, new Vector2Int(initialBuildingData.x, initialBuildingData.y), initialBuildingData.buildingType, false);

                    newBuildingDataList.Add(newBuildingInfo);
                }

                runtimeData.BuildingInfoList = newBuildingDataList;
            }

            // 벽 초기 데이터
            {
                Debug.LogWarning("[PCRStage] WallInfoList First Load");

                List<WallInfo> newWallDataList = new List<WallInfo>();

                foreach(InitialWallStaticData initialWallData in initialWallDataList)
                {
                    WallInfo newWallInfo = new WallInfo(initialWallData.wallType, new Vector2Int(initialWallData.x, initialWallData.y));

                    newWallDataList.Add(newWallInfo);
                }

                runtimeData.WallInfoList = newWallDataList;
            }


            runtimeData.HasSavedGame = true;
        }

        public PCRConstructionStaticData GetCurrentConstructionData(int buildingType, int level)
        {
            if (constructionDataList == null || constructionDataList.Count <= 0)
            {
                Debug.LogError("[ProductionStage] constructionDataList이 비어있습니다.");
                return null;
            }

            foreach (PCRConstructionStaticData data in constructionDataList)
            {
                if (data.buildingType == buildingType)
                {
                    if (data.level == level)
                    {
                        return data;
                    }
                }
            }

            return null;
        }

        public PCRProductionStaticData GetCurrentProductionData(int buildingType, int level)
        {
            if (productionDataList == null || productionDataList.Count <= 0)
            {
                Debug.LogError("[ProductionStage] ProductionDataList이 비어있습니다.");
                return null;
            }

            foreach (PCRProductionStaticData data in productionDataList)
            {
                if (data.buildingType == buildingType)
                {
                    if (data.level == level)
                    {
                        return data;
                    }
                }
            }

            return null;
        }

        public BuildingStaticData GetCurrentBuildingData(int buildingType)
        {
            if (buildingDataList == null || buildingDataList.Count <= 0)
            {
                Debug.LogError("[ProductionStage] buildingDataList이 비어있습니다.");
                return null;
            }

            foreach (BuildingStaticData data in buildingDataList)
            {
                if (data.buildingType == buildingType)
                {
                    return data;
                }
            }

            return null;
        }

        public List<BuildingInfo> GetBuildingInfoList()
        {
            ProductionRuntimeData runtimeData = productionRuntimeData as ProductionRuntimeData;

            if (runtimeData.BuildingInfoList == null)
            {
                Debug.LogError("[ProductionStage] InitialBuildingDataList이 비어있습니다.");
                return null;
            }

            return runtimeData.BuildingInfoList;
        }

        public List<WallInfo> GetWallInfoList()
        {
            ProductionRuntimeData runtimeData = productionRuntimeData as ProductionRuntimeData;

            if (runtimeData.WallInfoList == null)
            {
                Debug.LogError("[ProductionStage] InitialWallDataList이 비어있습니다.");
                return null;
            }

            return runtimeData.WallInfoList;
        }

    }
}

