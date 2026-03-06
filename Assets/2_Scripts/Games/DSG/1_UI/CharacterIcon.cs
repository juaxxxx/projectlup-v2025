using LUP.DSG.Utils.Enums;
using System;
using TMPro;
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

        public CharacterSelectButton selectedButton;

        private int characterId;

        public Action<int, CharacterSelectButton> OnSelected;
        public Action<int, CharacterSelectButton> OnDeselected;

        [SerializeField]
        private float iconWidth = 400f;
        [SerializeField]
        private float iconHeight = 800f;

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
            FormationView formationView = FindAnyObjectByType<FormationView>();
            if(formationView != null)
            {
                OnSelected = formationView.RequestPlaceCharacter;
                OnDeselected = formationView.RequestReleaseCharacter;
            }

            selectedButton.Init();
            selectedButton.button.onClick.AddListener(OnButtonClicked);

            SetIconRectSize(iconWidth, iconHeight);
        }

        public void SetIconData(int id, int characterLevel, AttributeTypeImage typeIcon)
        {
            characterId = id;
            level.text = "Lv." + characterLevel;

            if (CharacterIconCache.TryGetByCharacterId(characterId, out var sprite))
            {
                portrait.sprite = sprite;
                portrait.color = Color.white;
            }
            else
            {
                // 아직 안 만들어졌으면 일단 색만 입힘
                portrait.sprite = null;
            }

            attributeIcon.sprite = typeIcon.typeIcon;
            attributeIcon.color = typeIcon.typeColor;
        }

        public void OnButtonClicked()
        {
            if (selectedButton.isSelected)
            {
                OnDeselected?.Invoke(characterId, selectedButton);
            }
            else
            {
                OnSelected?.Invoke(characterId, selectedButton);
            }

            CharactersList charactersList = gameObject.GetComponentInParent<CharactersList>();
            if (charactersList != null)
            {
                charactersList.UpdateCheckedList(characterId, selectedButton.isSelected);
            }
        }
        private void RefreshIcon()
        {
            if (CharacterIconCache.TryGetByCharacterId(characterId, out var sprite))
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

        public void SetIconRectSize(float width, float height)
        {
            var rt = portrait.rectTransform;

            rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
            rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
        }

    }
}