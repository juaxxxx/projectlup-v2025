using Unity.Android.Gradle;
using Unity.Android.Types;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class BuildingState : MonoBehaviour, ITaskState
{
    private TaskController taskController;

    private BuildingPlacementRules buildingPlacementRules;

    private BuildingType currBuildingType;

    private void Start()
    {
        buildingPlacementRules = gameObject.AddComponent<BuildingPlacementRules>();
        currBuildingType = BuildingType.NONE;
    }

    public void InputHandle(TaskController controller)
    {
        // @TODO
        // 건설 버튼 눌렀을 때
        // 건물 선택창 활성화
        // 1. 건물 선택 버튼 클릭 시 건설 위치 요구.
        // 1-1. 화면 클릭 시 해당 위치가 설치 가능한지 표시
        // 1-2. 실행 or 취소 버튼
        // 다른 곳 클릭 시 IdleState로 전환.

        if (!taskController)
        {
            taskController = controller;
            buildingPlacementRules.Init(taskController.tileMap);
        }

        // 2. 입력
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            // 클릭시 UI가 포함이면 리턴한다.
            if (EventSystem.current.IsPointerOverGameObject()) return;

            var pos = Mouse.current.position.ReadValue();
            var ray = Camera.main.ScreenPointToRay(pos);

            RaycastHit tileHit;

            if (Physics.Raycast(ray, out tileHit, 1000f, LayerMask.GetMask("Tile")))
            {
                var tile = tileHit.collider.GetComponent<Tile>();
                if (tile)
                {
                    controller.UpdateLastClickTile(tile);

                    if (currBuildingType == BuildingType.NONE)
                    {
                        Debug.Log("Current BuildingType is NONE.");
                    }

                    switch (buildingPlacementRules.CanPlace(currBuildingType, tile))
                    {
                        case PlacementResultType.SUCCESS:
                            Build(currBuildingType);
                            controller.CreateBuilding(currBuildingType, tile.gameObject.transform.position);
                            break;
                        case PlacementResultType.NOTENOUGHSPACE:
                            Debug.Log("Lack Space");
                            break;
                        case PlacementResultType.LACKOFRESOURCE:
                            Debug.Log("Lack Resource");
                            break;
                    }
                    Debug.Log("여기서 idle로 가는데");
                    controller.ReturnToIdleState();
                    return;
                }
                else
                {
                    controller.ReturnToIdleState();
                }
            }
            else
            {
                controller.ReturnToIdleState();
            }
        }
    }

    public void Open(TaskController controller)
    {
        Debug.Log("Building State Open");

    }

    public void Close(TaskController controller)
    {
        Debug.Log("Building State Close");
    }

    public void Build(BuildingType type)
    {
        switch(type)
        {
            case BuildingType.WHEATFARM:
                break;
        }
    }

    public void SetCurrBuildingType(BuildingType type)
    {
        currBuildingType = type;
    }
}
