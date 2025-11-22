using System;
using UnityEngine;

[Serializable]
public class VersionsData : BaseRuntimeData
{
    [SerializeField] private string _name = "Versions";
    [SerializeField] private string _assetbundlehash = "";

    public string assetbundlehash
    {
        get => _assetbundlehash;
        set => SetValue(ref _assetbundlehash, value);
    }

    public string name
    {
        get => _name;
        set => SetValue(ref _name, value);
    }

    public override void ResetData()
    {
        _assetbundlehash = "";
        _name = "Versions";
    }
}
