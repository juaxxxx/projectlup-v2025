using UnityEngine;

namespace LUP.PCR
{

    // 테스트용 사라질 예정
    public static class GridSize
    {
        public static int x = 21;
        public static int y = 20;

        public static float mapZPos = -2.5f;
        public static float tileSize = 5f;
    }

    public enum TileType
    {
        NONE,
        PATH,
        WALL,
        BUILDING,
        LADDER,
    }

    public enum BuildingType
    {
        NONE,
        CONTROLTOWER,
        POWERSTATION,
        RESTAURANT,
        LABORATORY,
        WATERTREATMENTPLANT,
        WORKSTATION,
        WHEATFARM,
        MUSHROOMFARM,
        STONEMINE,
        IRONMINE,
        COALMINE,
        MOLEFARM,
        DAIRYFARM,
        LADDER
    }

    public enum WallType
    {
        NONE,
        DUST,
        STONE
    }

    public enum ResourceType
    {
        None,
        Stone,
        Coal,
        Iron,
        Wheat,
        Mushroom,
        Meat,
        Food,
        Power,
        Diamond
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

    public enum FoodType
    {
        None,
        Bread,
        GrilledMushroom,
        MeatSoup,
    }

    public enum TaskType
    {
        BuildingWheatFarm,
        BuildingMushroomFarm,
    }

    public enum UIScreen
    {
        Main,
        Inventory,
        SelectConstrcut,
        ProductableBuilding,
        ConstructionDecision,
    }
}
