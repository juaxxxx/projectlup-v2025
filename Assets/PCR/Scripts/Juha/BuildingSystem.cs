using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class BuildingSystem : MonoBehaviour
{
    [SerializeField]
    private GameObject wheatFarmPrefab;
    [SerializeField]
    private GameObject mushroomFarmPrefab;
    [SerializeField]
    private GameObject restaurantPrefab;

    private Dictionary<int, WallBase> walls;
    private Dictionary<int, BuildingBase> buildings;

    private BuildPreview buildPreview;

    // Load Wall, Building Data


    public void CreateBuilding(BuildingType type, Tile pivotTile)
    {
        if (buildPreview.canBuild == false)
        {
            Debug.Log("Can't build");
            return;
        }

        Vector3 pos = pivotTile.gameObject.transform.position;

        switch (type)
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
}
