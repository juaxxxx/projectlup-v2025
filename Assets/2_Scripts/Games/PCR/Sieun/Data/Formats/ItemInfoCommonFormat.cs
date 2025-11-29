using System;
using UnityEngine;

namespace LUP.PCR
{
    [Serializable]
    public struct ItemInfoCommonFormat
    {
        public string itemName;
        [Multiline] public string description;
        public Sprite icon;
    }
}

