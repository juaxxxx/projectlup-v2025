using UnityEngine;

public class PlacementRules : MonoBehaviour
{
    protected TileMap tileMap;
    protected Vector2Int placementSize;

    public void Init(TileMap tileMap)
    {
        this.tileMap = tileMap;
    }

    public bool CheckSpaceAvailable(Tile pivotTile)
    {
        if (!tileMap)
        {
            Debug.Log("TileMap is empty!");
            return false;
        }

        int startGridX = pivotTile.tileInfo.pos.x;
        int startGridY = pivotTile.tileInfo.pos.y;

        if (startGridX + placementSize.x - 1 >= GridSize.x ||
            startGridY + placementSize.y - 1 >= GridSize.y)
        {
            return false;
        }

        for (int i = 0; i < placementSize.x; i++)
        {
            for (int j = 0; j < placementSize.y; j++)
            {
                int nextGridX = startGridX + i;
                int nextGridY = startGridY + j;

                Debug.Log("nextGridX: " + nextGridX + ", nextGridY: " + nextGridY);
                Debug.Log("type: " + tileMap.tiles[nextGridX, nextGridY].tileInfo.tileType);

                // 기획에 따라 통과 가능한 건물과 아닌 건물로 나뉠수도
                if (tileMap.tiles[nextGridX, nextGridY].tileInfo.tileType != TileType.NONE &&
                    tileMap.tiles[nextGridX, nextGridY].tileInfo.tileType != TileType.PATH)
                {
                    return false;
                }
            }
        }

        return true;
    }

    public bool CheckResourceAvailable()
    {
        // @TODO: 자원 구현 시 작업
        return true;
    }

    public void TransitionRules(BuildingType type)
    {
        switch(type)
        {
            case BuildingType.WHEATFARM:
                placementSize = new Vector2Int(4, 1);
                break;
            case BuildingType.MUSHROOMFARM:
                placementSize = new Vector2Int(2, 1);
                break;
            case BuildingType.MOLEFARM:
                placementSize = new Vector2Int(4, 1);
                break;
            case BuildingType.RESTAURANT:
                placementSize = new Vector2Int(3, 1);
                break;
        }
    }


}
