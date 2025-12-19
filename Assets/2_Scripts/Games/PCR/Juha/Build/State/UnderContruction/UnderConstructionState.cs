using UnityEngine;

namespace LUP.PCR
{
    public class UnderConstructionState : IBuildState
    {
        public float elapsedTime;       // 누적 진행 시간
        public float totalTime;         // 총 건설 시간
        public float progressRatio;     // 진행률 (누적 진행 시간 / 총 건설 시간)
        public bool isCompledted;       // 완료 여부
        public bool isStarted;          // 생산 시작 여부

        private BuildingBase building;
        private ConstructionInfo currentConstructionInfo;

        public void Enter(BuildingBase building)
        {
            Debug.Log("UnderContructionState Enter");

            // 건설중 UI 활성화
            if (building.ConstructScreen)
            {
                building.ConstructScreen.SetActive(true);
            }

            currentConstructionInfo = building.GetConstructionInfo();
            building.GetBuildingInfo().isConstructing = true;

            Start();
        }
        public void Exit(BuildingBase building)
        {
            if (building.ConstructScreen)
            {
                building.ConstructScreen.SetActive(false);
            }

            Stop();
            // 건설 취소.
            Debug.Log("UnderContructionState Exit");
        }
        public void Tick(BuildingBase building, float deltaTime)
        {
            if (!isStarted)
            {
                return;
            }
            if (isCompledted)
            {
                return;
            }

            elapsedTime += deltaTime;
            progressRatio = Mathf.Clamp01(elapsedTime / totalTime);

            if (progressRatio >= 1f)
            {
                isCompledted = true;
            }

            if (isCompledted)
            {
                building.CompleteContruction();
            }
        }

        public void Reset()
        {
            elapsedTime = currentConstructionInfo.elapsedTime;
            totalTime = building.currentConstructionData.constructionTime;
            progressRatio = 0f;
            isCompledted = false;
            isStarted = false;
        }

        public void Start()
        {
            Reset();
            isStarted = true;
            isCompledted = false;
        }

        public void Stop()
        {
            Reset();
        }

        public void Interact(BuildingBase building)
        {
            // 건설 진행도 UI 활성화
            Debug.Log("UnderContructionState Interact");
        }
    }
}