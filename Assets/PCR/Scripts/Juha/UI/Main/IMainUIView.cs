using System;
using UnityEngine;

public interface IMainUIView
{
    event Action OnClickDig;
    event Action OnClickConstruct;

    void Show();
    void Hide();
}
