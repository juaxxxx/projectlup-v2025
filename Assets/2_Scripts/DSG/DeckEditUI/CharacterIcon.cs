using System;
using System.Text;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using LUP.DSG.Utils.Enums;

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

        private void Awake()
        {
            FormationSystem formationSystem = FindAnyObjectByType<FormationSystem>();
            OnSelected = formationSystem.PlaceCharacter;
            OnDeselected = formationSystem.ReleaseCharacter;
        }

        private void Start()
        {
            selectedButton.button.onClick.AddListener(OnButtonClicked);
        }

        public void SetIconData(OwnedCharacterInfo info, EAttributeType type, Color portraitColor, int characterLevel, bool isChecked)
        {
            level.text = "Lv." + characterLevel.ToString();

            portrait.color = portraitColor;

            if (type == EAttributeType.ROCK)
            {
                attributeIcon.color = Color.red;
            }
            else if (type == EAttributeType.SCISSORS)
            {
                attributeIcon.color = Color.green;
            }
            else
            {
                attributeIcon.color = Color.blue;
            }

            characterInfo = info;
            if(isChecked)
            {
                selectedButton.ButtonClicked();
            }
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
    }
}