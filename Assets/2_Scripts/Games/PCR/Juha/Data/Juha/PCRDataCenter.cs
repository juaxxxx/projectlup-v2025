using UnityEngine;
using System.Collections.Generic;

namespace LUP.PCR
{
    public class PCRDataCenter : MonoBehaviour
    {
        int gridWidth = 5;
        int gridHeight = 5;

        TestDataset testDataset;

        public List<WallDataInfo> wallDatas;
        public List<BuildingDataInfo> buildingDatas;
        public TileInfo[,] tileInfoes;

        public PCRConstructionStaticData constructionData;
        public PCRProductionStaticData productionData;
        //public PCRBuildingStaticData buildingData;

        private void Awake()
        {
            testDataset = new TestDataset();
        }

        void Start()
        {

        }

        public void InitData()
        {
            tileInfoes = new TileInfo[GridSize.x, GridSize.y];

            for (int i = 0; i < GridSize.x; i++)
            {
                for (int j = 0; j < GridSize.y; j++)
                {
                    tileInfoes[i, j] = new TileInfo(TileType.PATH, BuildingType.NONE, WallType.NONE, new Vector2Int(i, j), 1);
                }
            }
            // 원래는 현재 벽, 건물 데이터를 받아와서 그 위치에 따라 타일 정보를 갱신해준다.

            // 테스트용 데이터 로드
            testDataset.TestNotWalls();
            wallDatas = testDataset.LoadWallInfo();

            for (int i = 0; i < wallDatas.Count; i++)
            {
                int x = wallDatas[i].pos.x;
                int y = wallDatas[i].pos.y;

                tileInfoes[x, y].tileType = TileType.WALL;
                tileInfoes[x, y].wallType = wallDatas[i].type;
            }

            Debug.Log("DataCenter Init");
        }
    }

}

