using PCR;
using System.Collections.Generic;
using UnityEngine;
namespace PCR

{

    public class APathfinding

    {

        AGridMap gridMap;



        public APathfinding(AGridMap map)

        {

            gridMap = map;

        }



        int GetDistance(ANode a, ANode b)

        {

            int dx = Mathf.Abs(a.indexX - b.indexX);

            int dy = Mathf.Abs(a.indexY - b.indexY);

            return 10 * (dx + dy); // 맨해튼 거리

        }



        public List<ANode> FindPath(ANode startNode, ANode targetNode)

        {

            List<ANode> openList = new List<ANode>();

            HashSet<ANode> closedSet = new HashSet<ANode>();

            openList.Add(startNode);



            while (openList.Count > 0)

            {

                ANode current = openList[0];

                for (int i = 1; i < openList.Count; i++)

                {

                    if (openList[i].FCost < current.FCost ||

                    (openList[i].FCost == current.FCost && openList[i].hCost < current.hCost))

                        current = openList[i];

                }



                openList.Remove(current);

                closedSet.Add(current);



                if (current == targetNode)

                    return RetracePath(startNode, targetNode);



                foreach (ANode neighbor in GetNeighbors(current))

                {

                    if (!neighbor.isWalkable || closedSet.Contains(neighbor))

                        continue;



                    int newCost = current.gCost + GetDistance(current, neighbor);

                    if (newCost < neighbor.gCost || !openList.Contains(neighbor))

                    {

                        neighbor.gCost = newCost;

                        neighbor.hCost = GetDistance(neighbor, targetNode);

                        neighbor.parentNode = current;



                        if (!openList.Contains(neighbor))

                            openList.Add(neighbor);

                    }

                }

            }



            return null;

        }



        List<ANode> RetracePath(ANode start, ANode end)

        {

            List<ANode> path = new List<ANode>();

            ANode current = end;

            while (current != start)

            {

                path.Add(current);

                current = current.parentNode;

            }

            path.Reverse();

            return path;

        }



        List<ANode> GetNeighbors(ANode node)

        {

            List<ANode> neighbors = new List<ANode>();

            for (int x = -1; x <= 1; x++)

            {

                for (int y = -1; y <= 1; y++)

                {

                    if (Mathf.Abs(x) + Mathf.Abs(y) != 1)

                        continue; // 대각선 제외



                    int checkX = node.indexX + x;

                    int checkY = node.indexY + y;



                    if (checkX >= 0 && checkX < gridMap.grid.GetLength(0)

                    && checkY >= 0 && checkY < gridMap.grid.GetLength(1))

                        neighbors.Add(gridMap.grid[checkX, checkY]);

                }

            }

            return neighbors;

        }

    }

}