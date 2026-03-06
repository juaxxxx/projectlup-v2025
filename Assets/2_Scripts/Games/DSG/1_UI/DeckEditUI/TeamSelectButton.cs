using System;
using UnityEngine;
using UnityEngine.UI;

namespace LUP.DSG
{
    public class TeamSelectButton : MonoBehaviour
    {
        [SerializeField]
        private Toggle toggle;

        public int teamIndex;
        public event Action<int> OnTeamSelected;

        private void Awake()
        {
            if (toggle == null) toggle = GetComponent<Toggle>();
            if (toggle != null) toggle.onValueChanged.AddListener(OnToggleChanged);
        }
        private void OnDestroy()
        {
            if (toggle != null)
                toggle.onValueChanged.RemoveListener(OnToggleChanged);
        }

        public void ButtonStateChange(bool isOn)
        {
            if (toggle == null) return;

            toggle.image.color = isOn ? Color.gray : Color.white;
            toggle.SetIsOnWithoutNotify(isOn);
        }

        private void OnToggleChanged(bool isOn)
        {
            if (toggle != null)
                toggle.image.color = isOn ? Color.gray : Color.white;

            if (!isOn) return;

            OnTeamSelected?.Invoke(teamIndex);

            SoundManager.Instance.PlaySFX("Inventory Stash 2");
        }
    }
}