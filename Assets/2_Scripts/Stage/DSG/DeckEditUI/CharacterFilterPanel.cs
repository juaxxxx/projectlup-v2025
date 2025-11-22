using LUP.DSG.Utils.Enums;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

namespace LUP.DSG
{
    public class CharacterFilterPanel : MonoBehaviour
    {
        [SerializeField]
        CharactersList charactersList;
        [SerializeField]
        private GameObject filterPanel;

        [SerializeField]
        private GameObject filterButtonPrefab;
        [SerializeField]
        private Transform attributesFilterArea;
        [SerializeField]
        private Transform RangesFilterArea;


        Dictionary<EAttributeType, bool> attributeFilter = new Dictionary<EAttributeType, bool>();
        Dictionary<ERangeType, bool> rangeTypeFilter = new Dictionary<ERangeType, bool>() ;

        void Start()
        {
            foreach (EAttributeType type in Enum.GetValues(typeof(EAttributeType)))
            {
                attributeFilter[type] = false;
            }
            foreach (ERangeType type in Enum.GetValues(typeof(ERangeType)))
            {
                rangeTypeFilter[type] = false;
            }

            CreateButtons<EAttributeType>();
            CreateButtons<ERangeType>();
        }

        private void CreateButtons<T>() where T : Enum
        {
            Transform CreatedArea;
            Type propertyType = typeof(T);
            if (propertyType == typeof(EAttributeType))
            {
                CreatedArea = attributesFilterArea;
                foreach (EAttributeType value in Enum.GetValues(propertyType))
                {
                    GameObject buttonObj = Instantiate(filterButtonPrefab, CreatedArea);
                    var button = buttonObj.AddComponent<AttributeFilterButton>();
                    button.Register(this, buttonObj, value);
                }
            }
            else
            {
                CreatedArea = RangesFilterArea;
                foreach (ERangeType value in Enum.GetValues(propertyType))
                {
                    GameObject buttonObj = Instantiate(filterButtonPrefab, CreatedArea);
                    var button = buttonObj.AddComponent<RangeFilterButton>();
                    button.Register(this, buttonObj, value);
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
            {
                if (pair.Value) filter.checkedAttributes.Add(pair.Key);
            }
            foreach (KeyValuePair<ERangeType, bool> pair in rangeTypeFilter)
            {
                if (pair.Value) filter.checkedRanges.Add(pair.Key);
            }

            charactersList.PopulateScrollView(filter.ContainsCheckedFilters() ? filter : null);
            filterPanel.SetActive(false);
        }
    }
}