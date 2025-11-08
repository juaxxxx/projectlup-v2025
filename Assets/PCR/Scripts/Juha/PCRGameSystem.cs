using System.Net.NetworkInformation;
using UnityEngine;

public class PCRGameSystem : MonoBehaviour
{

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
        // 각 시스템 컴포넌트 생성
        dataCenter = GetComponentInChildren<PCRDataCenter>();
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
        // BuildingSystem Init
        // TileMap Init

        // DigWallPreview Init
        digWallPreview.UpdateAllDigWallPreview(tileMap);

        // BuildPreview Init
        buildPreview.Init(tileMap);

        // TaskController Init
        taskController.InitTaskController(uiCenter, digWallPreview, buildPreview);

        // uiCenter Init
        uiCenter.InitUI(taskController);
    }

    private void InitializeTileMap()
    {

    }

}
