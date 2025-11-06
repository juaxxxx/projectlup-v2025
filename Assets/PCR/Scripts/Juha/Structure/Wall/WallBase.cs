using UnityEngine;

public abstract class WallBase : StructureBase
{
    WallInfo wallInfo;

    public bool CanDig(int digPower)
    {
        return wallInfo.Durability <= digPower;
    }
}
