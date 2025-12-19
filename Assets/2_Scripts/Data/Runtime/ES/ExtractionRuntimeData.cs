using System;
using UnityEngine;

[Serializable]
public class ExtractionRuntimeData : BaseRuntimeData
{
    [SerializeField]
    private int playerID = 0;
    [SerializeField]
    private int weaponID = 0;

    public int PlayerID
    {
        get => playerID;
        set => SetValue(ref playerID, value);
    }

    public int WeaponID
    {
        get => weaponID;
        set => SetValue(ref weaponID, value);
    }

    public override void ResetData()
    {
        playerID = 0;
        weaponID = 0;
    }
}

