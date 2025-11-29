using System;
using UnityEngine;

namespace LUP.PCR
{
    public interface IMainUIView
    {
        event Action OnClickDig;
        event Action OnClickConstruct;

        void Show();
        void Hide();
    }

}

