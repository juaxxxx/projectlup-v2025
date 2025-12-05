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

        private void Awake()
        {
            buildingEvents = new BuildingEvents();
            constructState = new UnderConstructionState();
            completeState = new CompletedState();
        }

        private void Start()
        {
            // 현재는 생산 데이터를 초기값으로 갱신한다.
            // 현재는 건설 시작만 있다.
            Init();

            buildingEvents.OnBuildingSelected += OpenBuildingUI;
            buildingEvents.OnBuildingDeselected += CloseBuildingUI;
        }


        private void Update()
        {
            if (!hasWork)
            {
                return;
            }

            float deltaTime = Time.deltaTime;
            currBuildState?.Tick(this, deltaTime);
        }

        public override void Init()
        {
            ConstructScreen.SetActive(false);

            // 작업자 있는지 데이터 필요.
            hasWork = true;

            // 임시 건축 데이터 할당.
            currConstructionData = new ConstructionData();
            currConstructionData.time = 10;

            // 지금은 테스트를 위해 그냥 건설 시작할 때만 구현
            ChangeState(constructState);
        }

        public override void CompleteContruction()
        {
            ChangeState(completeState);
        }

        public override void Upgrade()
        {
            ChangeState(constructState);
        }

        public override void DeliverToInventory()
        {

        }

        public override void InteractForTouch()
        {

        }
    }
}


