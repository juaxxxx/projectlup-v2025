using UnityEngine;

namespace LUP.PCR
{
    public struct InventoryInfo
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
    }

    public class ResourceCenter : MonoBehaviour
    {
        InventoryInfo inventory;



        public void InitInventory()
        {
            
        }
    }
}