using UnityEngine;

[System.Serializable]
public struct BuildingData
{
    public BuildingType type;           // 건물 타입
    public Vector2Int size;             // 건물 크기(그리드)
    public int powerCost;               // 필요 에너지(전력)
}
