using UnityEngine;

namespace LUP.PCR
{
    [System.Serializable]
    public class ProductionInfo
    {
        public int buildingId;
        public float elapsedTime;
        public int currentCapacity;

        public ProductionInfo(int buildingId, float elapsedTime, int currentCapacity)
        {
            this.buildingId = buildingId;
            this.elapsedTime = elapsedTime;
            this.currentCapacity = currentCapacity;
        }
    }
}