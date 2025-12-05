using UnityEngine;
using System;

namespace LUP.PCR
{
    public class TaskController : MonoBehaviour
    {
        private ITaskState digWallState;
        private ITaskState buildingState;
        private ITaskState idleState;

        public DigWallPreview digWallPreview;
        public BuildPreview buildPreview;

        public BuildingType currSelectedBuildingType;
        public ITaskState currentState { get; set; }

        public TileMap tileMap;
        public Tile lastClickTile;

        public BuildingSystem buildingSystem;
        public PCRUICenter uiCenter;

        private void Awake()
        {
            idleState = new IdleState(this);
            digWallState = new DigWallState(this);
            buildingState = new BuildingState(this);
        }

        private void Start()
        {
            currentState = null;
            lastClickTile = null;
            currSelectedBuildingType = BuildingType.NONE;
        }

        private void Update()
        {
            if (currentState != null)
            {
                currentState.InputHandle();
            }
        }

        public void InitTaskController(PCRUICenter uiCenter, DigWallPreview digWallPreview, BuildPreview buildPreview, 
            TileMap tileMap, BuildingSystem buildingSystem)
        {
            this.uiCenter = uiCenter;
            this.digWallPreview = digWallPreview;
            this.buildPreview = buildPreview;
            this.tileMap = tileMap;
            this.buildingSystem = buildingSystem;

            Trasition(idleState);

            Debug.Log("TaskController Init");
        }

        public void Trasition(ITaskState state)
        {
            if (currentState != null)
            {
                currentState.Close(); // 상태 전환 전, 이전 작업 초기화
            }
            currentState = state;
            currentState.Open();  // 상태 전환 후, 현재 작업 초기화
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

        // @TODO: 이거 BuildSystem으로 가는게 맞는듯
        public void CreateBuilding()
        {
            buildingSystem.CreateBuilding(currSelectedBuildingType, lastClickTile);
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

}
