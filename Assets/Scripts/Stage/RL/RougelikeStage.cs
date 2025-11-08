using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Manager
{
    public class RoguelikeStage : BaseStage
    {
        public BaseStaticDataLoader StaticDataLoader;
        public BaseRuntimeData RuntimeData;

        List<RoguelikeStaticData> DataList;

        protected override void Awake() 
        {
            base.Awake();
            StageKind = Define.StageKind.RL;
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
            StaticDataLoader = base.GetStaticData(this, (int)Define.RoguelikeStageKind.Lobby);
            RuntimeData = base.GetRuntimeData(this, (int)Define.RoguelikeStageKind.Lobby);

            if (StaticDataLoader != null)
            {
                RoguelikeStaticDataLoader RLStaticDataLoader = (RoguelikeStaticDataLoader)StaticDataLoader;
                if (RLStaticDataLoader != null)
                {
                    DataList = RLStaticDataLoader.GetDataList();
                }
            }
        }

        protected override void SaveDatas()
        {
            if(RuntimeData != null)
            {
                base.SaveRuntimeData(RuntimeData);
            }
        }

        protected override void SetupInventory()
        {

        }
    }
}

