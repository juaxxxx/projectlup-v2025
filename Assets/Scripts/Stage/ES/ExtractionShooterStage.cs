using System.Collections;
using System.Collections.Generic;

namespace LUP
{
    public class ExtractionShooterStage : BaseStage
    {
        public BaseStaticDataLoader StaticDataLoader;
        public BaseRuntimeData RuntimeData;

        List<ExtractionStaticData> DataList;

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
            StaticDataLoader = base.GetStaticData(this, 1);
            RuntimeData = base.GetRuntimeData(this, 1);

            if (StaticDataLoader != null)
            {
                ExtractionStaticDataLoader ESStaticDataLoader = (ExtractionStaticDataLoader)StaticDataLoader;
                if (ESStaticDataLoader != null)
                {
                    DataList = ESStaticDataLoader.GetDataList();
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

