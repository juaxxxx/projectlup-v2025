using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LUP.PCR
{
    public class ProductionStage : BaseStage
    {
        public BaseRuntimeData productionRuntimeData;
        public List<PCRConstructionStaticData> constructionDataList;
        public List<PCRProductionStaticData> productionDataList;

        // 변수명은 예시이니 바꾸셔도 됩니다.
        public Inventory PCRInven;

        protected override void Awake()
        {
            base.Awake();
            StageKind = Define.StageKind.PCR;
        }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
        public override IEnumerator OnStageEnter()
        {
            yield return base.OnStageEnter();

            //구현부
            ProductionRuntimeData runtimeData = productionRuntimeData as ProductionRuntimeData;
            if (runtimeData == null)
            {
                Debug.LogWarning("[PCRStage] productionRuntimeData가 없습니다.");

                if (runtimeData.BuildingInfoList == null || runtimeData.BuildingInfoList.Count <= 0)
                {
                    Debug.LogWarning("[PCRStage] BuildingInfoList가 없습니다.");

                    // 없으면 초기 건물 리스트 입력하기. (일단 따라서 테스트해보기)
                    InitialBuildingSettingTable initalBuildingTable = Resources.Load<InitialBuildingSettingTable>("Data/Games/PCR/SO/InitialBuildingSettingTable");
                    if (initalBuildingTable != null)
                    {
                        runtimeData.BuildingInfoList = initalBuildingTable.buildingList;
                    }
                }

            }


            // InventoryManager를 통해 PCR 인벤토리 로드 및 등록
            PCRInven = InventoryManager.Instance.LoadOrCreateInventory("PCR", "PCRInventory.json");

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

        public PCRConstructionStaticData FindCurrentConstructionData(int buildingType, int level)
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

        public PCRProductionStaticData FindCurrentProductionData(int buildingType, int level)
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

        // buildingData

        // InitialBuilding
        // InitialWalldata
    }
}

