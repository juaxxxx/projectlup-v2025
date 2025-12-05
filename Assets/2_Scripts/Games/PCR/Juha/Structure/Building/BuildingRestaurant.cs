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
            // 현재는 건설 시작만 있다.
            Init();

            // 현재는 생산 데이터를 초기값으로 갱신한다.
            SetupRestaurantData();

            buildingEvents.OnBuildingSelected += OpenBuildingUI;
            buildingEvents.OnBuildingDeselected += CloseBuildingUI;
            // 메인 UI 중 비활성화 시켰던 기능 활성화 추가
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

        public override void Init()
        {
            ConstructScreen.SetActive(false);

            // 작업자 있는지 데이터 필요.
            hasWork = true;

            // 저장된 건물 정보랑 상태 가져오기

            // 지금은 테스트를 위해 그냥 건설 시작할 때만 구현
            ChangeState(new UnderConstructionState());
        }

        public override void InteractForTouch()
        {
            currBuildState?.Interact(this);
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

