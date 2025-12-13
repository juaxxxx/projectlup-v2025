using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LUP
{
    public class ProductionStage : BaseStage
    {
        public BaseRuntimeData RuntimeData;
        public List<PCRConstructionStaticData> DataList;

        // 변수명은 예시이니 바꾸셔도 됩니다.
        public Inventory PCRInven;

        protected override void Awake() 
        {
            base.Awake();
            StageKind = Define.StageKind.PCR;

            // 파일명은 팀끼리 구분되기만 하면 자유롭게 사용하셔도 됩니다.
            PCRInven.filename = "PCRInventory.json";
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

            // Inventory 생성 및 파일명 설정
            string inventoryFilename = PCRInven.filename;

            if (JsonDataHelper.FileExists(inventoryFilename))
            {
                // 기존 인벤토리 로드
                PCRInven = JsonDataHelper.LoadData<Inventory>(inventoryFilename);
                if (PCRInven != null)
                {
                    PCRInven.filename = inventoryFilename;
                    PCRInven.InitializeFromJson();  // Dictionary 복원
                    Debug.Log("[ESStage] 인벤토리 로드 완료");
                }
                else
                {
                    Debug.LogWarning("[ESStage] 인벤토리 로드 실패, 새로 생성");
                    PCRInven = new Inventory();
                    PCRInven.filename = inventoryFilename;
                }
            }
            else
            {
                PCRInven = new Inventory();
                PCRInven.filename = inventoryFilename;
                Debug.Log("[ESStage] 새 인벤토리 생성");
            }

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

            if (loaders != null && loaders.Count > 0)
            {
                foreach (var loader in loaders)
                {
                    if (loader is PCRConstructionStaticDataLoader pcrLoader)
                    {
                        DataList = pcrLoader.GetDataList();
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

    }
}

