using UnityEngine;

[System.Serializable]
public struct ConstructionData
{
    public float time;         // 건설 시간
    public CostEntry[] cost;     // 건설 비용
}
