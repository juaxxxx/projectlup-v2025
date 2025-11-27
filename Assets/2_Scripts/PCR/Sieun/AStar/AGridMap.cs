using UnityEngine;
using System.Collections.Generic;

namespace LUP.PCR
{
    public class AGridMap : MonoBehaviour
    {
        [Header("Map Settings")]
        [SerializeField] float tileSize = 5f;
        [SerializeField] int gridXCount = 10;
        [SerializeField] int gridYCount = 10;
        [SerializeField] LayerMask unwalkableMask;

        public ANode[,] grid;
        Vector3 gridStartPoint;
        [HideInInspector] public List<ANode> pathToDraw;

        /// <summary>
        /// 데이터 연결 전 워커 행동트리 테스트용 임시데이터

        PCRDataCenter pcrDataCenter;
        // from PCRDataCenter CreateGrid()
        public TileInfo[,] tileInfoes;
        int tileMapWidth = 28;
        int tileMapHeight = 15;
        
        //


        /// </summary>

        private void Awake()
        {
            gridStartPoint = transform.position;
            CreateGrid();
        }

        void CreateGrid()
        {
            tileInfoes = pcrDataCenter.tileInfoes;

            grid = new ANode[tileMapWidth, tileMapHeight];

            for (int x = 0; x < tileMapWidth; x++)
            {
                for (int y = 0; y < tileMapHeight; y++)
                {
                    Vector3 worldPosition =
                        gridStartPoint + new Vector3(x * tileSize + tileSize / 2f, y * tileSize + tileSize / 2f, 0);




                    bool walkable = !Physics.CheckSphere(worldPosition, tileSize * 0.4f, unwalkableMask);

                    
                    
                    
                    grid[x, y] = new ANode(walkable, worldPosition, x, y);
                }
            }

        }

        public ANode GetNodeFromWorldPosition(Vector3 worldPosition)
        {
            int x = Mathf.Clamp(Mathf.FloorToInt((worldPosition.x - gridStartPoint.x) / tileSize), 0, gridXCount - 1);
            int y = Mathf.Clamp(Mathf.FloorToInt((worldPosition.y - gridStartPoint.y) / tileSize), 0, gridYCount - 1);

            return grid[x, y];

        }
        public Vector3 GetNodeWorldPosition(ANode node)
        {
            return node.worldPos;
        }
        private void OnDrawGizmos()
        {
            if (grid == null) return;

            foreach (var node in grid)
            {
                //Gizmos.color = node.isWalkable ? Color.green : Color.red;
                //Gizmos.DrawCube(node.worldPos, Vector3.one * (tileSize * 0.9f));



            }

            if (pathToDraw != null)
            {
                Gizmos.color = Color.yellow;
                for (int i = 0; i < pathToDraw.Count - 1; i++)
                {
                    //Gizmos.DrawLine(pathToDraw[i].worldPos, pathToDraw[i + 1].worldPos);





                }
            }
        }

    }

}

/*
         [Header("Map Settings")]
    [SerializeField] float tileSize = 5f;
    [SerializeField] LayerMask unwalkableMask; // wall

    public ANode[,] grid;
    public TileInfo[,] sourceInfoTiles;

    Vector3 gridStartPoint;
    [HideInInspector] public List<ANode> pathToDraw;

    //[SerializeField] int gridXCount = 10;
    //[SerializeField] int gridYCount = 10;
    /// <summary>
    /// 데이터 연결 전 워커 행동트리 테스트용 임시데이터
    // from PCRDataCenter CreateGrid()
    //int tileMapWidth = 28;
    //int tileMapHeight = 15; // dataCenter.tileMapWidth, tileMapHeight
    /// </summary>

    /// <summary>
    /// PCRGameSystem 혹은 초기화 매니저에서 호출해야 함
    /// </summary>
    /// <param name="tileData">PCRDataCenter의 TileInfo 배열</param>
    public void InitMap(TileInfo[,] tileData)
    {
        gridStartPoint = transform.position;
        sourceInfoTiles = tileData;
        CreateGridFromData();
    }

    void CreateGridFromData()
    {
        if (sourceInfoTiles == null) { return; }

        int width = sourceInfoTiles.GetLength(0);
        int height = sourceInfoTiles.GetLength(1);

        grid = new ANode[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3 worldPosition =
                    GridToWorldPosition(new Vector2Int(x, y));

                bool walkable = !Physics.CheckSphere(worldPosition, tileSize * 0.4f, unwalkableMask);

                grid[x, y] = new ANode(walkable, worldPosition, x, y);
            }
        }

    }

    public Vector3 GridToWorldPosition(Vector2Int gridPos)
    {
        return gridStartPoint + new Vector3(gridPos.x * tileSize + tileSize / 2f, gridPos.y * tileSize + tileSize / 2f, 0);
    }



    //public ANode GetNodeFromWorldPosition(Vector3 worldPosition)
    //{
    //    int x = Mathf.Clamp(Mathf.FloorToInt((worldPosition.x - gridStartPoint.x) / tileSize), 0, gridXCount - 1);
    //    int y = Mathf.Clamp(Mathf.FloorToInt((worldPosition.y - gridStartPoint.y) / tileSize), 0, gridYCount - 1);

    //    return grid[x, y];

    //}
    public Vector3 GetNodeWorldPosition(ANode node)
    {
        return node.worldPos;
    }

    private void OnDrawGizmos()
    {
        if (grid == null) return;

        foreach (var node in grid)
        {
            //Gizmos.color = node.isWalkable ? Color.green : Color.red;
            //Gizmos.DrawCube(node.worldPos, Vector3.one * (tileSize * 0.9f));



        }

        if (pathToDraw != null)
        {
            Gizmos.color = Color.yellow;
            for (int i = 0; i < pathToDraw.Count - 1; i++)
            {
                //Gizmos.DrawLine(pathToDraw[i].worldPos, pathToDraw[i + 1].worldPos);





            }
        }
    }

}
 */