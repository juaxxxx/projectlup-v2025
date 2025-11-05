using UnityEngine;
using System.Collections;

namespace Manager
{
    public class RoguelikeStage : BaseStage
    {
        public BaseStaticData StaticData;
        public BaseRuntimeData RuntimeData;

        public GameObject TestCharacter;

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
            
            //구현부
            Instantiate(TestCharacter);

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
            StaticData = base.GetStaticData(this);
            RuntimeData = base.GetRuntimeData(this);
        }

        protected override void SaveDatas()
        {
            if(RuntimeData != null)
            {
                base.SaveRuntimeData(RuntimeData);
            }
        }
    }
}

