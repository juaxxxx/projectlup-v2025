using System.Collections;
using System.Collections.Generic;

namespace LUP
{
    public class ExtractionShooterStage : BaseStage
    {
        public BaseRuntimeData RuntimeData;
        public List<ExtractionStaticData> DataList;

        protected override void Awake() 
        {
            base.Awake();
            StageKind = Define.StageKind.ES;
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
            if (RuntimeData != null)
            {
                base.SaveRuntimeData(RuntimeData);
            }
        }

        protected override void SetupInventory()
        {

        }
    }
}

