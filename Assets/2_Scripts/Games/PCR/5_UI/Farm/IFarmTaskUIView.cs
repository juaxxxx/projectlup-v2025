using System;
using UnityEngine;

namespace LUP.PCR
{
    public interface IFarmTaskUIView
    {
        event Action<FarmUIBtnType> OnChangeTab;

        event Action OnClickBack;
        event Action OnClickProduction;
        event Action OnClickUpgrade;

        void Show();
        void Hide();
        void UpdateUIStats(FarmUIData data);
    }
}
