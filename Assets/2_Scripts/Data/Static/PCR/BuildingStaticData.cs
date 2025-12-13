[System.Serializable]
public class BuildingStaticData
{
    [Column("buildingType")] public int buildingType;
    [Column("power")] public int power;
    [Column("x")] public int x;
    [Column("y")] public int y;
}
