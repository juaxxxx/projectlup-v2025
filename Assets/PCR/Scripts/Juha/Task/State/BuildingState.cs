using Unity.Android.Gradle;
using Unity.Android.Types;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class BuildingState : ITaskState
{
    private TaskController taskController;
    private BuildPreview buildPreview;

    private BuildingType currBuildingType;

    public BuildingState(TaskController controller, BuildPreview buildPreview)
    {
        taskController = controller;
        this.buildPreview = buildPreview;
        currBuildingType = BuildingType.NONE;
    }

    private void Start()
    {
    }

    public void InputHandle()
    {
        if (!taskController)
        {
            return;
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
                    taskController.UpdateLastClickTile(tile);

                    if (currBuildingType == BuildingType.NONE)
                    {
                        Debug.Log("Current BuildingType is NONE.");
                    }

                    buildPreview.ChangePreview(currBuildingType);
                    buildPreview.UpdatePreview(currBuildingType, taskController.lastClickTile);

                    return;
                }
                else
                {
                    taskController.ReturnToIdleState();
                }
            }
            //else
            //{
            //    taskController.ReturnToIdleState();
            //}
        }
    }

    public void Open()
    {
        Debug.Log("Building State Open");

        if (currBuildingType == BuildingType.NONE)
        {
            Debug.Log("currBuildingType is NONE");
            return;
        }
        buildPreview.ChangePreview(currBuildingType);

        buildPreview.UpdatePreview(currBuildingType, taskController.lastClickTile);
    }

    public void Close()
    {
        Debug.Log("Building State Close");
        buildPreview.ResetPreview();
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
