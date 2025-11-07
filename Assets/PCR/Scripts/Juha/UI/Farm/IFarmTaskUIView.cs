using System;
using UnityEngine;

public interface IFarmTaskUIView
{
    event Action OnClickTask;
    event Action OnClickTurbo;
    event Action OnClickWorker;
    event Action OnClickUpgrade;
    event Action OnClickBack;
    event Action<FarmUIBtnType> OnChangeTask;

    void Show();
    void Hide();
    void UpdateUIStats(FarmUIData data);
}
