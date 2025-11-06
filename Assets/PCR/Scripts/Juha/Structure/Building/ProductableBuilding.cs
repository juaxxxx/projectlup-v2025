using NUnit.Framework.Constraints;
using UnityEngine;
using UnityEngine.UIElements;

public abstract class ProductableBuilding : BuildingBase
{
    // 읽기전용 데이터
    public ProductableBuildingData productableBuildingData;

    protected IBuildState constructState;
    protected IBuildState productableState;

    public int currStorage;
    public int maxStorage;

    public abstract void SetupProductionData();

    public abstract void StartProduction();

    public abstract void StopProduction();

    public abstract void CompleteProduction();
}
