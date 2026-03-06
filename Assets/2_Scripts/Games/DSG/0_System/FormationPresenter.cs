using OpenCvSharp.Flann;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LUP.DSG
{
    public class FormationPresenter : MonoBehaviour
    {
        [SerializeField]
        private FormationView view;

        private ICharacterFactory characterFactory;
        private DeckStrategyStage stage;

        private Team currentTeam;
        private int selectedCount = 0;
        private CharacterFilterState currentFilter = null;
        private Dictionary<int, OwnedCharacterInfo> ownedInfoDict = new Dictionary<int, OwnedCharacterInfo>();

        public event Action OnPowerUpdated;

        private void OnEnable()
        {
            StageInitializeInvoker.OnDSGStagePostInitialize += OnStagePostInitialize;
        }

        private void OnDisable()
        {
            StageInitializeInvoker.OnDSGStagePostInitialize -= OnStagePostInitialize;

            if (view != null)
            {
                view.OnCharacterIconSelected -= PlaceCharacter;
                view.OnCharacterIconReleased -= ReleaseCharacter;
                view.OnTeamButtonClicked -= ChangeAndLoadTeam;
                view.OnFilterRequested -= ApplyFilter;
            }
        }

        private void OnStagePostInitialize(DeckStrategyStage stage)
        {
            if (stage == null) return;
            this.stage = stage;
            characterFactory = new CharacterFactory(stage);

            DeckStrategyRuntimeData runtimeData = stage.DSGRuntimeData;
            if (runtimeData == null) return;

            BattleSystem battleSystem = stage.GetBattleSystem();
            if (battleSystem != null)
                OnPowerUpdated += battleSystem.UpdatePlayerCP;

            LoadTeam(runtimeData.SelectedTeamIndex);

            ToggleGroup toggleGroup = FindAnyObjectByType<ToggleGroup>();
            if (toggleGroup)
            {
                TeamSelectButton[] teamButtons = toggleGroup.GetComponentsInChildren<TeamSelectButton>(true);
                int idx = runtimeData.SelectedTeamIndex;

                if (idx >= 0 && idx < teamButtons.Length)
                    teamButtons[idx].ButtonStateChange(true);
            }

            ownedInfoDict.Clear();
            List<OwnedCharacterInfo> ownedList = runtimeData.OwnedCharacterList;
            if(ownedList != null)
            {
                for(int i = 0; i < ownedList.Count; ++i)
                {
                    ownedInfoDict.Add(ownedList[i].characterID, ownedList[i]);
                }
            }

            if (view != null)
            {
                view.OnCharacterIconSelected += PlaceCharacter;
                view.OnCharacterIconReleased += ReleaseCharacter;
                view.OnTeamButtonClicked += ChangeAndLoadTeam;
                view.OnFilterRequested += ApplyFilter;
            }
        }

        public void LoadTeam(int teamIndex)
        {
            if (stage == null) return;

            DeckStrategyRuntimeData runtimeData = stage.DSGRuntimeData;
            if (runtimeData != null)
                runtimeData.SelectedTeamIndex = teamIndex;

            currentTeam = stage.GetSelectedTeam();
            selectedCount = 0;

            if (currentTeam == null || currentTeam.characters == null || view == null) return;

            view.UpdateSelectedTeamButtonUI(teamIndex);
            RefreshCharacterListUI();

            RefreshAllSlots();
            
            view.TeamReset();
        }

        public void SaveCurrentTeam()
        {
            DeckStrategyRuntimeData runtimeData = stage?.DSGRuntimeData;
            if (runtimeData != null && runtimeData.Teams != null)
                runtimeData.Teams[runtimeData.SelectedTeamIndex] = currentTeam;
        }

        private void ChangeAndLoadTeam(int newTeamIndex)
        {
            stage.ChangeSelectedTeam(newTeamIndex);
            LoadTeam(newTeamIndex);
        }

        private void RefreshAllSlots()
        {
            if (view.lineupSlots == null) return;

            for (int i = 0; i < view.lineupSlots.Length; ++i)
            {
                LineupSlot slotView = view.lineupSlots[i];
                if (slotView == null) continue;

                slotView.ClearCharacter();

                OwnedCharacterInfo info = (i < currentTeam.characters.Length) ? currentTeam.characters[i] : null;
                if (info == null || info.characterID == 0) continue;

                // 팩토리를 통한 생성 및 View 갱신
                Character newChar = characterFactory.CreateCharacter(info, slotView.transform, false);
                slotView.SetCharacter(newChar);
                selectedCount++;
            }
            OnPowerUpdated?.Invoke();
        }

        private void PlaceCharacter(int characterId, CharacterSelectButton button)
        {
            if (selectedCount >= 5 || view == null) return;

            for (int i = 0; i < view.lineupSlots.Length; ++i)
            {
                LineupSlot slotView = view.lineupSlots[i];
                if (slotView == null || slotView.isPlaced) continue;

                ownedInfoDict.TryGetValue(characterId, out OwnedCharacterInfo characterInfo);
                if (characterInfo == null) continue;

                currentTeam.characters[i] = characterInfo;
                Character newChar = characterFactory.CreateCharacter(characterInfo, slotView.transform, false);
                slotView.SetCharacter(newChar);

                selectedCount++;
                button.ButtonClicked();
                SoundManager.Instance.PlaySFX("Inventory Stash 2");

                SaveCurrentTeam();
                return;
            }
        }

        private void ReleaseCharacter(int characterId, CharacterSelectButton button)
        {
            if (selectedCount <= 0 || view == null) return;

            for (int i = 0; i < view.lineupSlots.Length; ++i)
            {
                LineupSlot slotView = view.lineupSlots[i];
                if (slotView == null || !slotView.isPlaced || slotView.character == null) continue;
                if (slotView.character.characterData == null || 
                    slotView.character.characterData.ID != characterId) continue;

                currentTeam.characters[i] = null;
                slotView.ClearCharacter();

                selectedCount--;
                button.ButtonClicked();
                SoundManager.Instance.PlaySFX("Inventory Stash 2");

                SaveCurrentTeam();
                return;
            }
        }

        private void ApplyFilter(CharacterFilterState filter)
        {
            currentFilter = filter;
            RefreshCharacterListUI();
        }

        private void RefreshCharacterListUI()
        {
            DeckStrategyRuntimeData runtimeData = stage?.DSGRuntimeData;
            if (runtimeData == null || runtimeData.OwnedCharacterList == null) return;

            List<OwnedCharacterInfo> filteredList = new List<OwnedCharacterInfo>();

            foreach (OwnedCharacterInfo info in runtimeData.OwnedCharacterList)
            {
                if (info == null) continue;

                if (currentFilter != null && currentFilter.ContainsCheckedFilters())
                {
                    CharacterData data = stage.FindCharacterData(info.characterID, info.characterLevel);
                    if (data == null) continue;

                    bool matchAttribute = currentFilter.checkedAttributes.Count == 0 || currentFilter.checkedAttributes.Contains(data.type);
                    bool matchRange = currentFilter.checkedRanges.Count == 0 || currentFilter.checkedRanges.Contains(data.rangeType);

                    if (!matchAttribute || !matchRange) continue;
                }

                filteredList.Add(info);
            }

            if (view != null)
            {
                view.UpdateCharacterListUI(filteredList, currentTeam, stage);
            }
        }
    }
}