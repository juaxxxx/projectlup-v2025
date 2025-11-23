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
        private PCRDataCenter dataCenter;
        [SerializeField]
        private PCRUICenter uiCenter;
        [SerializeField]
        private DigWallPreview digWallPreview;
        [SerializeField]
        private BuildPreview buildPreview;

        private void Awake()
        {
            dataCenter = GetComponentInChildren<PCRDataCenter>();
            buildingGenerator = GetComponentInChildren<BuildingGenerator>();
            buildingSystem = GetComponentInChildren<BuildingSystem>();
            tileMap = GetComponentInChildren<TileMap>();
            digWallPreview = GetComponentInChildren<DigWallPreview>();
            buildPreview = GetComponentInChildren<BuildPreview>();
            taskController = GetComponentInChildren<TaskController>();
            uiCenter = GetComponentInChildren<PCRUICenter>();
        }

        private void Start()
        {
            // PCRDataCenter Init
            dataCenter.InitData();

            // BuildingSystem Init
            buildingSystem.InitBuildingSystem(dataCenter, buildingGenerator, buildPreview);

            // TileMap Init
            tileMap.InitializeTileMap(dataCenter.tileInfoes);

            // DigWallPreview Init
            digWallPreview.UpdateAllDigWallPreview(tileMap);

            // BuildPreview Init
            buildPreview.Init(tileMap);

            // TaskController Init
            taskController.InitTaskController(uiCenter, digWallPreview, buildPreview);

            // uiCenter Init
            uiCenter.InitUI(taskController);
        }
    }

}
