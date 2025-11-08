using System.Collections;
using System.Collections.Generic;

namespace LUP
{
    public class ProductionStage : BaseStage
    {
        public BaseStaticDataLoader StaticDataLoader;
        public BaseRuntimeData RuntimeData;

        // 실제 입력한 값들이 담긴 데이터 리스트
        List<ProductionStaticData> DataList;

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
                ProductionStaticDataLoader PCRStaticDataLoader = (ProductionStaticDataLoader)StaticDataLoader;
                if (PCRStaticDataLoader != null)
                {
                    DataList = PCRStaticDataLoader.GetDataList();
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

