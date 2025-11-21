using UnityEngine;

// 테스트용 사라질 예정
public static class GridSize
{
    public static int x = 10;
    public static int y = 10;
}

public enum TileType
{
    NONE,
    PATH,
    WALL,
    BUILDING,
}

public enum BuildingType
{
    NONE,
    MUSHROOMFARM,
    WHEATFARM,
    MOLEFARM,
    RESTAURANT
}

public enum WallType
{
    NONE,
    DUST,
    STONE
}

public enum ResourceType
{
    STONE,
    COAL,
    IRON,
    WHEAT,
    MUSHROOM,
}

public enum PlacementResultType
{
    SUCCESS,
    NOTENOUGHSPACE,
    LACKOFRESOURCE // 자원 종류별로 하나씩
}

public enum BuildState
{
    UNDERCONSTRUCTION,
    COMPLETED
}

public enum CropType
{
    NONE,
    WHEAT,
    POTATO
}

public enum FoodType
{
    NONE,
    BREAD,
    POTATOPIZZA
}

public enum TaskType
{
    BuildingWheatFarm,
    BuildingMushroomFarm,
}