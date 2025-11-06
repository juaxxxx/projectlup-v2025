using UnityEngine;

public class FarmTaskUIModel
{
    // Data(TextName, Resource, etc.) Update
    // UI Interact
    // Button Color, Active, etc.
    public FarmUIData uiData;

    public void UpdateData(ProductableBuilding building)
    {
        int level = building.level;
        uiData.SetData(level,
            building.buildingName, 
            building.productableBuildingData.productionData[level].productionTime,
            building.currStorage,
            building.maxStorage, 
            building.productableBuildingData.buildingData.powerCost);
    }
}