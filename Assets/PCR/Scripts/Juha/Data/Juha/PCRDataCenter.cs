using UnityEngine;
using System.Collections.Generic;

public class PCRDataCenter : MonoBehaviour
{
    int gridWidth = 5;
    int gridHeight = 5;
    int tileMapWidth = 28;
    int tileMapHeight = 15;

    TestDataset testDataset;

    public List<WallDataInfo> wallDatas;
    public List<BuildingDataInfo> buildingDatas;

    Tile[,] tiles;

    Dictionary<int, WallBase> walls;
    Dictionary<int, BuildingBase> buildings;

    private void Awake()
    {
        testDataset = new TestDataset();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        testDataset.TestNotWalls();
        wallDatas = testDataset.LoadWallInfo();
    }

}
