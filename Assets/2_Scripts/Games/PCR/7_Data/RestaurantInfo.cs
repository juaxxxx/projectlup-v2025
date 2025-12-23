using UnityEngine;

namespace LUP.PCR
{
    [System.Serializable]
    public class RestaurantInfo
    {
        public FoodType currentFood;
        public float elapsedTime;
        public int totalCount;
        public bool isCooking;

        public RestaurantInfo(float elapsedTime, int totalCount, bool isCooking, FoodType currentFood)
        {
            this.elapsedTime = elapsedTime;
            this.totalCount = totalCount;
            this.isCooking = isCooking;
            this.currentFood = currentFood;
        }
    }
}

