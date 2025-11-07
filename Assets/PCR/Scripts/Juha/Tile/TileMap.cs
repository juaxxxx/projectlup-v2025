using Unity.VisualScripting;
using UnityEditorInternal;
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

    public void InitializeTileMap()
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
                    tiles[i, j].SetTileInfo(new TileInfo(TileType.WALL, BuildingType.NONE, WallType.DUST, new Vector2Int(i, j), -1));
                }
            }
        }
    }

    public void UpdateAllDigWallPreview(DigWallPreview digWallPreview)
    {
        foreach (Tile tile in tiles)
        {
            if (tile.tileInfo.tileType == TileType.WALL)
            {
                digWallPreview.AddCanDigTile(tile);
            }
            else
            {
                digWallPreview.AddCanNotDigTile(tile);
            }
        }
    }

    // 나중에 데이터 가져올 때 호출할 함수
    void SetupTileMap(TileInfo[,] tileMap)
    {

    }

    // �׽�Ʈ��
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
