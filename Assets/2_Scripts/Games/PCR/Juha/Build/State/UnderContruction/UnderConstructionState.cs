using UnityEngine;

namespace LUP.PCR
{
    public class UnderConstructionState : IBuildState
    {
        private UnderContructionData constructionData;

        public void Enter(BuildingBase building)
        {
            Debug.Log("UnderContructionState Enter");

            // 건설중 UI 활성화
            if (building.ConstructScreen)
            {
                building.ConstructScreen.SetActive(true);
            }

            // 시간 등 데이터 변수 초기화
            constructionData.Reset(building.currentConstructionData.constructionTime);
        }
        public void Exit(BuildingBase building)
        {
            if (building.ConstructScreen)
            {
                building.ConstructScreen.SetActive(false);
            }
            // 건설 취소.
            Debug.Log("UnderContructionState Exit");
        }
        public void Tick(BuildingBase building, float deltaTime)
        {
            // 건설 시간 갱신
            constructionData.Tick(deltaTime);

            if (constructionData.isCompledted)
            {
                building.CompleteContruction();
            }
        }
        public void Interact(BuildingBase building)
        {
            // 건설 진행도 UI 활성화
            Debug.Log("UnderContructionState Interact");
        }
    }
}