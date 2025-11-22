namespace LUP.PCR
{
    public class RestaurantMemento
    {
        public int level;
        public int foodStorage;
        public int waterStorage;
        public float cookingSpeedPercent;
        public float eatingEfficiency;

        public RestaurantMemento(int level, int foodStorage, int waterStorage, float speed, float efficiency)
        {
            this.level = level;
            this.foodStorage = foodStorage;
            this.waterStorage = waterStorage;
            this.cookingSpeedPercent = speed;
            this.eatingEfficiency = efficiency;
        }
    }

}
