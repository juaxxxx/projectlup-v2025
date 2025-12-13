using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LUP
{
    public class ExtractionShooterStage : BaseStage
    {
        public BaseRuntimeData RuntimeData;
        public List<ExtractionStaticData> DataList;

        // 변수명은 예시이니 바꾸셔도 됩니다.
        public Inventory ESInven;

        protected override void Awake()
        {
            base.Awake();
            StageKind = Define.StageKind.ES;

            // 파일명은 팀끼리 구분되기만 하면 자유롭게 사용하셔도 됩니다.
            ESInven.filename = "ESInventory.json";
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
            //구현부

            // Inventory 생성 및 파일명 설정
            string inventoryFilename = ESInven.filename;

            if (JsonDataHelper.FileExists(inventoryFilename))
            {
                // 기존 인벤토리 로드
                ESInven = JsonDataHelper.LoadData<Inventory>(inventoryFilename);
                if (ESInven != null)
                {
                    ESInven.filename = inventoryFilename;
                    ESInven.InitializeFromJson();  // Dictionary 복원
                    Debug.Log("[ESStage] 인벤토리 로드 완료");
                }
                else
                {
                    Debug.LogWarning("[ESStage] 인벤토리 로드 실패, 새로 생성");
                    ESInven = new Inventory();
                    ESInven.filename = inventoryFilename;
                }
            }
            else
            {
                ESInven = new Inventory();
                ESInven.filename = inventoryFilename;
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
                    if (loader is ExtractionStaticDataLoader ESLoader)
                    {
                        DataList = ESLoader.GetDataList();
                    }
                }
            }

            if (runtimeDatas != null && runtimeDatas.Count > 0)
            {
                foreach (var runtimeData in runtimeDatas)
                {
                    if (runtimeData is ExtractionRuntimeData ESRuntimeData)
                    {
                        RuntimeData = ESRuntimeData;
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

