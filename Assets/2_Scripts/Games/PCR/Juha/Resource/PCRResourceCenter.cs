using UnityEngine;

namespace LUP.PCR
{

    public class PCRResourceCenter : MonoBehaviour
    {
        // 식량
        public int food;

        // 식량 재료
        public int vegfruit;
        public int meat;
        public int water;

        // 재료
        public int stone;
        public int iron;
        public int coal;

        // 전력
        public int power;


        public void InitInventory()
        {
            
        }

        public void AddResource(ResourceType type, int amount)
        {
            switch(type)
            {
                case ResourceType.STONE:
                    stone += amount;
                    break;
                case ResourceType.IRON:
                    iron += amount;
                    break;
                case ResourceType.COAL:
                    coal += amount;
                    break;
                case ResourceType.VEGFRUIT:
                    vegfruit += amount;
                    break;
                case ResourceType.MEAT:
                    meat += amount;
                    break;
                case ResourceType.WATER:
                    water = amount;
                    break;
                case ResourceType.FOOD:
                    food += amount;
                    break;
                case ResourceType.POWER:
                    power += amount;
                    break;

            }
        }

        public void UseResource(ResourceType type, int amount)
        {

        }
    }
}