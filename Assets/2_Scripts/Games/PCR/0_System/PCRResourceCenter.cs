using UnityEngine;

namespace LUP.PCR
{

    public class PCRResourceCenter : MonoBehaviour
    {
        public int food;
        public int wheat;
        public int mushroom;
        public int meat;
        public int water;
        public int stone;
        public int iron;
        public int coal;
        public int power;
        public int diamond;

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
                case ResourceType.WHEAT:
                    wheat += amount;
                    break;
                case ResourceType.MUSHROOM:
                    mushroom += amount;
                    break;
                case ResourceType.MEAT:
                    meat += amount;
                    break;
                case ResourceType.FOOD:
                    food += amount;
                    break;
                case ResourceType.POWER:
                    power += amount;
                    break;
                case ResourceType.DIAMOND:
                    diamond += amount;
                    break;
            }
        }

        public void UseResource(ResourceType type, int amount)
        {
            switch (type)
            {
                case ResourceType.STONE:
                    stone -= amount;
                    break;
                case ResourceType.IRON:
                    iron -= amount;
                    break;
                case ResourceType.COAL:
                    coal -= amount;
                    break;
                case ResourceType.WHEAT:
                    wheat -= amount;
                    break;
                case ResourceType.MUSHROOM:
                    mushroom -= amount;
                    break;
                case ResourceType.MEAT:
                    meat -= amount;
                    break;
                case ResourceType.FOOD:
                    food -= amount;
                    break;
                case ResourceType.POWER:
                    power -= amount;
                    break;
                case ResourceType.DIAMOND:
                    diamond -= amount;
                    break;
            }
        }
    }
}