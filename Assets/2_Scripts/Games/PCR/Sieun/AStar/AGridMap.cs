using UnityEngine;
using System.Collections.Generic;

namespace LUP.PCR
{
    public class AGridMap : MonoBehaviour
    {
        [Header("Map Settings")]
        [SerializeField] float tileSize = 5;
        [SerializeField] LayerMask unwalkableMask; // wall

        public ANode[,] grid;
        public TileInfo[,] sourceInfoTiles;

        Vector3 gridStartPoint;
        [HideInInspector] public List<ANode> pathToDraw;
        [HideInInspector] public ANode debugStartNode;
        [HideInInspector] public ANode debugTargetNode;
       
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
                    Vector3 worldPosition = GridToWorldPosition(new Vector2Int(x, y));
                    
                    //bool walkable = sourceInfoTiles[x, y].tileType != TileType.WALL;
                    bool walkable = !Physics.CheckSphere(worldPosition, tileSize * 0.4f, unwalkableMask);

                    grid[x, y] = new ANode(walkable, worldPosition, x, y);
                }
            }

        }

        // Vector2Int(데이터좌표) -> Vector3(월드 좌표) 변환 
        public Vector3 GridToWorldPosition(Vector2Int gridPos)
        {
            float xPos = gridPos.x * tileSize + tileSize / 2f;
            float yPos = -(gridPos.y * tileSize + tileSize / 2f);

            return gridStartPoint + new Vector3(xPos, yPos, -2.5f);
        }

        // Vector3(월드 좌표) -> ANode (내부에 x, y 인덱스 포함)
        public ANode GetNodeFromWorldPosition(Vector3 worldPosition)
        {
            if (grid == null) { return null; }

            int x = Mathf.FloorToInt((worldPosition.x - gridStartPoint.x) / tileSize);
            int y = Mathf.FloorToInt(-(worldPosition.y - gridStartPoint.y) / tileSize);

            x = Mathf.Clamp(x, 0, grid.GetLength(0) - 1);
            y = Mathf.Clamp(y, 0, grid.GetLength(1) - 1);

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
        //public Vector3 GetNodeWorldPosition(ANode node)
        //{
        //    return node.worldPos;
        //}

        public Vector3 GetNodeFootPosition(ANode node)
        {
            Vector3 centerPos = node.worldPos;

            return new Vector3(centerPos.x, centerPos.y - (tileSize / 2f), centerPos.z);
        }


        private void OnDrawGizmos()
        {
            if (grid == null) return;


            foreach (var node in grid)
            {
                Gizmos.color = node.isWalkable ? new Color(0, 1, 0, 0.3f) : new Color(1, 0, 0, 0.3f);
                Gizmos.DrawCube(node.worldPos, Vector3.one * (tileSize * 0.9f));
            }

            if (pathToDraw != null)
            {
                Gizmos.color = Color.yellow;
                for (int i = 0; i < pathToDraw.Count - 1; i++)
                {
                    Gizmos.DrawLine(pathToDraw[i].worldPos, pathToDraw[i + 1].worldPos);
                }
            }

            if (debugStartNode != null)
            {
                Gizmos.color = new Color(0, 0, 1, 0.3f);
                Gizmos.DrawSphere(debugStartNode.worldPos, tileSize * 0.5f);
            }

            if (debugTargetNode != null)
            {
                Gizmos.color = new Color(1, 0, 0, 0.3f);
                Gizmos.DrawSphere(debugTargetNode.worldPos, tileSize * 0.5f);
                Gizmos.DrawLine(debugTargetNode.worldPos, debugTargetNode.worldPos + Vector3.up * 10f);
            }
        }

        [ContextMenu("Print Walkable Nodes")]
        public void PrintWalkableNodes()
        {
            if (grid == null)
            {
                Debug.LogWarning("그리드가 아직 생성되지 않았습니다. 게임 실행(Play) 후에 눌러보세요.");
                return;
            }

            string result = "--- 갈 수 있는 좌표 목록 (Walkable Nodes) ---\n";
            int count = 0;

            for (int x = 0; x < grid.GetLength(0); x++)
            {
                for (int y = 0; y < grid.GetLength(1); y++)
                {
                    if (grid[x, y].isWalkable)
                    {
                        result += $"[{x}, {y}] ";
                        count++;

                        // 보기 좋게 10개씩 줄바꿈
                        if (count % 10 == 0) result += "\n";
                    }
                }
            }
            Debug.Log($"{result}\n------------------------------------------\n총 {count}개의 이동 가능 타일 발견");
        }


    }

}
