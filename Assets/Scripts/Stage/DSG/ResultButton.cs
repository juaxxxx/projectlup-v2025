using UnityEngine;

namespace DSG
{
    public class ResultButton : MonoBehaviour
    {
        [SerializeField] private BattleSystem battleSystem;

        public void OnExitButtonClicked()
        {
            if (battleSystem == null)
                battleSystem = FindFirstObjectByType<BattleSystem>();

            if (battleSystem != null)
            {
                battleSystem.EndBattle();
            }
        }
    }
}