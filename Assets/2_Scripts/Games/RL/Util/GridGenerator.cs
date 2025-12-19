#if UNITY_EDITOR
using System.Globalization;
//using UnityEditor.Experimental.GraphView;
#endif
using UnityEngine;
namespace LUP.RL
{
    public class GridGenerator : MonoBehaviour
    {
        public static GridGenerator Instance;
        [SerializeField]
        public Transform plane;
    
        [SerializeField]
        public float CellY = 0;
        private TileData[,] grid;

        
        public int gridX = 10;
        public int gridZ = 15;
        private void Awake()
        {
            Instance = this;
        }
        void Start()
        {
        
            float mapWidth = plane.localScale.x;
            float mapHeight = plane.localScale.z;


            //  왼쪽아래 구석으로 맵 자동조정  
            Vector3 origin = plane.position - new Vector3(mapWidth / 2f, 0, mapHeight / 2f);
            float cellWidth = mapWidth / gridX;
            float cellHeight = mapHeight / gridZ;

            grid = new TileData[gridX, gridZ];

            for (int z = 0; z < gridZ; ++z)
            {
                for (int x = 0; x < gridX; ++x)
                {
                    Vector3 center = origin + new Vector3
                    (
                       cellWidth * (x + 0.5f),
                       CellY,
                       cellHeight * (z + 0.5f)
                    );

             
                    TileData data = new TileData
                    {
                        x = x,
                        z = z,
                        worldPos = center,
                        isWalkable = true
                    };
                    grid[x, z] = data;
                }

            }

        }
        public TileData GetTile(int x, int z)
        {
            if (x < 0 || z < 0 || x >= gridX || z >= gridZ)
                return null;
            return grid[x, z];
        }
        
    }
}