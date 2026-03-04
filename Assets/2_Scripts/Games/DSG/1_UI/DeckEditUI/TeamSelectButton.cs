using UnityEngine;
using UnityEngine.UI;

namespace LUP.DSG
{
    public class TeamSelectButton : MonoBehaviour
    {
        [SerializeField]
        private Toggle toggle;

        private FormationSystem formationSystem;

        public int teamIndex;

        private bool isRegistered;

        private void Awake()
        {
            StageInitializeInvoker.OnDSGStagePostInitialize += PostInitialize;

            if (toggle == null)
                toggle = GetComponent<Toggle>();
        }
        private void OnDestroy()
        {
            StageInitializeInvoker.OnDSGStagePostInitialize -= PostInitialize;

            if (toggle != null && isRegistered)
                toggle.onValueChanged.RemoveListener(OnToggleChanged);
        }

        private void PostInitialize(DeckStrategyStage stage)
        {
            formationSystem = stage != null ? stage.GetComponent<FormationSystem>() : null;
            if (toggle == null) return;

            if (!isRegistered)
            {
                toggle.onValueChanged.AddListener(OnToggleChanged);
                isRegistered = true;
            }
        }

        void OnToggleChanged(bool isOn)
        {
            Debug.Log("OnToggleChanged");

            if (toggle != null)
                toggle.image.color = isOn ? Color.gray : Color.white;

            if (!isOn) return;

            if (formationSystem != null)
                formationSystem.PlaceTeam(teamIndex);
            SoundManager.Instance.PlaySFX("Inventory Stash 2");
        }

        public void ButtonStateChange(bool isOn)
        {
            if (toggle == null) return;

            toggle.image.color = isOn ? Color.gray : Color.white;
            toggle.SetIsOnWithoutNotify(isOn);
        }
    }
}