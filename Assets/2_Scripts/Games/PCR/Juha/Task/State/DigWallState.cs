using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace LUP.PCR
{
    public class DigWallState : ITaskState
    {
        private TaskController taskController;

        public DigWallState(TaskController controller)
        {
            taskController = controller;
        }

        public void InputHandle()
        {
            if (!taskController)
            {
                Debug.Log("taskController is Null");

                return;
            }

            if (Mouse.current != null && Mouse.current.leftButton.wasReleasedThisFrame)
            {
                // 클릭시 UI가 포함이면 리턴한다.
                if (EventSystem.current.IsPointerOverGameObject()) return;

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
                    WallBase wall = wallHit.collider.GetComponent<WallBase>();
                    if (wall)
                    {
                        taskController.buildingSystem.RemoveWall(wall);
                        taskController.ReturnToIdleState();
                    }
                    else
                    {
                        taskController.ReturnToIdleState();
                    }
                }
                else
                {
                    taskController.ReturnToIdleState();
                }
            }
        }

        public void Open()
        {
            // 땅 표시 활성화
            Debug.Log("DigWall State Open");
            taskController.digWallPreview.Show();
        }

        public void Close()
        {
            // 땅 표시 비활성화
            Debug.Log("DigWall State Close");
            taskController.digWallPreview.Hide();
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