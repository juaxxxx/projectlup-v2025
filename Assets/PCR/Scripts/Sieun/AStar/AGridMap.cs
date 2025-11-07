using UnityEngine;
using System.Collections.Generic;



namespace PCR
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



        private void Awake()

        {

            gridStartPoint = transform.position;

            CreateGrid();

        }



        void CreateGrid()

        {

            grid = new ANode[gridXCount, gridYCount];



            for (int x = 0; x < gridXCount; x++)

            {

                for (int y = 0; y < gridYCount; y++)

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

                Gizmos.color = node.isWalkable ? Color.green : Color.red;

                Gizmos.DrawCube(node.worldPos, Vector3.one * (tileSize * 0.9f));

            }



            if (pathToDraw != null)

            {

                Gizmos.color = Color.yellow;

                for (int i = 0; i < pathToDraw.Count - 1; i++)

                    Gizmos.DrawLine(pathToDraw[i].worldPos, pathToDraw[i + 1].worldPos);

            }

        }

    }

}

