using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Manager
{
    public abstract class BaseStage : MonoBehaviour
    {
        [ReadOnly] public Define.StageKind StageKind = Define.StageKind.Main;

        protected virtual void Awake()
        {

        }

        void Update()
        {

        }

        /*
        UnKnown = 0,    // 이상한 씬
        Debug = 1,      // 디버그 씬 (개발용)
        Main = 2,       // 메인 화면
        Intro = 3,      // 인트로
        Roguelike = 4,  // 로그라이크
        Shooting = 5,   // 슈팅
        ExtractionShooter = 6, // 익스트랙션 슈터
        Production = 7,  // 생산/건설/강화
        DeckStrategy = 8, // 덱 전략
         */

        public void LoadStage(Define.StageKind stage, int sceneindex = -1)
        {
            StageManager.Instance.LoadStage(stage, sceneindex);
        }

        protected abstract void LoadResources();

        // 자식 클래스에서 각 게임에 걸맞는 변수에 데이터 삽입
        protected abstract void GetDatas();

        protected abstract void SaveDatas();

        protected abstract void SetupInventory();

        public virtual IEnumerator OnStageEnter()
        {
            SetupInventory();
            LoadResources();
            GetDatas();

            yield return null;
        }

        public virtual IEnumerator OnStageStay()
        {
            yield return null;
        }

        public virtual IEnumerator OnStageExit()
        {
            SaveDatas();
            Manager.InventoryManager.Instance.OnStageExit();

            yield return null;
        }

        protected void SaveRuntimeData(BaseRuntimeData runtimeData)
        {
            DataManager.Instance.SaveRuntimeData(runtimeData);
        }

        protected BaseStaticDataLoader GetStaticData(BaseStage stage, int dataindex)
        {
            BaseStaticDataLoader data = null;

            data = Manager.DataManager.Instance.GetStaticData(stage.StageKind, dataindex);

            return data;
        }

        protected BaseRuntimeData GetRuntimeData(BaseStage stage, int dataindex)
        {
            BaseRuntimeData data = null;

            data = Manager.DataManager.Instance.GetRuntimeData(stage.StageKind, dataindex);

            return data;
        }

       
    }
}

