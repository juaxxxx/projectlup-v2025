using System.Collections.Generic;
using UnityEngine;

public struct WallDataInfo
{
    public int id;
    public WallType type;
    public Vector2Int pos;

    public WallDataInfo(int id, WallType type, Vector2Int pos)
    {
        this.id = id;
        this.type = type;
        this.pos = pos;
    }
}

public struct BuildingDataInfo
{

}

public class TestDataset : MonoBehaviour
{

    List<Vector2Int> notWalls;

    public void TestNotWalls()
    {
        notWalls.Add(new Vector2Int(0, 0));
        notWalls.Add(new Vector2Int(1, 0));
        notWalls.Add(new Vector2Int(2, 0));
        notWalls.Add(new Vector2Int(3, 0));
        notWalls.Add(new Vector2Int(4, 0));
        notWalls.Add(new Vector2Int(6, 0));
        notWalls.Add(new Vector2Int(7, 0));
        notWalls.Add(new Vector2Int(10, 0));
        notWalls.Add(new Vector2Int(10, 0));
        notWalls.Add(new Vector2Int(5, 1));
        notWalls.Add(new Vector2Int(6, 1));
        notWalls.Add(new Vector2Int(7, 1));
        notWalls.Add(new Vector2Int(8, 1));
        notWalls.Add(new Vector2Int(4, 2));
        notWalls.Add(new Vector2Int(5, 2));
        notWalls.Add(new Vector2Int(6, 2));
        notWalls.Add(new Vector2Int(8, 2));
        notWalls.Add(new Vector2Int(0, 3));
        notWalls.Add(new Vector2Int(1, 3));
        notWalls.Add(new Vector2Int(2, 3));
        notWalls.Add(new Vector2Int(3, 3));
        notWalls.Add(new Vector2Int(4, 3));
        notWalls.Add(new Vector2Int(8, 3));
        notWalls.Add(new Vector2Int(9, 3));
        notWalls.Add(new Vector2Int(10, 3));
        notWalls.Add(new Vector2Int(11, 3));
        notWalls.Add(new Vector2Int(12, 3));
        notWalls.Add(new Vector2Int(0, 4));
        notWalls.Add(new Vector2Int(1, 4));
        notWalls.Add(new Vector2Int(2, 4));
        notWalls.Add(new Vector2Int(3, 4));
        notWalls.Add(new Vector2Int(4, 4));
        notWalls.Add(new Vector2Int(8, 4));
        notWalls.Add(new Vector2Int(9, 4));
        notWalls.Add(new Vector2Int(10, 4));
        notWalls.Add(new Vector2Int(11, 4));
        notWalls.Add(new Vector2Int(12, 4));
        notWalls.Add(new Vector2Int(4, 5));
        notWalls.Add(new Vector2Int(8, 5));
        notWalls.Add(new Vector2Int(9, 5));
        notWalls.Add(new Vector2Int(10, 5));
        notWalls.Add(new Vector2Int(11, 5));
        notWalls.Add(new Vector2Int(12, 5));
        notWalls.Add(new Vector2Int(13, 5));
        notWalls.Add(new Vector2Int(14, 5));

    }

    private bool FindNotWall(Vector2Int pos)
    {
        for (int i =0; i< notWalls.Count; i++)
        {
            if (notWalls[i].x == pos.x && notWalls[i].y == pos.y)
            {
                notWalls.RemoveAt(i);
                return true;
            }
        }

        return false;
    }

    public List<WallDataInfo> LoadWallInfo()
    {
        // 임시로 받았다고 치자
        List<WallDataInfo> wallInfoes = new List<WallDataInfo>();
        wallInfoes.Clear();

        int idCount = 0;
        for (int i = 0; i < 28; i++)
        {
            for (int j = 0; j < 15; j++)
            {
                if (FindNotWall(new Vector2Int(i, j)))
                {
                    continue;
                }

                idCount++;
                wallInfoes.Add(new WallDataInfo(idCount, WallType.DUST, new Vector2Int(i, j)));
            }
        }

        return wallInfoes;
    }

    public List<BuildingDataInfo> LoadBuildingInfo()
    {
        List<BuildingDataInfo> buildingInfoes = new List<BuildingDataInfo>();
        buildingInfoes.Clear();



        return buildingInfoes;
    }
}
