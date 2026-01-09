using UnityEngine;

namespace LUP.DSG
{
    public class StageButton : MonoBehaviour
    {
        [SerializeField]
        private EnemyStageData enemyStageData;

        public void SetStageInfo()
        {
            DeckStrategyStage stage = LUP.StageManager.Instance.GetCurrentStage() as DeckStrategyStage;
            stage.SetEnemyStage(enemyStageData);
        }
    }
}