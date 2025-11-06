using UnityEngine;

[CreateAssetMenu(fileName = "RestaurantData", menuName = "LUPData/Producing/RestrauntData")]
public class RestaurantStatus : ScriptableObject
{
    public string buildingName = "식당";
    public int level = 1;
    public int maxLevel = 30;

    public int foodStorage = 8; // 레벨1 기준
    public int waterStorage = 4;
    public float cookingSpeedPercent = 0.1f; // 10%
    public float eatingEfficiency = 0.02f; // 2%

    public int currentFood = 0;
    public int currentWater = 0;

    public int[] levelUpFoodStorage = { 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8 };
    public int[] levelUpWaterStorage = { 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4 };
}
