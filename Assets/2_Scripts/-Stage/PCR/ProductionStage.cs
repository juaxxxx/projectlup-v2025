using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LUP
{
    public class ProductionStage : BaseStage
    {
        public BaseRuntimeData RuntimeData;
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

            Debug.Log("GetDatas");

            if (loaders != null && loaders.Count > 0)
            {
                foreach (var loader in loaders)
                {
                    if (loader is PCRConstructionStaticDataLoader pcrConstructionLoader)
                    {
                        Debug.Log("ConstructionLoad");
                        constructionDataList = pcrConstructionLoader.GetDataList();
                    }
                    else if (loader is PCRProductionStaticDataLoader pcrProductionLoader)
                    {
                        Debug.Log("ProductionLoad");
                        productionDataList = pcrProductionLoader.GetDataList();
                    }
                }
            }

            if (runtimeDatas != null && runtimeDatas.Count > 0)
            {
                foreach (var runtimeData in runtimeDatas)
                {
                    if (runtimeData is ProductionRuntimeData pcrRuntimeData)
                    {
                        RuntimeData = pcrRuntimeData;
                    }
                }
            }
        }

        protected override void SaveDatas()
        {
            List<BaseRuntimeData> runtimeDataList = new List<BaseRuntimeData>();

            if (RuntimeData != null)
            {
                runtimeDataList.Add(RuntimeData);
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

