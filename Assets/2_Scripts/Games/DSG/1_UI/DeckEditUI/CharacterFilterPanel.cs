using LUP.DSG.Utils.Enums;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace LUP.DSG
{
    public class CharacterFilterPanel : MonoBehaviour
    {
        private readonly EAttributeType[] AttributeTypes = (EAttributeType[])Enum.GetValues(typeof(EAttributeType));
        private readonly ERangeType[] RangeTypes = (ERangeType[])Enum.GetValues(typeof(ERangeType));

        [SerializeField]
        private GameObject filterPanel;

        [SerializeField]
        private GameObject filterButtonPrefab;
        [SerializeField]
        private Transform attributesFilterArea;
        [SerializeField]
        private Transform RangesFilterArea;

        private readonly Dictionary<EAttributeType, bool> attributeFilter = new Dictionary<EAttributeType, bool>();
        private readonly Dictionary<ERangeType, bool> rangeTypeFilter = new Dictionary<ERangeType, bool>() ;

        public event Action<CharacterFilterState> OnConfirmFilter;

        void Start()
        {
            for (int i = 0; i < AttributeTypes.Length; i++)
                attributeFilter[AttributeTypes[i]] = false;

            for (int i = 0; i < RangeTypes.Length; i++)
                rangeTypeFilter[RangeTypes[i]] = false;

            CreateButtons<EAttributeType>();
            CreateButtons<ERangeType>();
        }

        private void CreateButtons<T>() where T : Enum
        {
            if (filterButtonPrefab == null || 
                RangesFilterArea == null || 
                attributesFilterArea == null) 
                return;

            Type propertyType = typeof(T);
            if (propertyType == typeof(EAttributeType))
            {
                for (int i = 0; i < AttributeTypes.Length; i++)
                {
                    GameObject buttonObj = Instantiate(filterButtonPrefab, attributesFilterArea);
                    AttributeFilterButton button = buttonObj.AddComponent<AttributeFilterButton>();
                    button.Register(this, buttonObj, AttributeTypes[i]);
                }
            }
            else
            {
                for (int i = 0; i < RangeTypes.Length; i++)
                {
                    GameObject buttonObj = Instantiate(filterButtonPrefab, RangesFilterArea);
                    RangeFilterButton button = buttonObj.AddComponent<RangeFilterButton>();
                    button.Register(this, buttonObj, RangeTypes[i]);
                }
            }
        }

        public void UpdateFilter<T>(T checkedFilter) where T : Enum
        {
            switch (checkedFilter)
            {
                case EAttributeType attr:
                    attributeFilter[attr] = !attributeFilter[attr];
                    break;
                case ERangeType range:
                    rangeTypeFilter[range] = !rangeTypeFilter[range];
                    break;
            }
        }

        public void ConfirmFilter()
        {
            CharacterFilterState filter = new CharacterFilterState();

            foreach (KeyValuePair<EAttributeType, bool> pair in attributeFilter)
                if (pair.Value) filter.checkedAttributes.Add(pair.Key);
            foreach (KeyValuePair<ERangeType, bool> pair in rangeTypeFilter)
                if (pair.Value) filter.checkedRanges.Add(pair.Key);

            OnConfirmFilter?.Invoke(filter.ContainsCheckedFilters() ? filter : null);

            if (filterPanel != null)
                filterPanel.SetActive(false);
        }

        public void ResetAllFilter()
        {
            IFilterable[] filters = GetComponentsInChildren<IFilterable>(includeInactive: true);
            foreach(IFilterable filter in filters)
                filter.ResetCheckState();

            for (int i = 0; i < AttributeTypes.Length; i++)
                attributeFilter[AttributeTypes[i]] = false;
            for (int i = 0; i < RangeTypes.Length; i++)
                rangeTypeFilter[RangeTypes[i]] = false;
        }
    }
}