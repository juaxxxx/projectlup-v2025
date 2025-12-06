using LUP.DSG.Utils.Enums;
using System;
using System.Text;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace LUP.DSG
{
    public class CharacterIcon : MonoBehaviour
    {
        [SerializeField]
        private Image portrait;
        [SerializeField]
        private Image attributeIcon;
        [SerializeField]
        private TextMeshProUGUI level;

        public SelectedButton selectedButton;

        public OwnedCharacterInfo characterInfo;

        public Action<OwnedCharacterInfo, SelectedButton> OnSelected;
        public Action<int, SelectedButton> OnDeselected;

        public int selectedSlot = -1;

        private void OnEnable()
        {
            IconBootstrapper.OnAllIconsGenerated += RefreshIcon;
        }

        private void OnDisable()
        {
            IconBootstrapper.OnAllIconsGenerated -= RefreshIcon;
        }
        public void Init()
        {
            FormationSystem formationSystem = FindAnyObjectByType<FormationSystem>();
            OnSelected = formationSystem.PlaceCharacter;
            OnDeselected = formationSystem.ReleaseCharacter;

            selectedButton.Init();
            selectedButton.button.onClick.AddListener(OnButtonClicked);
        }

        public void SetIconData(OwnedCharacterInfo info, EAttributeType type,
                        Color portraitColor, int characterLevel, bool isChecked)
        {
            characterInfo = info;

            level.text = "Lv." + characterLevel;

            int characterId = info.characterID;

            if (CharacterIconCache.TryGet(characterId, out var sprite))
            {
                portrait.sprite = sprite;
                portrait.color = Color.white;
            }
            else
            {
                // 아직 안 만들어졌으면 일단 색만 입힘
                portrait.sprite = null;
                portrait.color = portraitColor;
            }

            // 속성 색 처리, isChecked 처리 그대로…
        }

        public void OnButtonClicked()
        {
            if (selectedButton.isSelected)
            {
                OnDeselected?.Invoke(characterInfo.characterID, selectedButton);
            }
            else
            {
                OnSelected?.Invoke(characterInfo, selectedButton);
            }

            CharactersList charactersList = gameObject.GetComponentInParent<CharactersList>();
            if (charactersList != null)
            {
                charactersList.UpdateCheckedList(characterInfo.characterID, selectedButton.isSelected);
            }
        }
        private void RefreshIcon()
        {
            if (characterInfo == null)
                return;

            int characterId = characterInfo.characterID;

            if (CharacterIconCache.TryGet(characterId, out var sprite))
            {
                Debug.Log($"[CharacterIcon] Refresh 성공: {characterId}");

                portrait.sprite = sprite;
                portrait.color = Color.white;
                portrait.preserveAspect = true;
            }
            else
            {
                Debug.LogWarning($"[CharacterIcon] Refresh 실패(아직 없음): {characterId}");
            }
        }
    }
}