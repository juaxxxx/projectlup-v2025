using System;
using UnityEngine;

namespace LUP.PCR
{
    public interface IConstructionDecisionView
    {
        event Action OnClickAccept;
        event Action OnClickReject;

        void Show();
        void Hide();
    }
}

