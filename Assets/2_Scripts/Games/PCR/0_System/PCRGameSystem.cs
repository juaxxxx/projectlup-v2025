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
        private WorkerSystem workerSystem;
        [SerializeField]
        private TaskController taskController;
        [SerializeField]
        private PCRUICenter uiCenter;
        [SerializeField]
        private DigWallPreview digWallPreview;
        [SerializeField]
        private BuildPreview buildPreview;

        private PCRResourceCenter resourceCenter;


        private void Awake()
        {
            buildingGenerator = GetComponentInChildren<BuildingGenerator>();
            buildingSystem = GetComponentInChildren<BuildingSystem>();
            tileMap = GetComponentInChildren<TileMap>();
            workerSystem = GetComponentInChildren<WorkerSystem>();
            digWallPreview = GetComponentInChildren<DigWallPreview>();
            buildPreview = GetComponentInChildren<BuildPreview>();
            taskController = GetComponentInChildren<TaskController>();
            uiCenter = GetComponentInChildren<PCRUICenter>();
        }

        public void InitPCRGameSystem()
        {
            resourceCenter = new PCRResourceCenter();

            // TileMap Init
            tileMap.InitTileMap();

            // ResourceCenterInit
            resourceCenter.InitResource();

            // BuildingSystem Init
            buildingSystem.InitBuildingSystem(buildingGenerator, buildPreview, digWallPreview, tileMap, resourceCenter);

            // WorkerSystem Init
            workerSystem.InitWorkerSystem(buildingSystem, tileMap);

            // DigWallPreview Init
            digWallPreview.UpdateAllDigWallPreview(tileMap);

            // BuildPreview Init
            buildPreview.Init(tileMap);

            // TaskController Init
            taskController.InitTaskController(digWallPreview, buildPreview, tileMap, buildingSystem);

            // uiCenter Init
            uiCenter.InitUI(taskController, resourceCenter);
        }
    }

}
