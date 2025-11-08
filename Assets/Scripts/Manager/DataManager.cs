using UnityEngine;

namespace LUP
{
    public class DataManager : Singleton<DataManager>
    {
        [SerializeField]
        BaseStaticDataLoader data;

        public BaseStaticDataLoader GetStaticData(Define.StageKind stagekind, int stagetype)
        {
            BaseStaticDataLoader data = null;

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

            string filename = "";

            switch (stagekind)
            {
                case LUP.Define.StageKind.RL:
                    filename = Define.RuntimeDataTypes.ToFilename(Define.RuntimeDataType.RoguelikeRuntime);
                    data = JsonDataHelper.LoadData<RoguelikeRuntimeData>(filename);
                    break;
                case LUP.Define.StageKind.DSG:
                    filename = Define.RuntimeDataTypes.ToFilename(Define.RuntimeDataType.DeckStrategyRuntime);
                    data = JsonDataHelper.LoadData<DeckStrategyRuntimeData>(filename);
                    break;
                case LUP.Define.StageKind.Main:
                    filename = Define.RuntimeDataTypes.ToFilename(Define.RuntimeDataType.Versions);
                    data = JsonDataHelper.LoadData<VersionsData>(filename);
                    break;
                case LUP.Define.StageKind.PCR:
                    filename = Define.RuntimeDataTypes.ToFilename(Define.RuntimeDataType.ProductionRuntime);
                    data = JsonDataHelper.LoadData<ProductionRuntimeData>(filename);
                    break;
                case LUP.Define.StageKind.ES:
                    filename = Define.RuntimeDataTypes.ToFilename(Define.RuntimeDataType.ExtractionShooterRuntime);
                    data = JsonDataHelper.LoadData<ExtractionRuntimeData>(filename);
                    break;
                case LUP.Define.StageKind.ST:
                    filename = Define.RuntimeDataTypes.ToFilename(Define.RuntimeDataType.ShootingRuntime);
                    data = JsonDataHelper.LoadData<ShootingRuntimeData>(filename);
                    break;
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
