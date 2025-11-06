using UnityEngine;

public class ProductableState : IBuildState
{
    private ProductableStateData data;
    private ProductableBuilding productableBuilding;

    public void Enter(BuildingBase building)
    {
        Debug.Log("ProductableState Enter");

        productableBuilding = building as ProductableBuilding;

        if (productableBuilding)
        {
            float totalTime = productableBuilding.productableBuildingData.productionData[productableBuilding.level].productionTime;
            data = new ProductableStateData(totalTime);

            Start();
        }
    }
    public void Exit(BuildingBase building)
    {
        Debug.Log("ProductableState Exit");
        data.Reset(0);
    }
    public void Tick(BuildingBase building, float deltaTime)
    {
        if (!IsStarted())
        {
            return;
        }
        if (IsCompleted())
        {
            Debug.Log("ISComplete");
            return;
        }

        data.elapsedTime += deltaTime;
        data.progressRatio = Mathf.Clamp01(data.elapsedTime / data.totalTime);

        if (data.progressRatio >= 1f)
        {
            data.isCompledted = true;

            Complete();
        }
    }
    public void Interact(BuildingBase building)
    {

    }

    public void Complete()
    {
        productableBuilding.CompleteProduction();
    }

    public bool IsCompleted()
    {
        return data.isCompledted;
    }

    public bool IsStarted()
    {
        return data.isStarted;
    }


    public void Start()
    {
        float totalTime = productableBuilding.productableBuildingData.productionData[productableBuilding.level].productionTime;
        data.Reset(totalTime);
        data.isStarted = true;
        data.isCompledted = false;
    }

    public void Stop()
    {
        float totalTime = productableBuilding.productableBuildingData.productionData[productableBuilding.level].productionTime;
        data.Reset(totalTime);
    }
}
