using System;
using UnityEngine;

namespace LUP.DSG
{
    public class FormationView : MonoBehaviour
    {
        [SerializeField]
        private GameObject[] slotObjects = new GameObject[5];
        [SerializeField]
        private Transform characterListContent;
        [Header("Team Selection UI")]
        [SerializeField] private TeamSelectButton[] teamSelectButtons;

        public LineupSlot[] lineupSlots { get; private set; }

        public event Action<OwnedCharacterInfo, CharacterSelectButton> OnCharacterSlotClicked;
        public event Action<int> OnReleaseSlotClicked;
        public event Action<int> OnTeamButtonClicked;

        private void Awake()
        {
            CacheLineupSlots();
            RegisterTeamButtons();
        }

        public void UpdateCharacterListUI(Team selectedTeam)
        {
            CharactersList list = characterListContent != null ? characterListContent.GetComponentInParent<CharactersList>() : null;
            if (list == null) return;

            list.ResetSelectedStatus();
            if (selectedTeam?.characters != null)
            {
                foreach (OwnedCharacterInfo info in selectedTeam.characters)
                {
                    if (info == null) continue;
                    list.UpdateCheckedList(info.characterID, true);
                }
            }
            list.RePopulateThroughFilter();
        }

        public void OnClickTeamChangeButton(int teamIndex)
        {
            OnTeamButtonClicked?.Invoke(teamIndex);
        }

        public void PlayEquipSound()
        {
            SoundManager.Instance.PlaySFX("Inventory Stash 2");
        }

        private void CacheLineupSlots()
        {
            lineupSlots = new LineupSlot[slotObjects.Length];
            for (int i = 0; i < slotObjects.Length; i++)
            {
                lineupSlots[i] = slotObjects[i] != null ? slotObjects[i].GetComponent<LineupSlot>() : null;
            }
        }

        private void RegisterTeamButtons()
        {
            if (teamSelectButtons == null) return;

            foreach (TeamSelectButton button in teamSelectButtons)
            {
                if (button != null)
                    button.OnTeamSelected += HandleTeamSelected;
            }
        }

        private void HandleTeamSelected(int index)
        {
            OnTeamButtonClicked?.Invoke(index);
        }

        public void UpdateSelectedTeamTabUI(int teamIndex)
        {
            if (teamSelectButtons == null) return;

            foreach (TeamSelectButton button in teamSelectButtons)
            {
                // 현재 선택된 팀 인덱스와 일치하는 버튼만 켜지도록 연출 
                if (button != null)
                    button.ButtonStateChange(button.teamIndex == teamIndex);
            }
        }
    }
}