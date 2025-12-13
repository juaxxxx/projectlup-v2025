using UnityEngine;

namespace LUP.PCR
{
    [System.Serializable]
    public class WallInfo
    {
        public int wallType;
        public Vector2Int gridPos;

        public WallInfo(int wallType, Vector2Int gridPos)
        {
            this.wallType = wallType;
            this.gridPos = gridPos;
        }
    }
}

