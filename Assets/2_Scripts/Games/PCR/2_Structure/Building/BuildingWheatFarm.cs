using UnityEngine;

namespace LUP.PCR
{
    public class BuildingWheatFarm : ProductableBuilding
    {
        protected override void Awake()
        {
            buildingEvents = new BuildingEvents();
            constructState = new UnderConstructionState();
            productableState = new ProductableState();
        }

        protected override void Start()
        {
            base.Start();
            //buildingEvents.OnBuildingSelected += OpenBuildingUI;
            buildingEvents.OnBuildingDeselected += CloseBuildingUI;
        }

        private void Update()
        {
            float deltaTime = Time.deltaTime;

            // 공사 중(업그레이드 중)
            // 작업자 유무(hasWork)와 상관없이 무조건 시간을 흐르게 함
            if (buildingInfo.isConstructing)
            {
                currBuildState?.Tick(deltaTime);
                return; // 여기서 끝냄
            }

            // 생산 중
            // 작업자가 있을 때만(!hasWork 체크) 시간을 흐르게 함
            if (!hasWork)
            {
                return;
            }
            
            // 추후에 가속 아이템 적용 가능하게 만들어야 한다.
            currBuildState?.Tick(deltaTime);
        }

        public override void Init(ProductionRuntimeData runtimeData)
        {
            this.runtimeData = runtimeData;

            // Production
            productionInfo = runtimeData.GetProductionInfo(buildingInfo.buildingId);
            if (productionInfo == null)
            {
                ProductionInfo newProductionInfo = new ProductionInfo(buildingInfo.buildingId, 0f, 0);
                runtimeData.AddToList(runtimeData.ProductionInfoList, newProductionInfo);
                productionInfo = newProductionInfo;
            }

            // Constructing Building
            constructionInfo = runtimeData.GetConstructionInfo(buildingInfo.buildingId);
            if (constructionInfo == null)
            {
                ConstructionInfo newConstructionInfo = new ConstructionInfo(buildingInfo.buildingId, 0f);
                runtimeData.AddToList(runtimeData.ConstructionInfoList, newConstructionInfo);
                constructionInfo = newConstructionInfo;
            }


            if (ConstructScreen)
            {
                ConstructScreen.SetActive(false);
            }

            level.Value = buildingInfo.level;
            isConstructing.Value = buildingInfo.isConstructing;

            // 작업자 있는지 데이터 필요.
            hasWork = false;
            buildingName.Value = "WheatFarm";
            placeName = buildingName.Value;


            ProductionStage stage = LUP.StageManager.Instance.GetCurrentStage() as ProductionStage;
            currentConstructionData = stage.GetCurrentConstructionData((int)BuildingType.WHEATFARM, buildingInfo.level);
            currentProductionData = stage.GetCurrentProductionData((int)BuildingType.WHEATFARM, buildingInfo.level);
            maxStorage.Value = currentProductionData.StorageCapacity;
            productionPerHour.Value = currentProductionData.productionPerHour;
            
            if (buildingInfo.isConstructing)
            {
                ChangeState(constructState);
            }
            else
            {
                ChangeState(productableState);

                StartProduction();
            }
        }

        public override void CompleteContruction()
        {
            // 레벨업
            buildingInfo.level++;
            level.Value = buildingInfo.level;

            ProductionStage stage = LUP.StageManager.Instance.GetCurrentStage() as ProductionStage;
            currentConstructionData = stage.GetCurrentConstructionData((int)BuildingType.WHEATFARM, buildingInfo.level);
            currentProductionData = stage.GetCurrentProductionData((int)BuildingType.WHEATFARM, buildingInfo.level);
            maxStorage.Value = currentProductionData.StorageCapacity;
            productionPerHour.Value = currentProductionData.productionPerHour;


            ChangeState(productableState);
        }
        public override void Upgrade()
        {
            ChangeState(constructState);
        }

        public override void SetupProductionData()
        {

        }

        public override void StartProduction()
        {
            ProductableState state = currBuildState as ProductableState;
            if (state != null)
            {
                state.Start();
            }
            else
            {
                Debug.Log("State is NOT Productable State");
            }
        }
        public override void StopProduction()
        {
            ProductableState state = currBuildState as ProductableState;
            if (state != null)
            {
                state.Stop();
            }
            else
            {
                Debug.Log("State is NOT Productable State");
            }
        }

        public override void CompleteProduction()
        {
            Debug.Log("CompleteProduction");
            productionInfo.currentStorage = productionInfo.currentStorage + 1 > maxStorage.Value ? maxStorage.Value : productionInfo.currentStorage + 1;
            currentStorage.Value = productionInfo.currentStorage;

            if (productionInfo.currentStorage == maxStorage.Value)
            {
                DeliverToInventory();
                StartProduction();
                //StopProduction();
            }
            else
            {
                StartProduction();
            }
        }

        public override void DeliverToInventory()
        {
            resourceCenter.AddResource(ResourceType.Wheat, productionInfo.currentStorage);
            productionInfo.currentStorage = 0;
            currentStorage.Value = productionInfo.currentStorage;
        }
       
    
    }

}
