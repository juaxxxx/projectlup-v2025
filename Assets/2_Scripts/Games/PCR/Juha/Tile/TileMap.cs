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

        public void InitTileMap()
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
                        tiles[i, j].SetTileInfo(new TileInfo(TileType.PATH, BuildingType.NONE, WallType.NONE, new Vector2Int(i, j), 1));
                    }
                }
            }

            Debug.Log("TileMap Init");
        }

        public Tile GetTile(Vector2Int pos)
        {
            return tiles[pos.x, pos.y];
        }

        public void UpdateTilebyWall(WallType type, Vector2Int pos)
        {
            int x = pos.x;
            int y = pos.y;

            tiles[x, y].tileInfo.tileType = TileType.WALL;
            tiles[x, y].tileInfo.wallType = type;
        }

        public void UpdateTilebyBuilding(BuildingType type, Tile pivotTile)
        {
            Vector2Int placementSize = new Vector2Int(0, 0);

            switch (type)
            {
                case BuildingType.WHEATFARM:
                    placementSize = new Vector2Int(3, 1);
                    break;
                case BuildingType.MUSHROOMFARM:
                    placementSize = new Vector2Int(2, 1);
                    break;
                case BuildingType.MOLEFARM:
                    placementSize = new Vector2Int(3, 1);
                    break;
                case BuildingType.RESTAURANT:
                    placementSize = new Vector2Int(3, 1);
                    break;
                case BuildingType.POWERSTATION:
                    placementSize = new Vector2Int(2, 1);
                    break;
                case BuildingType.STONEMINE:
                case BuildingType.IRONMINE:
                case BuildingType.COALMINE:
                    placementSize = new Vector2Int(1, 1);
                    break;
                case BuildingType.WORKSTATION:
                    placementSize = new Vector2Int(2, 1);
                    break;
            }

            int x = pivotTile.tileInfo.pos.x;
            int y = pivotTile.tileInfo.pos.y;

            for (int i = 0; i < placementSize.x; i++)
            {
                for (int j = 0; j < placementSize.y; j++)
                {
                    int nx = x + i;
                    int ny = y + j;

                    tiles[nx, ny].tileInfo.tileType = TileType.BUILDING;
                    tiles[nx, ny].tileInfo.buildingType = type;
                }
            }
        }
    }


}
