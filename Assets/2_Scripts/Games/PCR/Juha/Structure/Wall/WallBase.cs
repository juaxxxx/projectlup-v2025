using UnityEngine;

namespace LUP.PCR
{
    public abstract class WallBase : StructureBase
    {
        WallInfo wallInfo;

        public bool CanDig(int digPower)
        {
            if (wallInfo != null)
            {
                return wallInfo.wallType <= digPower;
            }

            return true;
        }
    }

}