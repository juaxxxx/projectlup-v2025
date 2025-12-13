using UnityEngine;

namespace LUP.PCR
{
    [System.Serializable]
    public class ConstructionInfo
    {
        public int buildingId;
        public float elapsedTime;

        public ConstructionInfo(float elapsedTime)
        {
            this.elapsedTime = elapsedTime;
        }
    }
}
