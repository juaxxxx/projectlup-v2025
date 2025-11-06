using UnityEngine;

public class ProductableStateData
{
    public float elapsedTime;       // 누적 진행 시간
    public float totalTime;         // 하나 만드는데 걸리는 시간
    public float progressRatio;     // 진행률 (누적 진행 시간 / 총 건설 시간)
    public bool isCompledted;       // 완료 여부
    public bool isStarted;          // 생산 시작 여부
    public bool isActiveInteract;

    public ProductableStateData(float totalTime)
    {
        elapsedTime = 0f;
        this.totalTime = totalTime;
        progressRatio = 0f;
        isCompledted = false;
        isStarted = false;
        isActiveInteract = false;
    }

    public void Reset(float totalTime)
    {
        elapsedTime = 0f;
        this.totalTime = totalTime;
        progressRatio = 0f;
        isCompledted = false;
        isStarted = false;
        isActiveInteract = false;
    }

    public void SetProductionTime(float totalTime)
    {
        this.totalTime = totalTime;
    }


    public bool IsCompleted()
    {
        return isCompledted;
    }

    public bool IsStarted()
    {
        return isStarted;
    }


    public void Start()
    {
        isStarted = true;
    }

    public void Stop()
    {
        isStarted = false;
        isCompledted = false;
    }
}
