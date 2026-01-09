using System;
using UnityEngine;

namespace LUP.PCR
{
    public interface IInventoryUIView
    {
        event Action OnClickBack;

        void Show();
        void Hide();

        void UpdateResourceText(PCRResourceCenter resourceCenter);
    }
}

