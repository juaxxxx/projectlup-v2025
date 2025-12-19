using LUP.ES;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LUP
{
    public class ExtractionShooterStage : BaseStage
    {
        public ExtractionRuntimeData RuntimeData;
        public List<ExtractionStaticData> DataList;

        private CharacterSelector characterSelector;
        // 변수명은 예시이니 바꾸셔도 됩니다.
        public Inventory ESInven;

        protected override void Awake()
        {
            base.Awake();
            StageKind = Define.StageKind.ES;
        }

        void Start()
        {
            characterSelector = GetComponentInChildren<CharacterSelector>();
        }

        void Update()
        {

        }

        public override IEnumerator OnStageEnter()
        {
            yield return base.OnStageEnter();
            //구현부

            // InventoryManager를 통해 ES 인벤토리 로드 및 등록
            ESInven = InventoryManager.Instance.LoadOrCreateInventory("ES", "ESInventory.json");

            if(characterSelector != null)
            {
                characterSelector.Init();
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

            if (RuntimeData != null)
            {
                RuntimeData.PlayerID = 0;
                RuntimeData.WeaponID = 0;
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

        public void SceneChange(int sceneNumber)
        {
            Time.timeScale = 1f;
            LoadStage(StageKind, sceneNumber);
        }
    }
}

