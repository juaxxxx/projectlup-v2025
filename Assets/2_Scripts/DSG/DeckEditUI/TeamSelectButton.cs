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

        private void OnEnable()
        {
            StageEnterSystem.OnAfterDSGStageEnter += Initialize;
        }

        private void OnDisable()
        {
            StageEnterSystem.OnAfterDSGStageEnter -= Initialize;
        }

        private void Initialize(DeckStrategyStage stage)
        {
            formationSystem = FindAnyObjectByType<FormationSystem>();

            toggle.onValueChanged.AddListener(OnToggleChanged);
            if (teamIndex == 0)
            {
                toggle.isOn = true;
            }
        }

        //public IEnumerator OnStageEnter()
        //{
        //    formationSystem = FindAnyObjectByType<FormationSystem>();

        //    toggle.onValueChanged.AddListener(OnToggleChanged);
        //    if (teamIndex == 0)
        //    {
        //        toggle.isOn = true;
        //    }

        //    yield return null;
        //}

        void OnToggleChanged(bool isOn)
        {
            Debug.Log("OnToggleChanged");
            toggle.image.color = isOn ? UnityEngine.Color.gray : UnityEngine.Color.white;
            if (isOn)
            {
                formationSystem.PlaceTeam(teamIndex);
            }
        }
    }
}