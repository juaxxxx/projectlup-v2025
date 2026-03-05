using OpenCvSharp.Flann;
using System;
using UnityEngine;

namespace LUP.DSG
{
    public class FormationPresenter : MonoBehaviour
    {
        private FormationView view;
        private ICharacterFactory characterFactory;
        private DeckStrategyStage stageModel;

        private Team currentTeam;
        private int selectedCount = 0;

        public event Action OnPowerUpdated;

        public FormationPresenter(FormationView view, ICharacterFactory factory, DeckStrategyStage stage)
        {
            this.view = view;
            characterFactory = factory;
            stageModel = stage;

            // View의 이벤트 구독
            this.view.OnCharacterSlotClicked += HandlePlaceCharacter;
            this.view.OnReleaseSlotClicked += HandleReleaseCharacter;
            this.view.OnTeamButtonClicked += ChangeAndLoadTeam;
        }

        ~FormationPresenter()
        {
            view.OnCharacterSlotClicked -= HandlePlaceCharacter;
            view.OnReleaseSlotClicked -= HandleReleaseCharacter;
            view.OnTeamButtonClicked -= ChangeAndLoadTeam;
        }

        public void LoadTeam(int teamIndex)
        {
            if (stageModel == null) return;

            DeckStrategyRuntimeData runtimeData = stageModel.RuntimeData as DeckStrategyRuntimeData;
            if (runtimeData != null)
                runtimeData.SelectedTeamIndex = teamIndex;

            currentTeam = stageModel.GetSelectedTeam();
            selectedCount = 0;

            if (currentTeam == null || currentTeam.characters == null) return;

            view.UpdateSelectedTeamTabUI(teamIndex);
            view.UpdateCharacterListUI(currentTeam);
            RefreshAllSlots();
        }

        private void RefreshAllSlots()
        {
            for (int i = 0; i < view.lineupSlots.Length; ++i)
            {
                var slotView = view.lineupSlots[i];
                if (slotView == null) continue;

                slotView.ClearCharacter();

                OwnedCharacterInfo info = (i < currentTeam.characters.Length) ? currentTeam.characters[i] : null;
                if (info == null || info.characterID == 0) continue;

                // 팩토리를 통한 생성 및 View 갱신
                Character newChar = characterFactory.CreateCharacter(info, slotView.slotTransform, false);
                slotView.SetCharacterView(newChar);
                selectedCount++;
            }
            OnPowerUpdated?.Invoke();
        }

        private void HandlePlaceCharacter(OwnedCharacterInfo info, CharacterSelectButton button)
        {
            if (selectedCount >= 5) return;

            for (int i = 0; i < view.lineupSlots.Length; ++i)
            {
                LineupSlot slotView = view.lineupSlots[i];
                if (slotView == null || slotView.isPlaced) continue;

                // 1. Model 데이터 업데이트
                currentTeam.characters[i] = info;

                // 2. Factory를 통한 객체 생성
                Character newChar = characterFactory.CreateCharacter(info, slotView.slotTransform, false);

                // 3. View 업데이트 명령
                slotView.SetCharacterView(newChar);

                selectedCount++;
                button.ButtonClicked();
                view.PlayEquipSound();

                SaveCurrentTeam();
                return;
            }
        }

        private void HandleReleaseCharacter(int slotIndex)
        {
            if (selectedCount <= 0 || slotIndex < 0 || slotIndex >= view.lineupSlots.Length) return;

            LineupSlot slotView = view.lineupSlots[slotIndex];
            if (slotView == null || !slotView.isPlaced) return;

            // Model 업데이트
            currentTeam.characters[slotIndex] = null;

            // View 업데이트
            slotView.ClearCharacter();

            selectedCount--;
            view.PlayEquipSound();
            SaveCurrentTeam();
        }

        public void SaveCurrentTeam()
        {
            DeckStrategyRuntimeData runtimeData = stageModel.RuntimeData as DeckStrategyRuntimeData;
            if (runtimeData != null && runtimeData.Teams != null)
                runtimeData.Teams[runtimeData.SelectedTeamIndex] = currentTeam;
        }

        private void ChangeAndLoadTeam(int newTeamIndex)
        {
            // 1. Stage 모델의 데이터 변경
            stageModel.ChangeSelectedTeam(newTeamIndex);

            // 2. 변경된 데이터로 필드(슬롯) 및 UI 새로고침
            LoadTeam(newTeamIndex);
        }
    }
}