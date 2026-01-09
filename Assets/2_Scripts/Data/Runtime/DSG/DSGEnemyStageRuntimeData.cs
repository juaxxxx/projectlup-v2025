using LUP.DSG;
using LUP.ES;
using UnityEngine;

public class DSGEnemyStageRuntimeData : BaseRuntimeData
{
    [SerializeField]
    private EnemyStageData selectedEnemyStage;

    public EnemyStageData SelectedEnemyStage
    {
        get => selectedEnemyStage;
        set => SetValue(ref selectedEnemyStage, value);
    }

    public override void ResetData()
    {
        selectedEnemyStage = null;
    }
}
