using System.Collections;
using System.Collections.Generic;

namespace LUP
{
    public class DeckStrategyStage : BaseStage
    {
        public BaseRuntimeData RuntimeData;

        public List<DeckStaticData> DeckDataList;
        public List<DeckCharacterStaticData> CharacterDataList;

        protected override void Awake() 
        {
            base.Awake();
            StageKind = Define.StageKind.DSG;
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

            // 일단 타입별로 가져오는 예시
            if (loaders != null && loaders.Count > 0)
            {
                foreach (var loader in loaders)
                {
                    if (loader is DeckStaticDataLoader deckLoader)
                    {
                        DeckDataList = deckLoader.GetDataList();
                    }
                    else if (loader is DeckCharacterStaticDataLoader charLoader)
                    {
                        CharacterDataList = charLoader.GetDataList();
                    }
                }
            }

            // 일단 타입별로 가져오는 예시
            if (runtimeDatas != null && runtimeDatas.Count > 0)
            {
                foreach (var runtimeData in runtimeDatas)
                {
                    if (runtimeData is DeckStrategyRuntimeData deckRuntimeData)
                    {
                        RuntimeData = deckRuntimeData;
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

