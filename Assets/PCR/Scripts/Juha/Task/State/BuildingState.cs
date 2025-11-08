using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class BuildingState : ITaskState
{
    private TaskController taskController;
    private BuildPreview buildPreview;

    public BuildingState(TaskController controller, BuildPreview buildPreview)
    {
        taskController = controller;
        this.buildPreview = buildPreview;
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

                    if (taskController.currSelectedBuildingType == BuildingType.NONE)
                    {
                        Debug.Log("Current BuildingType is NONE.");
                    }

                    buildPreview.ChangePreview(taskController.currSelectedBuildingType);
                    buildPreview.UpdatePreview(taskController.currSelectedBuildingType, taskController.lastClickTile);

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

        if (taskController.currSelectedBuildingType == BuildingType.NONE)
        {
            Debug.Log("currBuildingType is NONE");
            return;
        }
        buildPreview.ChangePreview(taskController.currSelectedBuildingType);

        buildPreview.UpdatePreview(taskController.currSelectedBuildingType, taskController.lastClickTile);
    }

    public void Close()
    {
        Debug.Log("Building State Close");
        buildPreview.ResetPreview();
    }

}
