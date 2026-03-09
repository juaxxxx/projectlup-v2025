using System;
using UnityEngine;

namespace LUP.DSG
{
    public class BaseFilterButton<T> : MonoBehaviour, IFilterable where T : unmanaged, Enum
    {
        public T enumValue { get; private set; }
        private FilterButtonUI uiView;
        private bool isSelected = false;
        public event Action<T> OnFilterToggled;

        public void Register(FilterButtonUI view, T enumVal)
        {
            uiView = view;
            enumValue = enumVal;

            if (!uiView) return;

            if (uiView.filterText)
                uiView.filterText.text = enumVal.ToString();
            if (uiView.filterButton)
                uiView.filterButton.onClick.AddListener(OnFilterButtonClicked);

            ResetCheckState();
        }

        public void ResetCheckState()
        {
            isSelected = false;
            if(uiView.checkedImage) uiView.checkedImage.enabled = false;
        }

        private void OnFilterButtonClicked()
        {
            isSelected = !isSelected;
            if (uiView.checkedImage) uiView.checkedImage.enabled = isSelected;

            OnFilterToggled?.Invoke(enumValue);
        }

        private void OnDestroy()
        {
            if (uiView && uiView.filterButton)
                uiView.filterButton.onClick.RemoveListener(OnFilterButtonClicked);
        }
    }
}