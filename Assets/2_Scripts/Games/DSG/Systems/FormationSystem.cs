using LUP.DSG.Utils.Enums;
using System.Collections;
using System.Drawing;
using Unity.VisualScripting;
//using UnityEditor;
using UnityEngine;

namespace LUP.DSG
{
    public readonly struct AttributeTypeImage
    {
        public readonly Sprite TypeIcon;
        public readonly UnityEngine.Color TypeColor;
        public AttributeTypeImage(Sprite icon, UnityEngine.Color color)
        {
            TypeIcon = icon;
            TypeColor = color;
        }
    }
    public class FormationSystem : MonoBehaviour
    {
        public GameObject[] slots = new GameObject[5];

        public Team selectedTeam { get; private set; }

        [SerializeField]
        private Transform characterListContent;
        [SerializeField]
        private CharacterFilterPanel filterPanel;
        [SerializeField]
        private RectTransform AttributeChart;

        [SerializeField] private Sprite paperIcon, rockIcon, scissorsIcon;

        public int selectedTeamIndex = 0;
        public int selectedCount = 0;

        public delegate void CharacterPlacedHandler(int slotIndex, CharacterData characterBase);
        public delegate void CharacterReleasedHandler(int slotIndex);

        public CharacterPlacedHandler placedHandler;
        public CharacterReleasedHandler releaseHandler;

        public event System.Action OnPowerUpdated;

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
            // 이 시점에는 LineupSlot.Initialize 가 이미 실행되어
            // 각 slot.character 가 null 이 아님
            PlaceTeam(selectedTeamIndex);  // 기본 0번 팀 같은 것
        }
        public void PlaceTeam(int teamIndex)
        {
            selectedTeamIndex = teamIndex;
            DeckStrategyStage stage = LUP.StageManager.Instance.GetCurrentStage() as DeckStrategyStage;
            if (stage != null)
            {
                DeckStrategyRuntimeData runtimeData = (DeckStrategyRuntimeData)stage.RuntimeData;
                if(runtimeData == null || runtimeData.Teams.Count == 0)
                {
                    runtimeData.Teams[selectedTeamIndex] = new Team();
                }

                if(characterListContent != null)
                {
                    ResetCharacterList(runtimeData.Teams[selectedTeamIndex]);
                }

                selectedCount = 0;
                selectedTeam = runtimeData.Teams[selectedTeamIndex];
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
            if(filterPanel != null)
            {
                filterPanel.ResetAllFilter();
            }
        }

        public void SaveTeam()
        {
            DeckStrategyStage stage = LUP.StageManager.Instance.GetCurrentStage() as DeckStrategyStage;
            if (stage == null) return;
            DeckStrategyRuntimeData runtimeData = (DeckStrategyRuntimeData)stage.RuntimeData;
            if (runtimeData == null || runtimeData.Teams == null) return;

            if (runtimeData.Teams[selectedTeamIndex] == null) runtimeData.Teams[selectedTeamIndex] = new Team();
            runtimeData.Teams[selectedTeamIndex] = selectedTeam;
            //for (int i = 0; i < slots.Length; ++i)
            //{
            //    LineupSlot slot = slots[i].GetComponent<LineupSlot>();
            //}
        }

        public AttributeTypeImage GetTypeByAttributeImage(EAttributeType type)
        {
            switch (type)
            {
                case EAttributeType.ROCK:
                    return new AttributeTypeImage(rockIcon, UnityEngine.Color.yellow);
                case EAttributeType.PAPER:
                    return new AttributeTypeImage(paperIcon, UnityEngine.Color.red);
                case EAttributeType.SCISSORS:
                    return new AttributeTypeImage(scissorsIcon, UnityEngine.Color.blue);
                default:
                    return new AttributeTypeImage(null, UnityEngine.Color.white);
            }
        }

        public void OnClickAttributetable()
        {
            AttributeChart.gameObject.SetActive(true);
        }
        public void OnClickAttributeChartExit()
        {
            AttributeChart.gameObject.SetActive(false);
        }
    }

}