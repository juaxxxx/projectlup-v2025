using UnityEngine;

namespace LUP.PCR
{
    [System.Serializable]
    public class BuildingInfo
    {
        public int buildingId;
        public int level;
        public Vector2Int gridPos;
        public int buildingType;

        public BuildingInfo(int buildingId, int level, Vector2Int gridPos, int buildingType)
        {
            this.buildingId = buildingId;
            this.level = level;
            this.gridPos = gridPos;
            this.buildingType = buildingType;
        }
    }
}
