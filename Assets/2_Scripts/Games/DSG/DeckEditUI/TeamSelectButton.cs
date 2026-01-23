using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LUP.DSG
{
    public class TeamSelectButton : MonoBehaviour
    {
        [SerializeField]
        private UnityEngine.UI.Toggle toggle;

        private FormationSystem formationSystem;

        public int teamIndex;

        private void Awake()
        {
            StageInitializeInvoker.OnDSGStagePostInitialize += PostInitialize;
        }
        private void OnDestroy()
        {
            StageInitializeInvoker.OnDSGStagePostInitialize -= PostInitialize;
        }

        private void PostInitialize(DeckStrategyStage stage)
        {
            formationSystem = FindAnyObjectByType<FormationSystem>();

            if (teamIndex == 0)
            {
                toggle.isOn = true;
            }
            toggle.onValueChanged.AddListener(OnToggleChanged);
        }

        void OnToggleChanged(bool isOn)
        {
            Debug.Log("OnToggleChanged");
            toggle.image.color = isOn ? UnityEngine.Color.gray : UnityEngine.Color.white;
            if (isOn)
            {
                formationSystem.PlaceTeam(teamIndex);
                SoundManager.Instance.PlaySFX("Inventory Stash 2");
            }
        }
    }
}