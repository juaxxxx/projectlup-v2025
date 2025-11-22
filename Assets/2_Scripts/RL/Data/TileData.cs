using UnityEngine;
namespace LUP.RL
{ 
[System.Serializable]

    public class TileData
    {
        public float x;
        public float y;
        public float z;
        public Vector3 worldPos;
        public GameObject cellObj;
        public bool isWalkable = true;
    }
}