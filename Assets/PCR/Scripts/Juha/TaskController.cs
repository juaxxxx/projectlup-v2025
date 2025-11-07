using Unity.IO.LowLevel.Unsafe;
using Unity.Profiling;
using Unity.VisualScripting;
using UnityEditorInternal;
using UnityEngine;
using System;

public class TaskController : MonoBehaviour
{
    private ITaskState digWallState;
    private ITaskState buildingState;
    private ITaskState idleState;

    private DigWallPreview digWallPreview;
    private BuildPreview buildPreview;

    public BuildingType currSelectedBuildingType;
    public ITaskState CurrentState { get; set; }

    public TileMap tileMap;
    public Tile lastClickTile;

    public PCRUICenter uiCenter;

    
    
    [SerializeField]
    private GameObject wheatFarmPrefab;
    [SerializeField]
    private GameObject mushroomFarmPrefab;
    [SerializeField]
    private GameObject restaurantPrefab;

    private void Awake()
    {
        digWallPreview = GetComponent<DigWallPreview>();
        buildPreview = GetComponent<BuildPreview>();
        tileMap = GetComponent<TileMap>();

        idleState = new IdleState(this);
        digWallState = new DigWallState(this, digWallPreview);
        buildingState = new BuildingState(this, buildPreview);
    }

    private void Start()
    {
        buildPreview.Init(tileMap);


        SetupMainMenuUI();
        
        Trasition(idleState);

        currSelectedBuildingType = BuildingType.NONE;
    }

    private void Update()
    {
        CurrentState.InputHandle();
    }

    public void Trasition(ITaskState state)
    {
        if (CurrentState != null)
        {
            CurrentState.Close(); // 상태 전환 전, 이전 작업 초기화
        }
        CurrentState = state;
        CurrentState.Open();  // 상태 전환 후, 현재 작업 초기화
    }


    public void DigWallTask()
    {
        Trasition(digWallState);
    }

    public void BuildingTask()
    {
        Trasition(buildingState);
    }

    public void IdleTask()
    {
        Trasition(idleState);
    }

    public void SetIdleActiveTrue()
    {
        IdleState idle = idleState as IdleState;
        idle.SetIsActiveUI(true);
    }

    public void SetIdleActiveFalse()
    {
        IdleState idle = idleState as IdleState;
        idle.SetIsActiveUI(false);
    }

    public void SetCurrSelectedBuildingType(BuildingType buildingType)
    {
        currSelectedBuildingType = buildingType;
    }

    public void CreateBuilding()
    {
        if (buildPreview.canBuild == false)
        {
            Debug.Log("Can't build");
            return;
        }

        Vector3 pos = lastClickTile.gameObject.transform.position;

        switch (currSelectedBuildingType)
        {
            case BuildingType.WHEATFARM:
                Instantiate(wheatFarmPrefab, pos, Quaternion.identity);

                break;
            case BuildingType.MUSHROOMFARM:
                Instantiate(mushroomFarmPrefab, pos, Quaternion.identity);

                break;
            case BuildingType.RESTAURANT:
                Instantiate(restaurantPrefab, pos, Quaternion.identity);

                break;
        }
    }

    private void SetupMainMenuUI()
    {
        if (uiCenter == null)
        {
            Debug.Log("UICenter is null");
            return;
        }

        uiCenter.InitUI(this);

    }

    public void ReturnToIdleState()
    {
        IdleTask();
        uiCenter.ReturnToMainScreen();
    }

    public void UpdateLastClickTile(Tile tile)
    {
        lastClickTile = tile;
        Debug.Log("Current Tile Pos: (" + lastClickTile.tileInfo.pos.x + ", " + lastClickTile.tileInfo.pos.y + ")");
    }
}
