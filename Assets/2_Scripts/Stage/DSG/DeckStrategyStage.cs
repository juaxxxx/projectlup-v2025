using LUP.DSG;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LUP
{
    public static class StageEnterSystem
    {
        // base.OnStageEnter() 이후 초기화 지점
        public static event Action<DeckStrategyStage> OnAfterDSGStageEnter;

        public static void Invoke(DeckStrategyStage stage)
        {
            OnAfterDSGStageEnter?.Invoke(stage);
        }
    }

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

            StageEnterSystem.Invoke(this);

            //구현부
            //ToggleGroup toggleGroup = FindAnyObjectByType<ToggleGroup>();
            //if (toggleGroup != null)
            //{
            //    TeamSelectButton[] teamSelectButtons = toggleGroup.transform.GetComponentsInChildren<TeamSelectButton>();
            //    if (teamSelectButtons.Length > 0)
            //    {
            //        List<Coroutine> running = new List<Coroutine>();

            //        foreach (TeamSelectButton button in teamSelectButtons)
            //        {
            //            Coroutine coroutine = StartCoroutine(button.OnStageEnter());
            //            running.Add(coroutine);
            //        }

            //        foreach (Coroutine coroutine in running)
            //        {
            //            yield return coroutine;
            //        }
            //    }
            //}

            OnStageEnterFinished();

            yield return null;
        }

        public void OnStageEnterFinished()
        {
            DataCenter dataCenter = FindAnyObjectByType<DataCenter>();
            if (dataCenter != null)
            {
                dataCenter.SetOwnedCharacterList();
            }
            CharactersList charactersList = FindAnyObjectByType<CharactersList>();
            if (charactersList != null)
            {
            }
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

            if (RuntimeData != null)
            {
                DeckStrategyRuntimeData deckStrategyRuntimeData = (DeckStrategyRuntimeData)RuntimeData;
                if (deckStrategyRuntimeData != null && deckStrategyRuntimeData.Teams.Count == 0)
                {
                    deckStrategyRuntimeData.Teams.AddRange(new UserData.Team[8]);
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

            // 나중에 다른 RuntimeData 추가 시 여기에 추가
            // if (CharacterRuntimeData != null)
            // {
            //     runtimeDataList.Add(CharacterRuntimeData);
            // }

            base.SaveRuntimeDataList(runtimeDataList);
        }

    }
}

