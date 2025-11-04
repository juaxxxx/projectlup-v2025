using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;

namespace DSG
{
    public class TeamSelectButton : MonoBehaviour
    {
        [SerializeField]
        private UnityEngine.UI.Toggle toggle;

        [SerializeField]
        private FormationSystem formationSystem;

        public int teamIndex;
        void Start()
        {
            toggle.onValueChanged.AddListener(OnToggleChanged);
            if (teamIndex == 0)
            {
                OnToggleChanged(true);
            }
        }

        void OnToggleChanged(bool isOn)
        {
            toggle.isOn = isOn;
            toggle.image.color = isOn ? Color.gray : Color.white;

            if (isOn)
            {
                formationSystem.PlaceTeam(teamIndex);
            }
        }
    }
}