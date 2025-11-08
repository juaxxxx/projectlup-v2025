using ST;
using System.Collections;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

namespace LUP.DSG
{
    public class FormationSystem : MonoBehaviour
    {
        [SerializeField]
        DataCenter dataCenter;

        [SerializeField]
        private TeamDataTable teamDataTable;

        public GameObject[] slots = new GameObject[5];

        public UserData.Team selectedTeam { get; private set; }

        [SerializeField]
        private Transform characterListContent;
        [SerializeField]
        private CharacterFilterPanel filterPanel;

        public int selectedTeamIndex = 0;
        public int selectedCount = 0;

        public delegate void CharacterPlacedHandler(int slotIndex, CharacterData characterBase);
        public delegate void CharacterReleasedHandler(int slotIndex);

        public CharacterPlacedHandler placedHandler;
        public CharacterReleasedHandler releaseHandler;

        void Start()
        {
            //for(int i = 0; i < slots.Length; ++i)
            //{
            //    CreateSlot(i);
            //}
        }

        private void CreateSlot(int index)
        {

        }

        public void PlaceTeam(int teamIndex)
        {
            selectedTeamIndex = teamIndex;
            if (teamDataTable.teams[selectedTeamIndex] == null) return;
            ResetCharacterList(teamDataTable.teams[selectedTeamIndex]);

            selectedCount = 0;
            selectedTeam = teamDataTable.teams[selectedTeamIndex];

            ApplyPlaceTeam();
        }

        public void ApplyPlaceTeam()
        {
            for (int i = 0; i < slots.Length; ++i)
            {
                LineupSlot slot = slots[i].GetComponent<LineupSlot>();
                slot.DeselectCharacter();

                if (selectedTeam.characters[i].characterID == 0) continue;

                CharacterIcon[] icons = characterListContent.GetComponentsInChildren<CharacterIcon>();
                foreach (var icon in icons)
                {
                    if (icon.characterInfo.characterID == selectedTeam.characters[i].characterID)
                    {
                        if (icon.selectedButton.isSelected)
                        {
                            ReleaseCharacter(icon.characterInfo.characterID, icon.selectedButton);
                            icon.selectedSlot = -1;
                        }
                        else
                        {
                            PlaceCharacterInPlaceTeam(icon.characterInfo, icon.selectedButton, i);
                            icon.selectedSlot = i;
                        }

                        break;
                    }
                }
            }
        }

        public void PlaceCharacterInPlaceTeam(OwnedCharacterInfo info, SelectedButton button, int slotIndex)
        {
            if (selectedCount >= 5 || slotIndex == -1) return;

            LineupSlot slot = slots[slotIndex].GetComponent<LineupSlot>();
            if (!slot.isPlaced)
            {
                slot.SetSelectedCharacter(info, false);
                selectedTeam.characters[slotIndex] = info;
                ++selectedCount;
                Debug.Log("characterID: " + selectedTeam.characters[slotIndex].characterID);
                button.ButtonClicked();
            }
        }

        public void PlaceCharacter(OwnedCharacterInfo info, SelectedButton button)
        {
            if (selectedCount >= 5) return;

            for(int i = 0; i < slots.Length; ++i)
            {
                LineupSlot slot = slots[i].GetComponent<LineupSlot>();
                if (!slot.isPlaced)
                {
                    slot.SetSelectedCharacter(info, false);
                    selectedTeam.characters[i] = info;
                    ++selectedCount;
                    Debug.Log("characterID: " + selectedTeam.characters[i].characterID);
                    button.ButtonClicked();
                    return;
                }
            }
        }

        public void ReleaseCharacter(int characterID, SelectedButton button)
        {
            if (selectedCount <= 0) return;

            for (int i = 0; i < slots.Length; ++i)
            {
                LineupSlot slot = slots[i].GetComponent<LineupSlot>();
                if (slot.character == null || slot.character.characterData == null) continue;
                if (slot.character.characterData.ID == characterID)
                {
                    slot.DeselectCharacter();
                    selectedTeam.characters[i] = null;

                    --selectedCount;
                    button.ButtonClicked();
                    return;
                }
            }
        }

        private void ResetCharacterList(UserData.Team team)
        {
            CharactersList list = characterListContent.GetComponentInParent<CharactersList>();
            if (list != null)
            {
                list.ResetSelectedStatus();
                foreach (OwnedCharacterInfo info in team.characters)
                {
                    list.UpdateCheckedList(info.characterID, true);
                }
                list.PopulateScrollView();
            }
            filterPanel.ResetAllFilter();
        }

        public void SaveTeam()
        {
            for (int i = 0; i < slots.Length; ++i)
            {
                LineupSlot slot = slots[i].GetComponent<LineupSlot>();
                if (teamDataTable.teams[selectedTeamIndex] == null) teamDataTable.teams[selectedTeamIndex] = new UserData.Team();
                teamDataTable.teams[selectedTeamIndex] = selectedTeam;
            }
#if UNITY_EDITOR
            EditorUtility.SetDirty(teamDataTable);
            AssetDatabase.SaveAssets();
#endif
        }
    }

}