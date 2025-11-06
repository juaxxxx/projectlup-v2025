using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;

public class IdleState : MonoBehaviour, ITaskState
{
    private TaskController taskController;
    private bool bIsActiveUI;

    private void Start()
    {
        bIsActiveUI = false;
    }

    public void InputHandle(TaskController controller)
    {
        // @TODO
        // 평소 상태는 지금은 명령할게 생각 안남.

        if (!taskController)
        {
            taskController = controller;
        }

        // 입력
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            // 클릭시 UI가 포함이면 리턴한다.
            if (EventSystem.current.IsPointerOverGameObject()) return;

            if (bIsActiveUI)
            {
                controller.ReturnToIdleState();
                SetIsActiveUI(false);
            }
            else
            {
                var pos = Mouse.current.position.ReadValue();
                var ray = Camera.main.ScreenPointToRay(pos);
                RaycastHit wallHit;
                RaycastHit tileHit;

                // 클릭 타일 갱신
                if (Physics.Raycast(ray, out tileHit, 1000f, LayerMask.GetMask("Tile")))
                {
                    Tile tile = tileHit.collider.GetComponent<Tile>();

                    if (tile)
                    {
                        controller.UpdateLastClickTile(tile);
                    }
                }

                if (Physics.Raycast(ray, out wallHit, 1000f, LayerMask.GetMask("Building")))
                {
                    BuildingBase building = wallHit.collider.GetComponent<BuildingBase>();

                    if (building)
                    {
                        Debug.Log("Find Building");
                        OpenBuildingTaskUI(building);

                        building.InteractForTouch();
                    }
                }
            }
        }
    }

    public void SetIsActiveUI(bool isActive)
    {
        bIsActiveUI = isActive;
    }

    public void Open(TaskController controller)
    {
        SetIsActiveUI(false);
        Debug.Log("Idle State Open");
    }

    public void Close(TaskController controller)
    {
        Debug.Log("Idle State Close");
    }

    private void OpenBuildingTaskUI(BuildingBase building)
    {
        ProductableBuilding productableBuilding = building as ProductableBuilding;
        if (productableBuilding)
        {
            taskController.uiCenter.OpenProductableTask(productableBuilding);
        }

    }
}
