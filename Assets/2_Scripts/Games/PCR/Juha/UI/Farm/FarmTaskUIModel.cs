using UnityEngine;

namespace LUP.PCR
{
    public class FarmTaskUIModel
    {
        // Data(TextName, Resource, etc.) Update
        // UI Interact
        // Button Color, Active, etc.
        public FarmUIData uiData;

        public void UpdateData(ProductableBuilding building)
        {
            int level = building.GetBuildingInfo().level;
            uiData.SetData(level,
                building.buildingName,
                (int)building.currentProductionData.productionPerHour,
                building.GetProductionInfo().currentStorage,
                building.maxStorage,
                building.buildingStaticData.power);
        }
    }
}

