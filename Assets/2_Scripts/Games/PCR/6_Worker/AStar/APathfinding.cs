using System.Collections.Generic;
using UnityEngine;

namespace LUP.PCR
{
    public enum LadderState
    {
        None,
        Bottom,
        Middle,
        Top,
        Single
    }
    public class APathfinding
    {
        AGridMap gridMap;

        public APathfinding(AGridMap map)
        {
            gridMap = map;
        }

        int GetDistance(ANode a, ANode b)
        {
            return 10 * (Mathf.Abs(a.indexX - b.indexX) + Mathf.Abs(a.indexY - b.indexY));
        }

        void ResetNodes()
        {
            foreach (ANode node in gridMap.grid)
            {
                node.gCost = int.MaxValue;
                node.hCost = 0;
                node.parentNode = null;
            }
        }

        public List<ANode> FindPath(ANode startNode, ANode targetNode)
        {
            ResetNodes();
            startNode.gCost = 0;

            List<ANode> openList = new();
            HashSet<ANode> closedSet = new();
            openList.Add(startNode);

            while (openList.Count > 0)
            {
                ANode current = openList[0];

                for (int i = 1; i < openList.Count; i++)
                {
                    if (openList[i].FCost < current.FCost ||
                        (openList[i].FCost == current.FCost && openList[i].hCost < current.hCost))
                    {
                        current = openList[i];
                    }
                }

                openList.Remove(current);
                closedSet.Add(current);

                if (current == targetNode)
                {
                    return RetracePath(startNode, targetNode);
                }

                foreach (ANode neighbor in GetNeighbors(current))
                {
                    if (closedSet.Contains(neighbor))
                        continue;

                    int newCost = current.gCost + GetDistance(current, neighbor);
                    if (newCost < neighbor.gCost || !openList.Contains(neighbor))
                    {
                        neighbor.gCost = newCost;
                        neighbor.hCost = GetDistance(neighbor, targetNode);
                        neighbor.parentNode = current;

                        if (!openList.Contains(neighbor))
                        {
                            openList.Add(neighbor);
                        }
                    }
                }
            }

            return null;
        }
        List<ANode> RetracePath(ANode start, ANode end)
        {
            List<ANode> path = new();
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
            List<ANode> neighbors = new();

            int[,] dirs = { { 1, 0 }, { -1, 0 }, { 0, 1 }, { 0, -1 } };

            LadderState state = GetLadderState(node);

            for (int i = 0; i < 4; i++)
            {
                int nx = node.indexX + dirs[i, 0];
                int ny = node.indexY + dirs[i, 1];

                if (!gridMap.IsIdxValid(nx, ny))
                {
                    continue;
                }

                ANode neighbor = gridMap.grid[nx, ny];
                if (!neighbor.isWalkable)
                {
                    continue;
                }

                int dy = dirs[i, 1];
                if (dy == 0 && CanMoveSide(state))
                { 
                    neighbors.Add(neighbor);
                }
                else if (dy == 1 && CanMoveUp(node, neighbor, state))
                {
                    neighbors.Add(neighbor);
                }
                else if (dy == -1 && CanMoveDown(node, neighbor, state))
                {
                    neighbors.Add(neighbor);
                }
            }

            return neighbors;
        }
        bool HasLadderAt(int x, int y)
        {
            if (!gridMap.IsIdxValid(x, y)) return false;
            return gridMap.grid[x, y].isLadder;
        }

        LadderState GetLadderState(ANode node)
        {
            if (!node.isLadder)
            {
                return LadderState.None;
            }

            bool up = HasLadderAt(node.indexX, node.indexY + 1);
            bool down = HasLadderAt(node.indexX, node.indexY - 1);

            if (!up && !down)
            {
                return LadderState.Single;
            }
            
            if (!up && down)
            {
                return LadderState.Top;
            }
            if (up && !down)
            {
                return LadderState.Bottom;
            }
                return LadderState.Middle;
        }

        bool CanMoveSide(LadderState state)
        {
            return state != LadderState.Middle;
        }

        bool CanMoveUp(ANode current, ANode neighbor, LadderState state)
        {
            if (neighbor.isLadder)
            {
                return true;
            }

            // 사다리 꼭대기에서 위 바닥으로 나가기
            return state == LadderState.Top || state == LadderState.Single;
        }

        bool CanMoveDown(ANode current, ANode neighbor, LadderState state)
        {
            if (neighbor.isLadder)
            {
                return true;
            }

            // 사다리 아래 끝에서 바닥으로
            return state == LadderState.Bottom || state == LadderState.Single;
        }
    }
}
