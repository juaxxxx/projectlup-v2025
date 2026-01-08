using System.Collections.Generic;
using UnityEngine;

namespace LUP.PCR
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
            int distX = Mathf.Abs(a.indexX - b.indexX);
            int distY = Mathf.Abs(a.indexY - b.indexY);

            int cost = 10 * (distX + distY);

            // 엘리베이터끼리는 비용 대폭 할인 (선호도 증가)
            if (a.isElevator && b.isElevator)
            {
                return (int)(cost * 0.5f);
            }

            // 사다리끼리는 비용 소폭 증가 (엘리베이터가 있다면 그걸 타도록 유도)
            if (a.isLadder && b.isLadder)
            {
                return (int)(cost * 1.2f);
            }

            return cost;
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
                    if (openList[i].FCost < current.FCost || (openList[i].FCost == current.FCost
                        && openList[i].hCost < current.hCost)) { current = openList[i]; }
                }

                openList.Remove(current);
                closedSet.Add(current);

                if (current == targetNode) { return RetracePath(startNode, targetNode); }

                foreach (ANode neighbor in GetNeighbors(current))
                {
                    if (!neighbor.isWalkable || closedSet.Contains(neighbor)) { continue; }

                    int newCost = current.gCost + GetDistance(current, neighbor);
                    if (newCost < neighbor.gCost || !openList.Contains(neighbor))
                    {
                        neighbor.gCost = newCost;
                        neighbor.hCost = GetDistance(neighbor, targetNode);
                        neighbor.parentNode = current;

                        if (!openList.Contains(neighbor)) { openList.Add(neighbor); }
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
                    if (x == 0 && y == 0) { continue; }
                    if (Mathf.Abs(x) + Mathf.Abs(y) != 1) { continue; } // 대각선 제외

                    int checkX = node.indexX + x;
                    int checkY = node.indexY + y;

                    if (gridMap.IsIdxValid(checkX, checkY))
                    {
                        ANode neighbor = gridMap.grid[checkX, checkY];
                        if (!neighbor.isWalkable)
                        {
                            continue; // 벽이면 통과 }

                            if (y == 0)
                            {
                                neighbors.Add(neighbor);
                            }
                            else
                            {
                                bool canClimb = (node.isLadder || neighbor.isLadder);
                                bool canElevate = (node.isElevator || neighbor.isElevator);

                                if (canClimb || canElevate)
                                {
                                    neighbors.Add(neighbor);
                                }
                            }
                        }
                    }

                }
            }
            return neighbors;
        }
    }
}