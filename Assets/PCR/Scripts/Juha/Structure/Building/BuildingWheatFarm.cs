using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class BuildingWheatFarm : ProductableBuilding
{

    private void Awake()
    {
        buildingEvents = new BuildingEvents();
        constructState = new UnderConstructionState();
        productableState = new ProductableState();
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
        // 추후에 가속 아이템 적용 가능하게 만들어야 한다.
        float deltaTime = Time.deltaTime;
        currBuildState?.Tick(this, deltaTime);
    }

    public override void Init()
    {
        // 저장된 건물 정보랑 상태 가져오기
        SetupProductionData();

        // 지금은 테스트를 위해 그냥 건설 시작할 때만 구현
        ChangeState(constructState);
    }

    public override void CompleteContruction()
    {
        ChangeState(productableState);
    }
    public override void Upgrade()
    {
        ChangeState(productableState);
    }
    public override void InteractForTouch()
    {
        currBuildState?.Interact(this);
    }

    public override void SetupProductionData()
    {
        // 지금은 초기값으로 초기화하는 작업으로 테스트
        // 미리 저장된 값 대신 임의의 값으로 대체
        // 다음에는 저장된 데이터를 받아와서 갱신해준다.
        level = 0;
        currStorage = 0;
        buildingName = "밀 농장";

        if (level >= 0 && level < productableBuildingData.constructionData.Length)
        {
            currConstructionData = productableBuildingData.constructionData[level];
            maxStorage = productableBuildingData.productionData[level].storageCapacity;
        }
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
        currStorage = currStorage + 1 > maxStorage ? maxStorage : currStorage + 1;

        if (currStorage == maxStorage)
        {
            StopProduction();
        }
        else
        {
            StartProduction();
        }
    }

}
