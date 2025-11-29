using UnityEngine;

namespace LUP.PCR
{
    public class TileMap : MonoBehaviour
    {
        int gridWidth = 5;
        int gridHeight = 5;

        [SerializeField]
        private GameObject tilePrefab;

        public Tile[,] tiles;

        // 이건 어디서 할지 고민좀 해보자.
        //tileMap.UpdateAllDigWallPreview(digWallPreview);
        //tileMap.GenerateObject();

        public void InitializeTileMap(TileInfo[,] tileInfoes)
        {
            tiles = new Tile[GridSize.x, GridSize.y];
            for (int i = 0; i < GridSize.x; i++)
            {
                for (int j = 0; j < GridSize.y; j++)
                {
                    if (tilePrefab)
                    {
                        GameObject tile = Instantiate(
                            tilePrefab,
                            new Vector3(i * gridWidth + 2.5f, -j * gridHeight - 2.5f, -2.5f),
                            Quaternion.identity, this.transform);
                        tiles[i, j] = tile.GetComponent<Tile>();
                        tiles[i, j].SetTileInfo(tileInfoes[i, j]);
                    }
                }
            }

            Debug.Log("TileMap Init");

        }

        //public void GenerateObject()
        //{
        //    for (int i = 0; i < 10; i++)
        //    {
        //        for (int j = 0; j < 10; j++)
        //        {
        //            if (dustPrefab)
        //            {
        //                if (tiles[i, j].tileInfo.tileType == TileType.WALL)
        //                {
        //                    Instantiate(dustPrefab,
        //                    new Vector3(j * gridWidth + 2.5f, -i * gridHeight - 2.5f, -2.5f),
        //                    Quaternion.identity, this.transform);
        //                }
        //            }
        //        }
        //    }
        //}
    }


}
