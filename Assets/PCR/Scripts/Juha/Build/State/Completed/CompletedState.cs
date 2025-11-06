using UnityEngine;

public class CompletedState : IBuildState
{
    private CompletedData completedData;

    public void Enter(BuildingBase building)
    {
        Debug.Log("CompletedState Enter");

        // 건설중 UI 활성화
        // 시간 등 데이터 변수 초기화

        //completedData.Reset(building.buildingData.buildTime);
    }
    public void Exit(BuildingBase building)
    {
        // 생산 진행 취소.
        Debug.Log("CompletedState Exit");
    }
    public void Tick(BuildingBase building, float deltaTime)
    {
        // 생산 중인 경우 시간 갱신
        completedData.Tick(deltaTime);

        if (completedData.IsCompleted())
        {
            CompleteTask(building);
        }
    }
    public void Interact(BuildingBase building)
    {
        // 건물 기능 UI 활성화
        // 생산 중인 경우 진행도 표시
        Debug.Log("CompletedState Interact");
        if (completedData.isActiveInteract)
        {
            building.buildingEvents?.OnBuildingDeselected.Invoke();
        }
        else
        {
            building.buildingEvents?.OnBuildingSelected.Invoke();
        }
        completedData.isActiveInteract = !completedData.isActiveInteract;
    }

    public void CompleteTask(BuildingBase building)
    {
        Debug.Log("생산 완료!");

    }

    public void StartTask(float TaskTime)
    {
        Debug.Log("생산 시작!");
        completedData.Reset(TaskTime);
        completedData.Start();
    }
    public void StopTask(BuildingBase building)
    {
        Debug.Log("생산 중지!");
        completedData.Stop();
    }
    public void ResumeTask(BuildingBase building)
    {
        completedData.Start();
    }

    public bool IsStarted()
    {
        return completedData.IsStarted();
    }
}
