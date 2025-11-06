using System;
using UnityEngine;

public interface ISelectConstructUIView
{
    event Action OnClickSelectedBuilding;
    event Action OnClickBack;
    public event Action<BuildingType> OnBuildingTypeChanged;

    void Show();
    void Hide();
}
