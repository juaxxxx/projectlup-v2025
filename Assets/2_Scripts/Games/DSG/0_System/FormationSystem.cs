using LUP.DSG.Utils.Enums;
using System.Collections;
using System.Drawing;
using System.Linq;
using Unity.VisualScripting;
//using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace LUP.DSG
{
    public class FormationSystem : MonoBehaviour
    {
        public GameObject[] slots = new GameObject[5];

        public Team selectedTeam { get; private set; }

        [SerializeField]
        private Transform characterListContent;

        private int selectedCount = 0;

        public delegate void CharacterPlacedHandler(int slotIndex, CharacterData characterBase);
        public delegate void CharacterReleasedHandler(int slotIndex);

        public CharacterPlacedHandler placedHandler;
        public CharacterReleasedHandler releaseHandler;

        public event System.Action OnPowerUpdated;

        public event System.Action<int> OnInitTeam;
        public event System.Action OnResetTeam;

        private void OnEnable()
        {
            StageInitializeInvoker.OnDSGStagePostInitialize += OnStagePostInitialize;
        }

        private void OnDisable()
        {
            StageInitializeInvoker.OnDSGStagePostInitialize -= OnStagePostInitialize;
        }

        private void OnStagePostInitialize(DeckStrategyStage stage)
        {
            if (stage == null) return;
            DeckStrategyRuntimeData runtimeData = (DeckStrategyRuntimeData)stage.RuntimeData;
            if (runtimeData == null) return;
            PlaceTeam(runtimeData.SelectedTeamIndex);
            OnInitTeam?.Invoke(runtimeData.SelectedTeamIndex);
            ToggleGroup toggleGroup = FindAnyObjectByType<ToggleGroup>();
            if(toggleGroup)
            {
                TeamSelectButton selectedToggle = toggleGroup.GetComponentsInChildren<TeamSelectButton>().ElementAtOrDefault(runtimeData.SelectedTeamIndex);
                selectedToggle.ButtonStateChange(true);
            }
        }
        public void PlaceTeam(int index)
        {
            DeckStrategyStage stage = LUP.StageManager.Instance.GetCurrentStage() as DeckStrategyStage;
            if (stage != null)
            {
                if(characterListContent != null)
                {
                    ResetCharacterList(stage.GetSelectedTeam());
                }

                DeckStrategyRuntimeData runtimeData = (DeckStrategyRuntimeData)stage.RuntimeData;
                if (runtimeData == null) return;
                runtimeData.SelectedTeamIndex = index;
                selectedCount = 0;
                selectedTeam = stage.GetSelectedTeam();
                ApplyPlaceTeam();
                OnPowerUpdated?.Invoke();
            }
        }
        
        public void ApplyPlaceTeam()
        {
            for (int i = 0; i < slots.Length; ++i)
            {
                LineupSlot slot = slots[i].GetComponent<LineupSlot>();
                if (slot.isPlaced)
                {
                    slot.DeselectCharacter();
                }

                if (selectedTeam.characters[i] == null || selectedTeam.characters[i].characterID == 0) continue;

                if(characterListContent != null)
                {
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
                else
                {
                    slot.SetSelectedCharacter(selectedTeam.characters[i], false);
                    ++selectedCount;
                }
            }
        }

        public void PlaceCharacterInPlaceTeam(OwnedCharacterInfo info, SelectedButton button, int slotIndex)
        {
            if (selectedCount >= 5 || slotIndex == -1) return;

            if (slotIndex < 0 || slotIndex >= slots.Length)
            {
                return;
            }

            var slotObj = slots[slotIndex];
            if (slotObj == null)
            {
                return;
            }

            LineupSlot slot = slotObj.GetComponent<LineupSlot>();
            if (slot == null)
            {
                return;
            }

            if (!slot.isPlaced)
            {
                slot.SetSelectedCharacter(info, false);
                selectedTeam.characters[slotIndex] = info;
                ++selectedCount;
                button.ButtonClicked();
            }
        }

        public void PlaceCharacter(OwnedCharacterInfo info, SelectedButton button)
        {
            if (selectedCount >= 5) return;

            for (int i = 0; i < slots.Length; ++i)
            {
                var slotObj = slots[i];
                if (slotObj == null) continue;

                LineupSlot slot = slotObj.GetComponent<LineupSlot>();
                if (slot == null) continue;

                if (!slot.isPlaced)
                {
                    slot.SetSelectedCharacter(info, false);
                    selectedTeam.characters[i] = info;
                    ++selectedCount;
                    button.ButtonClicked();
                    SoundManager.Instance.PlaySFX("Inventory Stash 2");
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
                    SoundManager.Instance.PlaySFX("Inventory Stash 2");
                    return;
                }
            }
        }

        private void ResetCharacterList(Team team)
        {
            CharactersList list = characterListContent.GetComponentInParent<CharactersList>();
            if (list != null)
            {
                list.ResetSelectedStatus();
                foreach (OwnedCharacterInfo info in team.characters)
                {
                    if (info == null) continue;
                    list.UpdateCheckedList(info.characterID, true);
                }
                list.PopulateScrollView();
            }

            OnResetTeam?.Invoke();
        }

        public void SaveTeam()
        {
            DeckStrategyStage stage = LUP.StageManager.Instance.GetCurrentStage() as DeckStrategyStage;
            if (stage == null) return;
            DeckStrategyRuntimeData runtimeData = (DeckStrategyRuntimeData)stage.RuntimeData;
            if (runtimeData == null || runtimeData.Teams == null) return;

            if (runtimeData.Teams[runtimeData.SelectedTeamIndex] == null) runtimeData.Teams[runtimeData.SelectedTeamIndex] = new Team();
            runtimeData.Teams[runtimeData.SelectedTeamIndex] = selectedTeam;
        }
    }

}