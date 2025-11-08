using UnityEngine;

public class TileMap : MonoBehaviour
{
    int gridWidth = 5;
    int gridHeight = 5;
    int tileMapWidth = 28;
    int tileMapHeight = 15;

    [SerializeField]
    private GameObject tilePrefab;

    [SerializeField]
    private GameObject dustPrefab;

    public Tile[,] tiles;

    // 이건 어디서 할지 고민좀 해보자.
    //tileMap.UpdateAllDigWallPreview(digWallPreview);
    //tileMap.GenerateObject();

    public void InitializeTileMap(TileInfo[,] tileInfoes)
    {
        tiles = new Tile[tileMapWidth, tileMapHeight];
        for (int i = 0; i< tileMapWidth; i++)
        {
            for (int j = 0; j < tileMapHeight; j++)
            {
                if (tilePrefab)
                {
                    GameObject tile = Instantiate(
                        tilePrefab, 
                        new Vector3(i * gridWidth + 2.5f, -j * gridHeight - 2.5f, -2.5f),
                        Quaternion.identity, this.transform);
                    tiles[i, j] = tile.GetComponent<Tile>();
                    tiles[i, j].SetTileInfo(tileInfoes[i,j]);
                }
            }
        }
    }

    public void GenerateObject()
    {
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                if (dustPrefab)
                {
                    if (tiles[i, j].tileInfo.tileType == TileType.WALL)
                    {
                        Instantiate(dustPrefab,
                        new Vector3(j * gridWidth + 2.5f, -i * gridHeight - 2.5f, -2.5f),
                        Quaternion.identity, this.transform);
                    }
                }
            }
        }
    }
}
