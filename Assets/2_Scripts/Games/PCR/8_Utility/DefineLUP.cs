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
        Idle,
        Dig,
        Construct,
        BuildingWheatFarm,
        BuildingMushroomFarm,
    }

    public enum UIScreen
    {
        Main,
        Inventory,
        SelectConstrcut,
        FarmTask,
        ConstructionDecision,
        DigWall,
    }

    public enum WorkerActionState
    {
        Idle = 0,       // 기본 대기

        // 작업 (10번대)
        Farming = 10,     // 농사
        Hammering = 11,   // 건설, 제작
        Researching = 12, // 연구

        // 생활 (20번대)
        Eating = 20,      // 식사
        //Sleeping = 21     // 수면
    }

}
