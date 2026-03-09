using LUP.DSG.Utils.Enums;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace LUP.DSG
{
    public class CharacterFilterPanel : MonoBehaviour
    {
        [SerializeField]
        private GameObject filterPanel;
        [SerializeField]
        private GameObject filterButtonPrefab;
        [SerializeField]
        private Transform attributesFilterArea;
        [SerializeField]
        private Transform rangesFilterArea;

        // 람다 함수 캐싱
        private readonly List<Action<CharacterFilterState>> statePopulators = new();
        private readonly List<Action> dataResets = new();

        private IFilterable[] cachedFilters;

        public event Action<CharacterFilterState> OnConfirmFilter;

        void Start()
        {
            CreateButtons<AttributeFilterButton, EAttributeType>(attributesFilterArea);
            CreateButtons<RangeFilterButton, ERangeType>(rangesFilterArea);

            cachedFilters = GetComponentsInChildren<IFilterable>(includeInactive: true);
        }

        private void CreateButtons<TButton, TEnum>(Transform area) where TButton : BaseFilterButton<TEnum> where TEnum : unmanaged, Enum
        {
            if (filterButtonPrefab == null || area == null) return;

            TEnum[] enumValues = (TEnum[])Enum.GetValues(typeof(TEnum));
            bool[] localState = new bool[enumValues.Length];

            for (int i = 0; i < enumValues.Length; i++)
            {
                TEnum enumVal = enumValues[i];
                int index = i; // 클로저가 캡처할 수 있도록 로컬 인덱스 복사

                GameObject buttonObj = Instantiate(filterButtonPrefab, area);
                TButton button = buttonObj.AddComponent<TButton>();
                FilterButtonUI uiView = buttonObj.GetComponent<FilterButtonUI>();
                button.Register(uiView, enumVal);

                button.OnFilterToggled += (val) => localState[index] = !localState[index];
            }

            // confirm
            statePopulators.Add((filterState) =>
            {
                for (int i = 0; i < enumValues.Length; i++)
                {
                    if (localState[i]) filterState.AddFilter(enumValues[i]);
                }
            });

            // reset
            dataResets.Add(() =>
            {
                Array.Clear(localState, 0, localState.Length);
            });
        }

        public void ConfirmFilter()
        {
            CharacterFilterState filter = new CharacterFilterState();

            foreach (var populator in statePopulators)
                populator.Invoke(filter);

            OnConfirmFilter?.Invoke(filter.ContainsCheckedFilters() ? filter : null);

            if (filterPanel != null) filterPanel.SetActive(false);
        }

        public void ResetAllFilter()
        {
            if (cachedFilters != null)
            {
                for (int i = 0; i < cachedFilters.Length; i++)
                    cachedFilters[i].ResetCheckState();
            }

            for (int i = 0; i < dataResets.Count; i++)
                dataResets[i].Invoke();
        }
    }
}