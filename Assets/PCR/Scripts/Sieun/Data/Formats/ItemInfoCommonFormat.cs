using System;
using UnityEngine;

[Serializable]
public struct ItemInfoCommonFormat
{
    public string itemName; 
    [Multiline] public string description;
    public Sprite icon;
}
