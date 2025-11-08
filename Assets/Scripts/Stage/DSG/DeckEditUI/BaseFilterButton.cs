using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LUP.DSG
{
    public class BaseFilterButton<T> : MonoBehaviour where T : Enum
    {
        private CharacterFilterPanel filterPanel;

        [SerializeField]
        protected T enumValue;

        [SerializeField]
        protected Button filterButton;
        [SerializeField]
        protected TextMeshProUGUI filterText;

        private Image checkedImage;
        private bool isSelected = false;

        void Awake()
        {
            if (!typeof(T).IsEnum)
                throw new ArgumentException($"{typeof(T)} is not an Enum type");
        }

        public void Register(CharacterFilterPanel panel, GameObject buttonObject, T enumVal)
        {
            filterPanel = panel;
            enumValue = enumVal;
            if (filterButton = buttonObject.GetComponentInChildren<Button>())
            {
                filterButton.onClick.AddListener(SelectFilterProperty);
                checkedImage = filterButton.gameObject.transform.GetChild(0).GetComponentInChildren<Image>();
            }
            if (filterText = buttonObject.GetComponentInChildren<TextMeshProUGUI>())
            {
                filterText.text = enumVal.ToString();
            }
        }

        void SelectFilterProperty()
        {
            isSelected = !isSelected;
            checkedImage.enabled = isSelected;

            filterPanel.UpdateFilter(enumValue);
        }
    }
}