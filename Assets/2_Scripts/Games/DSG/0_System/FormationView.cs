using OpenCvSharp.ML;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace LUP.DSG
{
    public class FormationView : MonoBehaviour
    {
        [SerializeField]
        private GameObject[] slotObjects = new GameObject[5];
        [SerializeField]
        private CharactersList characterList;
        [SerializeField]
        private TeamSelectButton[] teamSelectButtons;
        [SerializeField]
        private CharacterFilterPanel characterFilterPanel;

        public LineupSlot[] lineupSlots { get; private set; }

        public event Action<int, CharacterSelectButton> OnCharacterIconSelected;
        public event Action<int, CharacterSelectButton> OnCharacterIconReleased;
        public event Action<int> OnTeamButtonClicked;
        public event Action<CharacterFilterState> OnFilterRequested;

        private void Awake()
        {
            CacheLineupSlots();
            RegisterTeamButtons();
        }

        private void OnEnable()
        {
            if (characterFilterPanel != null)
                characterFilterPanel.OnConfirmFilter += RequestApplyFilter;
        }

        private void OnDisable()
        {
            if (characterFilterPanel != null)
                characterFilterPanel.OnConfirmFilter -= RequestApplyFilter;
        }

        public void UpdateCharacterListUI(List<CharacterInfo> filteredList, Team selectedTeam, DeckStrategyStage stage)
        {
            if (characterList == null || filteredList == null || stage == null) return;

            characterList.ResetSelectedStatus();

            // 선택된 팀 체크박스 UI 업데이트
            if (selectedTeam?.characters != null)
            {
                foreach (CharacterInfo info in selectedTeam.characters)
                {
                    if (info == null) continue;
                    characterList.UpdateCheckedList(info.characterID, true);
                }
            }

            characterList.ReleaseAllIcons();

            AttributeIconContainer iconContainer = stage.GetComponent<AttributeIconContainer>();
            if (iconContainer == null) return;

            for (int i = 0; i < filteredList.Count; i++)
            {
                CharacterInfo info = filteredList[i];
                if (info == null) continue;

                CharacterData data = stage.FindCharacterData(info.characterID, info.characterLevel);
                if (data == null) continue;

                AttributeTypeImage typeIcon = iconContainer.GetTypeByAttributeImage(data.type);
                characterList.UpdateCharacterIcon(info, typeIcon);
            }
        }

        public void UpdateSelectedTeamButtonUI(int teamIndex)
        {
            if (teamSelectButtons == null) return;

            foreach (TeamSelectButton button in teamSelectButtons)
            {
                if (button != null)
                    button.ButtonStateChange(button.teamIndex == teamIndex);
            }
        }

        public void RequestPlaceCharacter(int characterId, CharacterSelectButton button)
        {
            OnCharacterIconSelected?.Invoke(characterId, button);
        }

        public void RequestReleaseCharacter(int characterId, CharacterSelectButton button)
        {
            OnCharacterIconReleased?.Invoke(characterId, button);
        }

        public void RequestApplyFilter(CharacterFilterState filter)
        {
            OnFilterRequested?.Invoke(filter);
        }

        public void TeamReset()
        {
            characterFilterPanel?.ResetAllFilter();
        }

        private void CacheLineupSlots()
        {
            lineupSlots = new LineupSlot[slotObjects.Length];
            for (int i = 0; i < slotObjects.Length; i++)
                lineupSlots[i] = slotObjects[i] != null ? slotObjects[i].GetComponent<LineupSlot>() : null;
        }

        private void RegisterTeamButtons()
        {
            if (teamSelectButtons == null) return;

            foreach (TeamSelectButton button in teamSelectButtons)
                if (button != null) button.OnTeamSelected += TeamSelected;
        }

        private void TeamSelected(int index)
        {
            OnTeamButtonClicked?.Invoke(index);
        }
    }
}