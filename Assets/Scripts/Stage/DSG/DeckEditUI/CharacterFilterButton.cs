using LUP.DSG;
using System;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

namespace LUP.DSG
{
    public class CharacterFilterButton<T> : FilterButtonBase where T : Enum
    {
        private CharacterFilterPanel filterPanel;
        private T enumValue;

        public override void Init(CharacterFilterPanel panel, Enum enumVal)
        {
            filterPanel = panel;
            enumValue = (T)enumVal;
            filterText.text = enumValue.ToString();

            filterButton.onClick.AddListener(() =>
            {
                isSelected = !isSelected;
                filterPanel.UpdateFilter(enumValue);
            });
        }
    }
}