using System.Collections.Generic;
using UnityEngine;

namespace LUP.DSG
{
    public class CharactersList : MonoBehaviour
    {
        [SerializeField]
        private GameObject iconPrefab;
        [SerializeField]
        private Transform contentParent;

        Dictionary<int, bool> selectedOwnedMap = new Dictionary<int, bool>();
        private readonly List<CharacterIcon> iconPool = new List<CharacterIcon>();

        private DeckStrategyStage cachedStage;
        private DeckStrategyRuntimeData cachedRuntimeData;

        private int activeCount = 0;

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
            cachedStage = stage;
            cachedRuntimeData = stage != null ? stage.RuntimeData as DeckStrategyRuntimeData : null;
            RebuildSelectedMap();
        }

        private void RebuildSelectedMap()
        {
            selectedOwnedMap.Clear();

            if (cachedRuntimeData == null || cachedRuntimeData.OwnedCharacterList == null)
                return;

            for (int i = 0; i < cachedRuntimeData.OwnedCharacterList.Count; i++)
            {
                OwnedCharacterInfo info = cachedRuntimeData.OwnedCharacterList[i];
                if (info == null) continue;

                selectedOwnedMap[info.characterID] = false;
            }
        }

        public void ResetSelectedStatus()
        {
            var keys = new List<int>(selectedOwnedMap.Keys);
            foreach (var key in keys)
            {
                selectedOwnedMap[key] = false;
            }

            for (int i = 0; i < iconPool.Count; i++)
            {
                if (iconPool[i] == null || iconPool[i].selectedButton == null) continue;
                iconPool[i].selectedButton.SetSelected(false);
            }
        }

        public void RePopulateThroughFilter(CharacterFilterState filterState = null)
        {
            ReleaseAllIcons();

            if (cachedStage == null || cachedRuntimeData == null) return;

            List<OwnedCharacterInfo> characterList = cachedRuntimeData.OwnedCharacterList;
            if (characterList == null) return;

            for (int i = 0; i < characterList.Count; i++)
            {
                OwnedCharacterInfo characterInfo = characterList[i];
                if (characterInfo == null) continue;
                CharacterData characterData = cachedStage.FindCharacterData(characterInfo.characterID, characterInfo.characterLevel);
                if (characterData == null) continue;
                if (!IsMatched(filterState, characterData)) continue;
                CharacterIcon icon = GetOrCreateIcon();
                if (icon == null) continue;

                BindIcon(icon, characterInfo, characterData);
            }
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

        private void ReleaseAllIcons()
        {
            for (int i = 0; i < iconPool.Count; i++)
            {
                if (iconPool[i] == null) continue;

                if (iconPool[i].selectedButton != null)
                    iconPool[i].selectedButton.SetSelected(false);

                iconPool[i].gameObject.SetActive(false);
            }

            activeCount = 0;
        }

        private bool IsMatched(CharacterFilterState filterState, CharacterData characterData)
        {
            if (filterState == null || characterData == null) return true;

            if (!filterState.ContainsCheckedFilters()) return true;

            bool attributeMatched = filterState.checkedAttributes.Contains(characterData.type);
            bool rangeMatched = filterState.checkedRanges.Contains(characterData.rangeType);

            return attributeMatched || rangeMatched;
        }

        private CharacterIcon GetOrCreateIcon()
        {
            CharacterIcon icon;
            if (activeCount < iconPool.Count)
            {
                icon = iconPool[activeCount];
                activeCount++;
                return icon;
            }

            if (iconPrefab == null || contentParent == null)
                return null;

            GameObject newIcon = Instantiate(iconPrefab, contentParent);
            icon = newIcon.GetComponent<CharacterIcon>();
            if (icon == null)
            {
                Debug.LogError("[CharactersList] iconPrefab에 CharacterIcon 컴포넌트가 없습니다.");
                Destroy(newIcon);
                return null;
            }

            icon.Init();
            iconPool.Add(icon);
            activeCount++;
            return icon;
        }

        private void BindIcon(CharacterIcon icon, OwnedCharacterInfo characterInfo, CharacterData characterData)
        {
            if (icon == null || characterInfo == null || characterData == null)
                return;

            icon.transform.SetParent(contentParent, false);
            icon.gameObject.SetActive(true);
            icon.SetIconData(characterInfo, characterData.type, characterInfo.characterLevel, false);
            icon.selectedSlot = -1;

            if (icon.selectedButton != null)
            {
                bool isSelected = selectedOwnedMap.TryGetValue(characterInfo.characterID, out bool value) && value;
                icon.selectedButton.SetSelected(isSelected);
            }
        }
    }
}