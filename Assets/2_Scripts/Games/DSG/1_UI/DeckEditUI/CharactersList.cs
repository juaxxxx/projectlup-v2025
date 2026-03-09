using LUP.DSG.Utils.Enums;
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

        private int activeCount = 0;

        public void ResetSelectedStatus()
        {
            selectedOwnedMap.Clear();

            for (int i = 0; i < iconPool.Count; i++)
            {
                if (iconPool[i] == null || iconPool[i].selectedButton == null) continue;
                iconPool[i].selectedButton.SetSelected(false);
            }
        }

        public void UpdateCharacterIcon(CharacterInfo info, AttributeTypeImage typeIcon)
        {
            if (info == null || typeIcon.typeIcon == null) return;

            CharacterIcon icon = GetOrCreateIcon();
            if (icon == null || icon.selectedButton == null) return;

            icon.transform.SetParent(contentParent, false);
            icon.gameObject.SetActive(true);
            icon.SetIconData(info.characterID, info.characterLevel, typeIcon);

            bool isSelected = selectedOwnedMap.TryGetValue(info.characterID, out bool value) && value;
            icon.selectedButton.SetSelected(isSelected);
        }

        public void UpdateCheckedList(int characterID, bool isChecked)
        {
            if (isChecked)
                selectedOwnedMap[characterID] = true;
            else
                selectedOwnedMap.Remove(characterID);
        }

        public void ReleaseAllIcons()
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
                Destroy(newIcon);
                return null;
            }

            icon.Init();
            iconPool.Add(icon);
            activeCount++;
            return icon;
        }

        private void BindIcon(CharacterInfo characterInfo, AttributeTypeImage typeIcon)
        {
            if (characterInfo == null || typeIcon.typeIcon == null) return;

            CharacterIcon icon = GetOrCreateIcon();
            if (icon == null) return;

            icon.transform.SetParent(contentParent, false);
            icon.gameObject.SetActive(true);
            icon.SetIconData(characterInfo.characterID, characterInfo.characterLevel, typeIcon);

            if (icon.selectedButton == null) return;

            bool isSelected = selectedOwnedMap.TryGetValue(characterInfo.characterID, out bool value) && value;
            icon.selectedButton.SetSelected(isSelected);
        }
    }
}