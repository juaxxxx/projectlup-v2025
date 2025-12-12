[System.Serializable]
public class PCRProductionStaticData
{
    [Column("BuildingType")] public int buildingType;
    [Column("Level")] public int level;
    [Column("ProductionPerHour")] public float productionPerHour;
    [Column("StorageCapacity")] public int StorageCapacity;
}
