using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class DigWallState : MonoBehaviour, ITaskState
{
    private TaskController taskController;

    private DigWallPreview digWallPreview;

    private void Start()
    {
        digWallPreview = GetComponent<DigWallPreview>();
    }

    public void InputHandle(TaskController controller)
    {
        // @TODO
        // 땅파기 버튼 눌렀을 때
        // 가능 불가능 땅 표시
        // 벽 클릭 시 땅파기 명령.
        // 다른 곳 클릭 시 IdleState로 전환.

        if (!taskController)
        {
            taskController = controller;
        }

        // 1. 가능 불가능 표시.

        // 2. 입력
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            // 클릭시 UI가 포함이면 리턴한다.
            if (EventSystem.current.IsPointerOverGameObject()) return;

            Debug.Log("벽 발굴 시도");

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
                var structure = wallHit.collider.GetComponent<WallBase>();
                if (structure)
                {
                    // 추후에 부술 수 있는 벽인지 판단할 수 있어야 한다.
                    structure.InteractForTouch();
                    // 벽 표시 갱신
                    UpdateDigTile();

                    controller.ReturnToIdleState();
                }
                else
                {
                    controller.ReturnToIdleState();
                }
            }
            else
            {
                Debug.Log("아냐 취소해");
                controller.ReturnToIdleState();
            }
        }
    }

    public void Open(TaskController controller)
    {
        // 땅 표시 활성화
        digWallPreview.Show();
    }

    public void Close(TaskController controller)
    {
        // 땅 표시 비활성화
        digWallPreview.Hide();
    }

    public void UpdateDigTile()
    {
        var pos = Mouse.current.position.ReadValue();
        var ray = Camera.main.ScreenPointToRay(pos);
        RaycastHit tileHit;

        if (Physics.Raycast(ray, out tileHit, 1000f, LayerMask.GetMask("Tile")))
        {
            Tile tile = tileHit.collider.GetComponent<Tile>();
            if (tile)
            {
                if (tile.tileInfo.tileType == TileType.WALL)
                {
                    digWallPreview.RemoveCanDigTile(tile);
                    tile.HideCanDigWallMark();
                    digWallPreview.AddCanNotDigTile(tile);
                    tile.SetTileInfo(new TileInfo(TileType.PATH, BuildingType.NONE, WallType.NONE, tile.tileInfo.pos, tile.tileInfo.id));
                }
            }
        }
    }
}
