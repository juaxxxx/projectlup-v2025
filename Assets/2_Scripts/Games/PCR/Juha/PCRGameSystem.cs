using UnityEngine;

namespace LUP.PCR
{

    public class PCRGameSystem : MonoBehaviour
    {

        [SerializeField]
        private BuildingGenerator buildingGenerator;
        [SerializeField]
        private BuildingSystem buildingSystem;
        [SerializeField]
        private TileMap tileMap;
        [SerializeField]
        private TaskController taskController;
        [SerializeField]
        private PCRUICenter uiCenter;
        [SerializeField]
        private DigWallPreview digWallPreview;
        [SerializeField]
        private BuildPreview buildPreview;
        [SerializeField]
        private PCRResourceCenter resourceCenter;

        private void Awake()
        {
            buildingGenerator = GetComponentInChildren<BuildingGenerator>();
            buildingSystem = GetComponentInChildren<BuildingSystem>();
            tileMap = GetComponentInChildren<TileMap>();
            digWallPreview = GetComponentInChildren<DigWallPreview>();
            buildPreview = GetComponentInChildren<BuildPreview>();
            taskController = GetComponentInChildren<TaskController>();
            uiCenter = GetComponentInChildren<PCRUICenter>();
            resourceCenter = GetComponentInChildren<PCRResourceCenter>();
        }

        public void InitPCRGameSystem()
        {
            // TileMap Init
            tileMap.InitTileMap();

            resourceCenter.InitInventory();

            // BuildingSystem Init
            buildingSystem.InitBuildingSystem(buildingGenerator, buildPreview, digWallPreview, tileMap, resourceCenter);

            // DigWallPreview Init
            digWallPreview.UpdateAllDigWallPreview(tileMap);

            // BuildPreview Init
            buildPreview.Init(tileMap);

            // TaskController Init
            taskController.InitTaskController(uiCenter, digWallPreview, buildPreview, tileMap, buildingSystem);

            // uiCenter Init
            uiCenter.InitUI(taskController);
        }
    }

}
