using System.Collections;
using UnityEngine;

namespace LUP.DSG
{
    public class TeamSelectButton : MonoBehaviour
    {
        [SerializeField]
        private UnityEngine.UI.Toggle toggle;

        private FormationSystem formationSystem;

        public int teamIndex;
        public IEnumerator OnStageEnter()
        {
            formationSystem = FindAnyObjectByType<FormationSystem>();

            toggle.onValueChanged.AddListener(OnToggleChanged);
            if (teamIndex == 0)
            {
                toggle.isOn = true;
            }

            //OnToggleChanged(true);
            yield return null;
        }

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