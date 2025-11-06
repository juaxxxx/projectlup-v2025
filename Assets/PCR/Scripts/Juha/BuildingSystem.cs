using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class BuildingSystem : MonoBehaviour
{
    private TaskController taskController;

    private TileMap tileMap;

    private DigWallPreview digWallPreview;

    private void Awake()
    {
        taskController = GetComponent<TaskController>();
        tileMap = GetComponent<TileMap>();
        digWallPreview = GetComponent<DigWallPreview>();
    }

    void Start()
    {
        InitializeTileMap();
    }

    private void InitializeTileMap()
    {
        tileMap.InitializeTileMap();
        tileMap.UpdateAllDigWallPreview(digWallPreview);
        tileMap.GenerateObject();
    }

    Vector3 ScreenToWorldOnZ(Vector2 screenPos, float z)
    {
        var cam = Camera.main;
        // 카메라에서 z평면까지의 거리
        float distance = (z - cam.transform.position.z) / cam.transform.forward.z;
        return cam.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, distance));
    }
}
