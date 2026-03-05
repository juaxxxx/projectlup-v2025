using OpenCvSharp.ML;
using UnityEngine;
using UnityEngine.UI;

namespace LUP.DSG
{
    public class FormationBootstrap : MonoBehaviour
    {
        [SerializeField] private FormationView formationView;
        private FormationPresenter presenter;

        private void OnEnable()
        {
            StageInitializeInvoker.OnDSGStagePostInitialize += OnStagePostInitialize;
        }

        private void OnDisable()
        {
            StageInitializeInvoker.OnDSGStagePostInitialize -= OnStagePostInitialize;
        }

        private void OnStagePostInitialize(DeckStrategyStage stage)
        {
            ICharacterFactory factory = new CharacterFactory(stage);

            // 3. Presenter 조립 (View, Model, Factory 연결)
            presenter = new FormationPresenter(formationView, factory, stage);

            DeckStrategyRuntimeData runtimeData = stage.RuntimeData as DeckStrategyRuntimeData;
            if (runtimeData == null) return;

            presenter.LoadTeam(runtimeData.SelectedTeamIndex);

            ToggleGroup toggleGroup = FindAnyObjectByType<ToggleGroup>();
            if (toggleGroup)
            {
                TeamSelectButton[] teamButtons = toggleGroup.GetComponentsInChildren<TeamSelectButton>(true);
                int idx = runtimeData.SelectedTeamIndex;

                if (idx >= 0 && idx < teamButtons.Length)
                    teamButtons[idx].ButtonStateChange(true);
            }
        }

        //private void Start()
        //{
        //    // 1. 의존성 주입을 위한 Stage 가져오기
        //    DeckStrategyStage currentStage = LUP.StageManager.Instance.GetCurrentStage() as DeckStrategyStage;

        //    // 2. 팩토리 생성
        //    ICharacterFactory factory = new CharacterFactory(currentStage);

        //    // 3. Presenter 조립 (View, Model, Factory 연결)
        //    presenter = new FormationPresenter(formationView, factory, currentStage);
        //}
    }
}