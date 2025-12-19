using UnityEngine;

namespace LUP.PCR
{
    [System.Serializable]
    public class ConstructionInfo
    {
        public int buildingId;
        public float elapsedTime;

        public ConstructionInfo(int buildingId, float elapsedTime)
        {
            this.buildingId = buildingId;
            this.elapsedTime = elapsedTime;
        }
    }
}
