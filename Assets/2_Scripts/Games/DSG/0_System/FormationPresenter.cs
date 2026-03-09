using LUP.DSG.Utils.Enums;
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

        private CharacterFactory characterFactory;
        private DeckStrategyStage stage;

        private Team currentTeam;
        private int selectedCount = 0;
        private CharacterFilterState currentFilter = null;

        private Dictionary<int, CharacterInfo> ownedInfoDict = new Dictionary<int, CharacterInfo>();
        private readonly List<CharacterInfo> filteredList = new List<CharacterInfo>();

        public event Action OnPowerUpdated;

        private void OnEnable()
        {
            StageInitializeInvoker.OnDSGStagePostInitialize += OnStagePostInitialize;
        }

        private void OnDisable()
        {
            StageInitializeInvoker.OnDSGStagePostInitialize -= OnStagePostInitialize;

            if (view)
            {
                view.OnCharacterIconSelected -= PlaceCharacter;
                view.OnCharacterIconReleased -= ReleaseCharacter;
                view.OnTeamButtonClicked -= ChangeAndLoadTeam;
                view.OnFilterRequested -= ApplyFilter;
            }
        }

        private void OnStagePostInitialize(DeckStrategyStage stage)
        {
            if (!stage) return;
            this.stage = stage;
            characterFactory = new CharacterFactory(stage);

            DeckStrategyRuntimeData runtimeData = stage.DSGRuntimeData;
            if (runtimeData == null) return;

            BattleSystem battleSystem = stage.GetBattleSystem();
            if (battleSystem)
                OnPowerUpdated += battleSystem.UpdatePlayerCP;

            LoadTeam(runtimeData.SelectedTeamIndex);

            ownedInfoDict.Clear();
            List<CharacterInfo> ownedList = runtimeData.OwnedCharacterList;
            if(ownedList != null)
            {
                for(int i = 0; i < ownedList.Count; ++i)
                    ownedInfoDict.Add(ownedList[i].characterID, ownedList[i]);
            }

            if (view)
            {
                view.OnCharacterIconSelected += PlaceCharacter;
                view.OnCharacterIconReleased += ReleaseCharacter;
                view.OnTeamButtonClicked += ChangeAndLoadTeam;
                view.OnFilterRequested += ApplyFilter;
            }
        }

        public void LoadTeam(int teamIndex)
        {
            if (!stage) return;

            DeckStrategyRuntimeData runtimeData = stage.DSGRuntimeData;
            if (runtimeData != null)
                runtimeData.SelectedTeamIndex = teamIndex;

            currentTeam = stage.GetSelectedTeam();
            selectedCount = 0;

            if (currentTeam == null || currentTeam.characters == null || !view) return;

            view.UpdateSelectedTeamButtonUI(teamIndex);
            currentFilter = null;
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
                if (!slotView) continue;

                if (slotView.character)
                    characterFactory.ReturnCharacter(slotView.character);
                slotView.ClearCharacter();

                CharacterInfo info = (i < currentTeam.characters.Length) ? currentTeam.characters[i] : null;
                if (info == null || info.characterID == 0) continue;

                Character newCharacter = characterFactory.GetCharacter(info, slotView.transform, false);
                slotView.SetCharacter(newCharacter);
                selectedCount++;
            }
            OnPowerUpdated?.Invoke();
        }

        private void PlaceCharacter(int characterId, CharacterSelectButton button)
        {
            if (selectedCount >= 5 || !view) return;

            for (int i = 0; i < view.lineupSlots.Length; ++i)
            {
                LineupSlot slotView = view.lineupSlots[i];
                if (!slotView || slotView.isPlaced) continue;

                ownedInfoDict.TryGetValue(characterId, out CharacterInfo characterInfo);
                if (characterInfo == null) continue;

                currentTeam.characters[i] = characterInfo;
                Character newCharacter = characterFactory.GetCharacter(characterInfo, slotView.transform, false);
                slotView.SetCharacter(newCharacter);

                selectedCount++;
                button.ButtonClicked();
                SoundManager.Instance.PlaySFX("Inventory Stash 2");

                SaveCurrentTeam();
                return;
            }
        }

        private void ReleaseCharacter(int characterId, CharacterSelectButton button)
        {
            if (selectedCount <= 0 || !view) return;

            for (int i = 0; i < view.lineupSlots.Length; ++i)
            {
                LineupSlot slotView = view.lineupSlots[i];
                if (!slotView || !slotView.isPlaced || !slotView.character) continue;
                if (slotView.character.characterData == null || 
                    slotView.character.characterData.ID != characterId) continue;

                currentTeam.characters[i] = null;

                characterFactory.ReturnCharacter(slotView.character);
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

            filteredList.Clear();

            bool hasFilters = currentFilter != null && currentFilter.ContainsCheckedFilters();
            foreach (CharacterInfo info in runtimeData.OwnedCharacterList)
            {
                if (info == null) continue;

                if (hasFilters)
                {
                    CharacterData data = stage.FindCharacterData(info.characterID, info.characterLevel);
                    if (data == null) continue;

                    bool matchAttribute = currentFilter.CheckFilterMatch(data.type);
                    bool matchRange = currentFilter.CheckFilterMatch(data.rangeType);

                    if (!matchAttribute || !matchRange) continue;
                }

                filteredList.Add(info);
            }

            if (view)
                view.UpdateCharacterListUI(filteredList, currentTeam, stage);
        }
    }
}