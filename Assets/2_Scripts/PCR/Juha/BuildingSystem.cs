using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

namespace LUP.PCR
{
    public class BuildingSystem : MonoBehaviour
    {
        private Dictionary<int, WallBase> currWalls;
        private Dictionary<int, BuildingBase> currBuildings;

        private BuildPreview buildPreview;

        // Load Wall, Building Data
        public void InitData(PCRDataCenter dataCenter)
        {
            List<WallDataInfo> wallInfoes = dataCenter.wallDatas;

            // 임시 id 할당
            int wallId = 1;

            for (int i = 0; i < wallInfoes.Count; i++)
            {

            }
        }




        public void CreateBuilding(BuildingType type, Tile pivotTile)
        {
            if (buildPreview.canBuild == false)
            {
                Debug.Log("Can't build");
                return;
            }

            Vector3 pos = pivotTile.gameObject.transform.position;

            switch (type)
            {
                case BuildingType.WHEATFARM:
                    //Instantiate(wheatFarmPrefab, pos, Quaternion.identity);

                    break;
                case BuildingType.MUSHROOMFARM:
                    //Instantiate(mushroomFarmPrefab, pos, Quaternion.identity);

                    break;
                case BuildingType.RESTAURANT:
                    // Instantiate(restaurantPrefab, pos, Quaternion.identity);

                    break;
            }
        }
    }
}
