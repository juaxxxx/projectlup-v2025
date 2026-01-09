using LUP.DSG.Utils.Enums;
using LUP;
using NUnit.Framework.Interfaces;
using Roguelike.Define;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.TextCore.Text;

namespace LUP.DSG
{
    public class CharactersList : MonoBehaviour
    {
        [SerializeField]
        private GameObject iconPrefab;
        [SerializeField]
        private Transform contentParent;

        Dictionary<int, bool> selectedOwnedMap = new Dictionary<int, bool>();

        private void Awake()
        {
            StageInitializeInvoker.OnDSGStagePostInitialize += PostInitialize;
        }
        private void OnDestroy()
        {
            StageInitializeInvoker.OnDSGStagePostInitialize -= PostInitialize;
        }

        private void PostInitialize(DeckStrategyStage stage)
        {
            if (stage != null)
            {
                DeckStrategyRuntimeData runtimeData = (DeckStrategyRuntimeData)stage.RuntimeData;
                if (runtimeData == null || runtimeData.OwnedCharacterList.Count == 0) return;
                List<OwnedCharacterInfo> characterList = runtimeData.OwnedCharacterList;

                selectedOwnedMap.Clear();
                foreach (var info in characterList)
                {
                    if (info == null) continue;
                    selectedOwnedMap[info.characterID] = false;
                }
            }
        }

        public void ResetSelectedStatus()
        {
            var keys = new List<int>(selectedOwnedMap.Keys);
            foreach (var key in keys)
            {
                selectedOwnedMap[key] = false;
            }
        }
        public void PopulateScrollView()
        {
            foreach (Transform child in contentParent)
            {
                Destroy(child.gameObject);
                child.gameObject.SetActive(false);
            }

            DeckStrategyStage stage = LUP.StageManager.Instance.GetCurrentStage() as DeckStrategyStage;
            if (stage == null) return;
            DeckStrategyRuntimeData runtimeData = (DeckStrategyRuntimeData)stage.RuntimeData;
            if (runtimeData == null || runtimeData.Teams.Count == 0) return;

            List<OwnedCharacterInfo> characterList = runtimeData.OwnedCharacterList;
            if (characterList == null) return;

            foreach (OwnedCharacterInfo character in characterList)
            {
                var characterData = stage.FindCharacterData(character.characterID, character.characterLevel);
                AddCharacterIcon(character, characterData.type);
            }
        }

        public void RePopulateThroughFilter(CharacterFilterState filterState = null)
        {
            foreach (Transform child in contentParent)
            {
                Destroy(child.gameObject);
                child.gameObject.SetActive(false);
            }

            DeckStrategyStage stage = LUP.StageManager.Instance.GetCurrentStage() as DeckStrategyStage;
            if (stage == null) return;

            DeckStrategyRuntimeData runtimeData = (DeckStrategyRuntimeData)stage.RuntimeData;
            if (runtimeData == null || runtimeData.OwnedCharacterList.Count == 0) return;

            List<OwnedCharacterInfo> characterList = runtimeData.OwnedCharacterList;
            if (characterList == null) return;

            if (filterState == null)
            {
                foreach (OwnedCharacterInfo character in characterList)
                {
                    var characterData = stage.FindCharacterData(character.characterID, character.characterLevel);
                    AddCharacterIcon(character, characterData.type);
                }

                CharacterIcon[] icons = contentParent.GetComponentsInChildren<CharacterIcon>();
                foreach (var icon in icons)
                {
                    if (icon == null || icon.characterInfo == null) continue;

                    int charId = icon.characterInfo.characterID;

                    if (selectedOwnedMap.TryGetValue(charId, out bool isSelected) && isSelected)
                    {
                        icon.selectedButton.ButtonClicked();
                    }
                }
            }
            else
            {
                foreach (OwnedCharacterInfo character in characterList)
                {
                    var characterData = stage.FindCharacterData(character.characterID, character.characterLevel);
                    if (!filterState.checkedAttributes.Contains(characterData.type) &&
                        !filterState.checkedRanges.Contains(characterData.rangeType))
                    {
                        continue;
                    }
                    AddCharacterIcon(character, characterData.type);
                }

                CharacterIcon[] icons = contentParent.GetComponentsInChildren<CharacterIcon>();
                foreach (var icon in icons)
                {
                    if (icon == null || icon.characterInfo == null) continue;

                    int charId = icon.characterInfo.characterID;

                    if (selectedOwnedMap.TryGetValue(charId, out bool isSelected) && isSelected)
                    {
                        icon.selectedButton.ButtonClicked();
                    }
                }
            }
        }

        private void AddCharacterIcon(OwnedCharacterInfo characterInfo, EAttributeType type)
        {
            var itemUI = Instantiate(iconPrefab, contentParent.transform);
            var icon = itemUI.GetComponent<CharacterIcon>();

            DeckStrategyStage stage = LUP.StageManager.Instance.GetCurrentStage() as DeckStrategyStage;
            if (stage == null) return;

            var modelData = stage.FindCharacterModel(characterInfo.characterModelID);
            icon.Init();
            icon.SetIconData(characterInfo, type, modelData.material.color, characterInfo.characterLevel, false);
        }

        public void UpdateCheckedList(int index, bool isChecked)
        {
            if (index == 0) return;

            if (!selectedOwnedMap.ContainsKey(index))
            {
                return;
            }

            selectedOwnedMap[index] = isChecked;
        }
    }
}