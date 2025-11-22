using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LUP
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

        public void LoadStage(Define.StageKind stage, int sceneindex = -1)
        {
            StageManager.Instance.LoadStage(stage, sceneindex);
        }

        protected abstract void LoadResources();

        // 자식 클래스에서 각 게임에 걸맞는 변수에 데이터 삽입
        protected abstract void GetDatas();

        protected abstract void SaveDatas();

        public virtual IEnumerator OnStageEnter()
        {
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

            yield return null;
        }

        protected void SaveRuntimeData(BaseRuntimeData runtimeData)
        {
            DataManager.Instance.SaveRuntimeData(runtimeData);
        }

        protected void SaveRuntimeDataList(List<BaseRuntimeData> runtimeDataList)
        {
            DataManager.Instance.SaveRuntimeDataList(runtimeDataList);
        }

        protected List<BaseStaticDataLoader> GetStaticData(BaseStage stage, int dataindex)
        {
            List<BaseStaticDataLoader> datas = null;

            datas = LUP.DataManager.Instance.GetStaticData(stage.StageKind, dataindex);

            return datas;
        }

        protected List<BaseRuntimeData> GetRuntimeData(BaseStage stage, int dataindex)
        {
            List<BaseRuntimeData> datas = null;

            datas = LUP.DataManager.Instance.GetRuntimeData(stage.StageKind, dataindex);

            return datas;
        }


    }
}

