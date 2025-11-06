using UnityEngine;

public struct FarmUIData
{
    public int level;
    public string buildingName;
    public int productionTime;
    public int curStorage;
    public int maxStorage;
    public int power;

    public void SetData(int level, string buildingName, int productionTime, int curStorage, int maxStorage, int power)
    {
        this.level = level;
        this.buildingName = buildingName;
        this.curStorage = curStorage;
        this.maxStorage = maxStorage;
        this.power = power;
    }
}