using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class DigWallState : ITaskState
{
    private TaskController taskController;

    private DigWallPreview digWallPreview;

    public DigWallState(TaskController controller, DigWallPreview digWallPreview)
    {
        taskController = controller;
        this.digWallPreview = digWallPreview;
    }

    public void InputHandle()
    {
        if (!taskController)
        {
            Debug.Log("taskController is Null");

            return;
        }

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
                    taskController.UpdateLastClickTile(tile);
                }
            }

            if (Physics.Raycast(ray, out wallHit, 1000f, LayerMask.GetMask("Wall")))
            {
                var structure = wallHit.collider.GetComponent<WallBase>();
                if (structure)
                {
                    // 추후에 부술 수 있는 벽인지 판단할 수 있어야 한다.
                    structure.InteractForTouch();
                    // 벽 표시 갱신
                    UpdateDigTile();

                    taskController.ReturnToIdleState();
                }
                else
                {
                    taskController.ReturnToIdleState();
                }
            }
            else
            {
                Debug.Log("아냐 취소해");
                taskController.ReturnToIdleState();
            }
        }
    }

    public void Open()
    {
        // 땅 표시 활성화
        Debug.Log("DigWall State Open");
        digWallPreview.Show();
    }

    public void Close()
    {
        // 땅 표시 비활성화
        Debug.Log("DigWall State Close");
        digWallPreview.Hide();
    }

    public void UpdateDigTile()
    {
        Debug.Log(taskController.lastClickTile);

        Tile tile = taskController.lastClickTile;
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

        //var pos = Mouse.current.position.ReadValue();
        //var ray = Camera.main.ScreenPointToRay(pos);
        //RaycastHit tileHit;

        //if (Physics.Raycast(ray, out tileHit, 1000f, LayerMask.GetMask("Default")))
        //{
        //    Tile tile = tileHit.collider.GetComponent<Tile>();
        //    if (tile)
        //    {
        //        Debug.Log("Update Dig Tile");
        //        if (tile.tileInfo.tileType == TileType.WALL)
        //        {
        //            digWallPreview.RemoveCanDigTile(tile);
        //            tile.HideCanDigWallMark();
        //            digWallPreview.AddCanNotDigTile(tile);
        //            tile.SetTileInfo(new TileInfo(TileType.PATH, BuildingType.NONE, WallType.NONE, tile.tileInfo.pos, tile.tileInfo.id));
        //        }
        //    }
        //}

    }
}
