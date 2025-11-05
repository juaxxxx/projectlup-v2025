using UnityEngine;

namespace Manager
{
    public class DataManager : Singleton<DataManager>
    {
        [SerializeField]
        BaseStaticData data;

        public BaseStaticData GetStaticData(Define.StageKind stagekind, int stagetype)
        {
            BaseStaticData data = null;

            data = ResourceManager.Instance.LoadStaticData(stagekind, stagetype);

            if (!data)
            {
                Debug.LogError($"Failed to load static data");
                return null;
            }

            return data;
        }

        public BaseRuntimeData GetRuntimeData(Define.StageKind stagekind, int stagetype)
        {
            BaseRuntimeData data = null;

            // 나중에 런타임데이터 스크립터블오브젝트가 들어가면 사용?
            // data = ResourceManager.Instance.LoadRuntimeData(stagekind, stagetype);

            
            string filename = "";

            switch (stagekind)
            {
                case Define.StageKind.RL:
                    filename = Define.RuntimeDataTypes.ToFilename(Define.RuntimeDataType.RoguelikeRuntime);
                    data = JsonDataHelper.LoadData<RoguelikeRuntimeData>(filename);
                    break;
                // 그 외 다른 스테이지..

                default:
                    Debug.LogError($"No runtime data defined for StageKind: { stagekind}");
                    return null;
            }

            if (data == null)
            {
                Debug.LogError($"Failed to load runtime data");
                return null;
            }

            data.filename = filename;

            if (!JsonDataHelper.FileExists(filename))
            {
                data.ResetData();
            }

            return data;
        }

        public override void Awake()
        {
            base.Awake();

            // RuntimeData가 코루틴을 사용할 수 있도록 설정
            BaseRuntimeData.SetCoroutineRunner(this);

            // Manager.ResourceManager.Instance.Load
        }

        public void SaveRuntimeData(BaseRuntimeData runtimeData)
        {
            JsonDataHelper.SaveData(runtimeData, runtimeData.filename);
        }
    }
}
