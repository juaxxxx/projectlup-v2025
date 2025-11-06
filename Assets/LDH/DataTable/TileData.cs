using UnityEngine;
namespace RL
{ 
[System.Serializable]

    public class TileData
    {
        public int x;
        public int z;
        public Vector3 worldPos;
        public GameObject cellObj;
        public bool isWalkable = true;
    }
}