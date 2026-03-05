using System;
using UnityEngine;
using UnityEngine.UI;

namespace LUP.DSG
{
    //public class FormationSystem : MonoBehaviour
    //{
    //    public GameObject[] slots = new GameObject[5];

    //    public Team selectedTeam { get; private set; }

    //    [SerializeField]
    //    private Transform characterListContent;

    //    private int selectedCount = 0;

    //    public delegate void CharacterPlacedHandler(int slotIndex, CharacterData characterBase);
    //    public delegate void CharacterReleasedHandler(int slotIndex);

    //    public CharacterPlacedHandler placedHandler;
    //    public CharacterReleasedHandler releaseHandler;

    //    public event Action OnPowerUpdated;
    //    public event Action OnResetTeam;

    //    private LineupSlot[] lineupSlots;
    //    private CharactersList charactersList;

    //    private void OnEnable()
    //    {
    //        StageInitializeInvoker.OnDSGStagePostInitialize += OnStagePostInitialize;
    //    }

    //    private void OnDisable()
    //    {
    //        StageInitializeInvoker.OnDSGStagePostInitialize -= OnStagePostInitialize;
    //    }

    //    private void OnStagePostInitialize(DeckStrategyStage stage)
    //    {
    //        if (stage == null) return;

    //        DeckStrategyRuntimeData runtimeData = (DeckStrategyRuntimeData)stage.RuntimeData;
    //        if (runtimeData == null) return;

    //        CacheLineupSlots();

    //        PlaceTeam(runtimeData.SelectedTeamIndex);

    //        ToggleGroup toggleGroup = FindAnyObjectByType<ToggleGroup>();
    //        if (toggleGroup)
    //        {
    //            TeamSelectButton[] teamButtons = toggleGroup.GetComponentsInChildren<TeamSelectButton>(true);
    //            int idx = runtimeData.SelectedTeamIndex;

    //            if (idx >= 0 && idx < teamButtons.Length)
    //                teamButtons[idx].ButtonStateChange(true);
    //        }
    //    }
    //    public void PlaceTeam(int index)
    //    {
    //        DeckStrategyStage stage = LUP.StageManager.Instance.GetCurrentStage() as DeckStrategyStage;
    //        if (stage == null) return;

    //        DeckStrategyRuntimeData runtimeData = stage.RuntimeData as DeckStrategyRuntimeData;
    //        if (runtimeData == null) return;

    //        runtimeData.SelectedTeamIndex = index;
    //        selectedTeam = stage.GetSelectedTeam();
    //        if (selectedTeam == null) return;

    //        selectedCount = 0;

    //        if (characterListContent != null)
    //        {
    //            ResetCharacterList(selectedTeam);
    //            ApplyPlaceTeam();
    //        }
    //        else
    //        {
    //            ApplyPlaceTeamInBattle();
    //            OnPowerUpdated?.Invoke();
    //        }
    //    }
    //    private void ApplyPlaceTeamInBattle()
    //    {
    //        if (lineupSlots == null || selectedTeam.characters == null)
    //            return;

    //        for (int i = 0; i < lineupSlots.Length; ++i)
    //        {
    //            LineupSlot slot = lineupSlots[i];
    //            if (slot == null) continue;

    //            if (slot.isPlaced) slot.DeselectCharacter();

    //            OwnedCharacterInfo info = (i < selectedTeam.characters.Length) ? selectedTeam.characters[i] : null;
    //            if (info == null || info.characterID == 0) continue;

    //            slot.SetSelectedCharacter(selectedTeam.characters[i], false);
    //            ++selectedCount;
    //        }
    //    }

    //    private void ApplyPlaceTeam()
    //    {
    //        if (lineupSlots == null || selectedTeam.characters == null)
    //            return;

    //        for (int i = 0; i < lineupSlots.Length; ++i)
    //        {
    //            LineupSlot slot = lineupSlots[i];
    //            if (slot == null) continue;

    //            if (slot.isPlaced) slot.DeselectCharacter();

    //            OwnedCharacterInfo info = (i < selectedTeam.characters.Length) ? selectedTeam.characters[i] : null;
    //            if (info == null || info.characterID == 0) continue;

    //            CharacterIcon icon = FindIdByList(selectedTeam.characters[i].characterID);
    //            if (icon && icon.selectedButton.isSelected)
    //            {
    //                PlaceCharacterInPlaceTeam(icon.characterInfo, icon.selectedButton, i);
    //                icon.selectedSlot = i;
    //            }
    //        }
    //    }

    //    public void PlaceCharacterInPlaceTeam(OwnedCharacterInfo info, CharacterSelectButton button, int slotIndex)
    //    {
    //        if (selectedCount >= 5 || slotIndex == -1) return;
    //        if (lineupSlots == null) return;
    //        if (slotIndex < 0 || slotIndex >= lineupSlots.Length) return;

    //        LineupSlot slot = lineupSlots[slotIndex];
    //        if (slot == null || !slot.isPlaced) return;

    //        slot.SetSelectedCharacter(info, false);
    //        selectedTeam.characters[slotIndex] = info;
    //        ++selectedCount;
    //    }

    //    public void PlaceCharacter(OwnedCharacterInfo info, CharacterSelectButton button)
    //    {
    //        if (selectedCount >= 5) return;
    //        if (lineupSlots == null) return;

    //        for (int i = 0; i < lineupSlots.Length; ++i)
    //        {
    //            LineupSlot slot = lineupSlots[i];
    //            if (slot == null) continue;

    //            if (!slot.isPlaced)
    //            {
    //                slot.SetSelectedCharacter(info, false);
    //                selectedTeam.characters[i] = info;
    //                ++selectedCount;
    //                button.ButtonClicked();
    //                SoundManager.Instance.PlaySFX("Inventory Stash 2");
    //                return;
    //            }
    //        }
    //    }

    //    public void ReleaseCharacter(int characterID, CharacterSelectButton button)
    //    {
    //        if (selectedCount <= 0) return;
    //        if (lineupSlots == null) return;

    //        for (int i = 0; i < lineupSlots.Length; ++i)
    //        {
    //            LineupSlot slot = lineupSlots[i];
    //            if (slot == null || slot.character == null || slot.character.characterData == null) continue;

    //            if (slot.character.characterData.ID == characterID)
    //            {
    //                slot.DeselectCharacter();
    //                selectedTeam.characters[i] = null;

    //                --selectedCount;
    //                button.ButtonClicked();
    //                SoundManager.Instance.PlaySFX("Inventory Stash 2");
    //                return;
    //            }
    //        }
    //    }

    //    private void ResetCharacterList(Team team)
    //    {
    //        CharactersList list = characterListContent != null ? characterListContent.GetComponentInParent<CharactersList>() : null;
    //        if (list != null)
    //        {
    //            list.ResetSelectedStatus();

    //            if (team != null && team.characters != null)
    //            {
    //                foreach (OwnedCharacterInfo info in team.characters)
    //                {
    //                    if (info == null) continue;
    //                    list.UpdateCheckedList(info.characterID, true);
    //                }
    //            }

    //            list.RePopulateThroughFilter();
    //        }

    //        OnResetTeam?.Invoke();
    //    }

    //    public void SaveTeam()
    //    {
    //        DeckStrategyStage stage = LUP.StageManager.Instance.GetCurrentStage() as DeckStrategyStage;
    //        if (stage == null) return;
    //        DeckStrategyRuntimeData runtimeData = (DeckStrategyRuntimeData)stage.RuntimeData;
    //        if (runtimeData == null || runtimeData.Teams == null) return;

    //        if (runtimeData.Teams[runtimeData.SelectedTeamIndex] == null) runtimeData.Teams[runtimeData.SelectedTeamIndex] = new Team();
    //        runtimeData.Teams[runtimeData.SelectedTeamIndex] = selectedTeam;
    //    }

    //    private void CacheLineupSlots()
    //    {
    //        if (slots == null)
    //        {
    //            lineupSlots = System.Array.Empty<LineupSlot>();
    //            return;
    //        }

    //        lineupSlots = new LineupSlot[slots.Length];
    //        for (int i = 0; i < slots.Length; i++)
    //        {
    //            GameObject go = slots[i];
    //            lineupSlots[i] = go != null ? go.GetComponent<LineupSlot>() : null;
    //        }
    //    }

    //    private CharacterIcon FindIdByList(int characterId)
    //    {
    //        CharacterIcon[] icons = characterListContent.GetComponentsInChildren<CharacterIcon>();
    //        foreach (CharacterIcon icon in icons)
    //        {
    //            if (icon.characterInfo.characterID == characterId)
    //            {
    //                return icon;
    //            }
    //        }

    //        return null;
    //    }
    //}
}