using UnityEngine;

namespace LUP.DSG
{
    public class TeamSelectButton : MonoBehaviour
    {
        [SerializeField]
        private UnityEngine.UI.Toggle toggle;

        private FormationSystem formationSystem;

        public int teamIndex;
        void Start()
        {
            formationSystem = FindAnyObjectByType<FormationSystem>();

            toggle.onValueChanged.AddListener(OnToggleChanged);
            if (teamIndex == 0)
            {
                toggle.isOn = true;
            }
        }

        void OnToggleChanged(bool isOn)
        {
            toggle.image.color = isOn ? Color.gray : Color.white;
            if (isOn)
            {
                formationSystem.PlaceTeam(teamIndex);
            }
        }
    }
}