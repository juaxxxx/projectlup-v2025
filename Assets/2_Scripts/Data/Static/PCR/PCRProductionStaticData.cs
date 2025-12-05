[System.Serializable]
public class PCRProductionStaticData
{
    [Column("BuildingType")] public int BuildingType;
    [Column("Level")] public int Level;
    [Column("ProductionTime")] public int ProductionTime;
    [Column("StorageCapacity")] public int StorageCapacity;
}
