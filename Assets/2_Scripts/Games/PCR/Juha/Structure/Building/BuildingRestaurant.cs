using UnityEngine;

namespace LUP.PCR
{
    public class BuildingRestaurant : BuildingBase
    {
        protected IBuildState constructState;
        protected IBuildState completeState;

        public FoodType currFood;

        public int maxStorage;
        public int currStorage;
        // 필요 자원 만들어야 한다.

        private void Awake()
        {
            buildingEvents = new BuildingEvents();
            constructState = new UnderConstructionState();
            completeState = new CompletedState();
        }

        private void Start()
        {

            buildingEvents.OnBuildingSelected += OpenBuildingUI;
            buildingEvents.OnBuildingDeselected += CloseBuildingUI;
        }

        private void Update()
        {
            if (!hasWork)
            {
                return;
            }

            // 추후에 가속 아이템 적용 가능하게 만들어야 한다.
            float deltaTime = Time.deltaTime;
            currBuildState?.Tick(this, deltaTime);
        }

        public override void Init(ProductionRuntimeData runtimeData)
        {
            this.runtimeData = runtimeData;


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

            // 작업자 있는지 데이터 필요.
            hasWork = true;
            buildingName = "Restaurant";

            ProductionStage stage = LUP.StageManager.Instance.GetCurrentStage() as ProductionStage;
            currentConstructionData = stage.GetCurrentConstructionData((int)BuildingType.RESTAURANT, buildingInfo.level);


            if (buildingInfo.isConstructing)
            {
                ChangeState(constructState);
            }
            else
            {
                ChangeState(completeState);
            }
        }

        public override void InteractForTouch()
        {
            currBuildState?.Interact(this);
        }

        public override void CompleteContruction()
        {
            // 레벨업
            buildingInfo.level++;
            ProductionStage stage = LUP.StageManager.Instance.GetCurrentStage() as ProductionStage;
            currentConstructionData = stage.GetCurrentConstructionData((int)BuildingType.WHEATFARM, buildingInfo.level);

            ChangeState(completeState);
        }

        public override void Upgrade()
        {
            ChangeState(constructState);
        }

        public override void DeliverToInventory()
        {

        }

        public void SetupRestaurantData()
        {
            // 지금은 초기값으로 초기화하는 작업으로 테스트
            // 미리 저장된 값 대신 임의의 값으로 대체
            // 다음에는 저장된 데이터를 받아와서 갱신해준다.
            currFood = FoodType.BREAD;
            maxStorage = 5;
            currStorage = 0;
            buildingName = "Restaurant";
        }

    }

}

