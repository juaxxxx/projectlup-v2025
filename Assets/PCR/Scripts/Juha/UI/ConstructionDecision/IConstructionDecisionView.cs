using System;
using UnityEngine;

public interface IConstructionDecisionView
{
    event Action OnClickAccept;
    event Action OnClickReject;

    void Show();
    void Hide();
}
