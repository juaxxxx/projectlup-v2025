using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

namespace LUP.PCR
{
    //public class BuildingCustom : ProductableBuilding
    //{
    //    private void Awake()
    //    {
    //        buildingEvents = new BuildingEvents();
    //        constructState = new UnderConstructionState();
    //        productableState = new ProductableState();
    //    }

    //    private void Start()
    //    {

    //        buildingEvents.OnBuildingSelected += OpenBuildingUI;
    //        buildingEvents.OnBuildingDeselected += CloseBuildingUI;
    //    }

    //    private void Update()
    //    {
    //        // 추후에 가속 아이템 적용 가능하게 만들어야 한다.
    //        float deltaTime = Time.deltaTime;
    //        currBuildState?.Tick(this, deltaTime);
    //    }

    //    public override void Init(ProductionRuntimeData runtimeData)
    //    {
    //        // 저장된 건물 정보랑 상태 가져오기
    //        SetupProductionData();

    //        // 지금은 테스트를 위해 그냥 건설 시작할 때만 구현
    //        ChangeState(constructState);
    //    }

    //    public override void CompleteContruction()
    //    {
    //        ChangeState(productableState);
    //    }
    //    public override void Upgrade()
    //    {
    //        ChangeState(productableState);
    //    }
    //    public override void InteractForTouch()
    //    {
    //        currBuildState?.Interact(this);
    //    }

    //    public override void SetupProductionData()
    //    {
    //        buildingName = "CustomBuilding";
            
    //        if (buildingInfo.level >= 0 && buildingInfo.level < productableBuildingData.constructionData.Length)
    //        {
    //            ProductionStage stage = LUP.StageManager.Instance.GetCurrentStage() as ProductionStage;

    //            currentConstructionData = stage.GetCurrentConstructionData((int)BuildingType.POWERSTATION, GetBuildingInfo().level);
    //            maxStorage = productableBuildingData.productionData[GetBuildingInfo().level].storageCapacity;
    //        }
    //    }

    //    public override void StartProduction()
    //    {
    //        ProductableState state = currBuildState as ProductableState;
    //        if (state != null)
    //        {
    //            state.Start();
    //        }
    //        else
    //        {
    //            Debug.Log("State is NOT Productable State");
    //        }
    //    }
    //    public override void StopProduction()
    //    {
    //        ProductableState state = currBuildState as ProductableState;
    //        if (state != null)
    //        {
    //            state.Stop();
    //        }
    //        else
    //        {
    //            Debug.Log("State is NOT Productable State");
    //        }
    //    }

    //    public override void CompleteProduction()
    //    {
    //        Debug.Log("CompleteProduction");
    //        productionInfo.currentStorage = productionInfo.currentStorage + 1 > maxStorage ? maxStorage : productionInfo.currentStorage + 1;
            
    //        if (productionInfo.currentStorage == maxStorage)
    //        {
    //            StopProduction();
    //        }
    //        else
    //        {
    //            StartProduction();
    //        }
    //    }

    //    public override void DeliverToInventory()
    //    {
    //        resourceCenter.AddResource(productableBuildingData.resource, productionInfo.currentStorage);
    //        productionInfo.currentStorage = 0;
    //    }
    //}
}
