using UnityEngine;

//public class BuildingRestaurant : BuildingBase
//{
//    // 식당 필요 데이터
//    // 하나의 식당에서 여러 음식들을 요리할 수 있게 기획 가정.
//    /*
//    선택된 음식 종류
//    걸리는 시간
//    최대 저장량 
//    현재 저장량 - 요리된 음식 - 음식은 자동으로 인벤토리에 저장된다고 하면 storage는 필요 없을지도?
//    필요 자원 - 한 번 요리하는데 필요한 자원
//    */
//    public FoodType currFood;
//    public float productionTime;
//    public int maxStorage;
//    public int currStorage;
//    // 필요 자원 만들어야 한다.

//    private void Awake()
//    {
//        buildingEvents = new BuildingEvents();
//    }

//    private void Start()
//    {
//        // 현재는 건설 시작만 있다.
//        Init();

//        // 현재는 생산 데이터를 초기값으로 갱신한다.
//        SetupRestaurantData();

//        buildingEvents.OnBuildingSelected += OpenBuildingUI;
//        // 메인 UI 중 비활성화 시키고 싶은 기능 비활성화 추가

//        buildingEvents.OnBuildingDeselected += CloseBuildingUI;
//        // 메인 UI 중 비활성화 시켰던 기능 활성화 추가
//    }

//    private void Update()
//    {
//        // 추후에 가속 아이템 적용 가능하게 만들어야 한다.
//        float deltaTime = Time.deltaTime;
//        currBuildState?.Tick(this, deltaTime);
//    }

//    public override void Init()
//    {
//        // 저장된 건물 정보랑 상태 가져오기

//        // 지금은 테스트를 위해 그냥 건설 시작할 때만 구현
//        ChangeState(new UnderConstructionState());
//    }

//    public override void InteractForTouch()
//    {
//        currBuildState?.Interact(this);
//    }

//    public void SetupRestaurantData()
//    {
//        // 지금은 초기값으로 초기화하는 작업으로 테스트
//        // 미리 저장된 값 대신 임의의 값으로 대체
//        // 다음에는 저장된 데이터를 받아와서 갱신해준다.
//        currFood = FoodType.BREAD;
//        productionTime = 5f;
//        maxStorage = 5;
//        currStorage = 0;
//    }

//    public void StartProduction()
//    {
//        CompletedState completedState = currBuildState as CompletedState;
//        completedState?.StartTask(this, productionTime);
//    }
//    public void StopProduction()
//    {
//        CompletedState completedState = currBuildState as CompletedState;
//        completedState?.StopTask(this);
//    }
//    public void CompleteProduction()
//    {
//        currStorage = currStorage + 1 > maxStorage ? maxStorage : currStorage + 1;

//        if (currStorage == maxStorage)
//        {
//            StopProduction();
//        }
//        else
//        {
//            StartProduction();
//        }
//    }

//    void OnGUI()
//    {
//        CompletedState completedState = currBuildState as CompletedState;
//        if (completedState != null)
//        {
//            GUILayout.BeginArea(new Rect(10, 10, 250, 500));
//            GUILayout.Label($"[식당]");
//            GUILayout.Label($"현재 음식: {currFood}");
//            GUILayout.Label($"생산 시간: {productionTime}");
//            GUILayout.Label($"저장량: {currStorage}");

//            if (GUILayout.Button("시작"))
//            {
//                StartProduction();
//            }

//            if (GUILayout.Button("중지"))
//            {
//                StopProduction();
//            }

//            GUILayout.EndArea();
//        }
//    }
//}
