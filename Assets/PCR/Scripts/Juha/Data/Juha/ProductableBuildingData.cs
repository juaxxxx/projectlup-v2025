using UnityEngine;

[CreateAssetMenu(fileName = "ProductableBuildingData", menuName = "LUPData/ProductableBuildingData")]
public class ProductableBuildingData : ScriptableObject
{
    [Header("건물 정보")]
    public BuildingData buildingData;

    [Header("건설(업그레이드) 정보")]
    public ConstructionData[] constructionData;

    [Header("생산 정보")]
    public ResourceType resource;
    public ProductionData[] productionData;
}
