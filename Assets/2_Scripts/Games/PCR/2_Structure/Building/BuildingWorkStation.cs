using UnityEngine;

namespace LUP.PCR
{
    public class BuildingWorkStation : BuildingBase
    {
        protected IBuildState constructState;
        protected IBuildState completeState;

        // 작업자 수용 수. 수용 수 그냥 없앨수도?
        public int maxStorage;
        public int currStorage;

        protected override void Awake()
        {
            buildingEvents = new BuildingEvents();
            constructState = new UnderConstructionState();
            completeState = new CompletedState();
        }

        protected override void Start()
        {
            base.Start();

            //buildingEvents.OnBuildingSelected += OpenBuildingUI;
            buildingEvents.OnBuildingDeselected += CloseBuildingUI;
        }


        private void Update()
        {
            if (!hasWork)
            {
                return;
            }

            float deltaTime = Time.deltaTime;
            currBuildState?.Tick(deltaTime);
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
            buildingName.Value = "WorkStation";
            placeName = buildingName.Value;


            ProductionStage stage = LUP.StageManager.Instance.GetCurrentStage() as ProductionStage;
            currentConstructionData = stage.GetCurrentConstructionData((int)BuildingType.WORKSTATION, buildingInfo.level);

            if (buildingInfo.isConstructing)
            {
                ChangeState(constructState);
            }
            else
            {
                ChangeState(completeState);
            }
        }

        public override void CompleteContruction()
        {            
            // 레벨업
            buildingInfo.level++;
            ProductionStage stage = LUP.StageManager.Instance.GetCurrentStage() as ProductionStage;
            currentConstructionData = stage.GetCurrentConstructionData((int)BuildingType.WORKSTATION, buildingInfo.level);


            ChangeState(completeState);
        }

        public override void Upgrade()
        {
            ChangeState(constructState);
        }

        public override void DeliverToInventory()
        {

        }

    }
}


