using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

namespace LUP.PCR
{
    public class BuildingSystem : MonoBehaviour
    {
        private BuildingGenerator buildingGenerator;

        private Dictionary<int, WallBase> currWalls;
        private Dictionary<int, BuildingBase> currBuildings;

        private BuildPreview buildPreview;

        private int buildingId = 1; // 테스트 Id.

        // Load Wall, Building Data
        public void InitBuildingSystem(PCRDataCenter dataCenter, BuildingGenerator buildingGenerator, BuildPreview buildPreview)
        {
            this.buildingGenerator = buildingGenerator;
            this.buildPreview = buildPreview;

            List<WallDataInfo> wallInfoes = dataCenter.wallDatas;
            currWalls = new Dictionary<int, WallBase>();
            currBuildings = new Dictionary<int, BuildingBase>();

            // 임시 id 할당
            int wallId = 1;

            // wall Init
            for (int i = 0; i < wallInfoes.Count; i++)
            {
                WallType wallType = wallInfoes[i].type;
                Vector2Int wallPos = wallInfoes[i].pos;

                GameObject wallObject = buildingGenerator.CreateInitWall(wallType, wallPos);
                if (!wallObject)
                {
                    Debug.Log("wallObject is null");
                    continue;
                }

                WallBase wall = wallObject.GetComponent<WallBase>();

                if (!wall)
                {
                    Debug.Log("WallBase is Null");
                    continue;
                }

                if (!currWalls.ContainsKey(wallId))
                {
                    currWalls.Add(wallId, wall);
                }
                else
                {
                    Debug.Log("wallId already exists");
                }

                wallId++;
            }

            Debug.Log("BuildingSystem Init");
        }

        public void CreateBuilding(BuildingType type, Tile pivotTile)
        {
            if (buildPreview.canBuild == false)
            {
                Debug.Log("Can't build");
                return;
            }

            BuildingBase building = buildingGenerator.CreateBuilding(type, pivotTile);

            if (building != null)
            {
                // @TODO: Id 설정 만들어야 한다.
                if (!currBuildings.ContainsKey(buildingId))
                {
                    currBuildings.Add(buildingId, building);
                }
                buildingId++;
            }
        }
    }
}
