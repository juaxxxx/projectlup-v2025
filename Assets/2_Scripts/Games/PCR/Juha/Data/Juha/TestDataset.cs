using System.Collections.Generic;
using UnityEngine;

namespace LUP.PCR
{
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
        public int id;
        public BuildingType type;
        public Vector2Int pos;

        public BuildingDataInfo(int id, BuildingType type, Vector2Int pos)
        {
            this.id = id;
            this.type = type;
            this.pos = pos;
        }
    }


    public class TestDataset : MonoBehaviour
    {

        List<Vector2Int> notWalls = new List<Vector2Int>();

        public void TestNotWalls()
        {

            notWalls.Add(new Vector2Int(10, 0));
            notWalls.Add(new Vector2Int(8, 1));
            notWalls.Add(new Vector2Int(9, 1));
            notWalls.Add(new Vector2Int(10, 1));
            notWalls.Add(new Vector2Int(11, 1));
            notWalls.Add(new Vector2Int(12, 1));
            notWalls.Add(new Vector2Int(10, 2));
            notWalls.Add(new Vector2Int(8, 3));
            notWalls.Add(new Vector2Int(9, 3));
            notWalls.Add(new Vector2Int(10, 3));
            notWalls.Add(new Vector2Int(11, 3));
            notWalls.Add(new Vector2Int(12, 3));
            notWalls.Add(new Vector2Int(13, 3));
            notWalls.Add(new Vector2Int(13, 4));
            notWalls.Add(new Vector2Int(13, 5));
            notWalls.Add(new Vector2Int(13, 6));
            notWalls.Add(new Vector2Int(14, 6));
            notWalls.Add(new Vector2Int(12, 6));
            notWalls.Add(new Vector2Int(11, 6));
            notWalls.Add(new Vector2Int(10, 6));
            notWalls.Add(new Vector2Int(9, 6));
            notWalls.Add(new Vector2Int(8, 6));
            notWalls.Add(new Vector2Int(7, 6));

        }

        private bool FindNotWall(Vector2Int pos)
        {
            for (int i = 0; i < notWalls.Count; i++)
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
            for (int i = 0; i < GridSize.x; i++)
            {
                for (int j = 0; j < GridSize.y; j++)
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

}