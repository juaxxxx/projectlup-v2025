using UnityEngine;
using System.Collections.Generic;

namespace LUP.PCR
{
    public class AGridMap : MonoBehaviour
    {
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
        /// </summary>

        /// PCRGameSystem 혹은 초기화 매니저에서 호출해야 함
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

                    bool walkable = sourceInfoTiles[x, y].tileType != TileType.WALL;
                    //!Physics.CheckSphere(worldPosition, tileSize * 0.4f, unwalkableMask);

                    grid[x, y] = new ANode(walkable, worldPosition, x, y);
                }
            }

        }

        // Vector2Int(데이터좌표) -> Vector3(월드 좌표) 변환 
        public Vector3 GridToWorldPosition(Vector2Int gridPos)
        {
            return gridStartPoint + new Vector3(gridPos.x * tileSize + tileSize / 2f, gridPos.y * tileSize + tileSize / 2f, 0);
        }

        // Vector3(월드 좌표) -> ANode (내부에 x, y 인덱스 포함)
        public ANode GetNodeFromWorldPosition(Vector3 worldPosition)
        {
            if (grid == null) { return null; }

            int x = Mathf.Clamp(Mathf.FloorToInt((worldPosition.x - gridStartPoint.x) / tileSize), 0, grid.GetLength(0) - 1);
            int y = Mathf.Clamp(Mathf.FloorToInt((worldPosition.y - gridStartPoint.y) / tileSize), 0, grid.GetLength(1) - 1);

            return grid[x, y];
        }

        public ANode GetNodeFromGridPos(Vector2Int pos)
        {
            if(grid == null) { return null; }
            
            if(pos.x >= 0 && pos.y >= 0 && pos.x < grid.GetLength(0) && pos.y < grid.GetLength(1))
            {

                return grid[pos.x, pos.y];
            }

            return null;
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
