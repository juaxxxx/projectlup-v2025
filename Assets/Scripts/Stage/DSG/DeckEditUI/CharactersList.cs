using DSG.Utils.Enums;
using Manager;
using NUnit.Framework.Interfaces;
using Roguelike.Define;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.TextCore.Text;
using static DSG.ResultCharacterDisplay;

namespace DSG
{
    public class CharactersList : MonoBehaviour
    {
        [SerializeField]
        private DataCenter dataCenter;

        [SerializeField]
        private GameObject iconPrefab;
        [SerializeField]
        private Transform contentParent;

        List<bool> SelectedOwnedList = new List<bool>();

        private void Start()
        {
            List<OwnedCharacterInfo> characterList = dataCenter.GetOwnedCharacterList();
            for(int i = 0; i <= characterList.Count; ++i)
            {
                SelectedOwnedList.Add(false);
            }

            //DeckStrategyStage stage = Manager.StageManager.Instance.GetCurrentStage() as DeckStrategyStage;
            //if(stage != null)
            //{
            //    DeckStrategyRuntimeData runtimeData = (DeckStrategyRuntimeData)stage.RuntimeData;
            //    runtimeData.OwnedCharacterList = characterList;
            //    testLog();
            //}

            PopulateScrollView();
        }

        //private void testLog()
        //{
        //    DeckStrategyStage stage = Manager.StageManager.Instance.GetCurrentStage() as DeckStrategyStage;
        //    if (stage != null)
        //    {
        //        DeckStrategyRuntimeData runtimeData = (DeckStrategyRuntimeData)stage.RuntimeData;
        //        foreach(OwnedCharacterInfo info in runtimeData.OwnedCharacterList)
        //        {
        //            Debug.Log(info.characterID + ", " + info.characterModelID + ", " + info.characterLevel);
        //        }
        //    }
        //}

        public void PopulateScrollView(CharacterFilterState filterState = null)
        {
            foreach (Transform child in contentParent)
            {
                Destroy(child.gameObject);
            }

            List<OwnedCharacterInfo> characterList = dataCenter.GetOwnedCharacterList();
            if (characterList == null) return;

            if(filterState == null)
            { 
                foreach (OwnedCharacterInfo character in characterList)
                {
                    var characterData = dataCenter.FindCharacterData(character.characterID);
                    AddCharacterIcon(character, characterData.type);
                }
            }
            else
            {
                foreach (OwnedCharacterInfo character in characterList)
                {
                    var characterData = dataCenter.FindCharacterData(character.characterID);
                    if (!filterState.checkedAttributes.Contains(characterData.type) &&
                        !filterState.checkedRanges.Contains(characterData.rangeType))
                    {
                        continue;
                    }
                    AddCharacterIcon(character, characterData.type);
                }
            }
        }

        private void AddCharacterIcon(OwnedCharacterInfo characterInfo, EAttributeType type)
        {
            var itemUI = Instantiate(iconPrefab, contentParent.transform);
            var icon = itemUI.GetComponent<CharacterIcon>(); // 프리팹에 붙은 UI 스크립트

            var modelData = dataCenter.FindCharacterModel(characterInfo.characterModelID);

            icon.SetIconData(characterInfo, type, modelData.material.color, characterInfo.characterLevel, SelectedOwnedList[characterInfo.characterID]);
        }

        public void UpdateCheckedList(int index, bool isChecked)
        {
            SelectedOwnedList[index] = isChecked;
        }
    }
}