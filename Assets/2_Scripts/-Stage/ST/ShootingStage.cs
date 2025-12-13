using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LUP
{
    public class ShootingStage : BaseStage
    {
        public BaseRuntimeData RuntimeData;
        public List<ShootingStaticData> DataList;

        // 변수명은 예시이니 바꾸셔도 됩니다.
        public Inventory STInven;

        protected override void Awake() 
        {
            base.Awake();
            StageKind = Define.StageKind.ST;

            // 파일명은 팀끼리 구분되기만 하면 자유롭게 사용하셔도 됩니다.
            STInven.filename = "STInventory.json";
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
            string inventoryFilename = STInven.filename;

            if (JsonDataHelper.FileExists(inventoryFilename))
            {
                // 기존 인벤토리 로드
                STInven = JsonDataHelper.LoadData<Inventory>(inventoryFilename);
                if (STInven != null)
                {
                    STInven.filename = inventoryFilename;
                    STInven.InitializeFromJson();  // Dictionary 복원
                    Debug.Log("[STStage] 인벤토리 로드 완료");
                }
                else
                {
                    Debug.LogWarning("[STStage] 인벤토리 로드 실패, 새로 생성");
                    STInven = new Inventory();
                    STInven.filename = inventoryFilename;
                }
            }
            else
            {
                STInven = new Inventory();
                STInven.filename = inventoryFilename;
                Debug.Log("[STStage] 새 인벤토리 생성");
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
                    if (loader is ShootingStaticDataLoader stLoader)
                    {
                        DataList = stLoader.GetDataList();
                    }
                }
            }

            if (runtimeDatas != null && runtimeDatas.Count > 0)
            {
                foreach (var runtimeData in runtimeDatas)
                {
                    if (runtimeData is ShootingRuntimeData stRuntimeData)
                    {
                        RuntimeData = stRuntimeData;
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

