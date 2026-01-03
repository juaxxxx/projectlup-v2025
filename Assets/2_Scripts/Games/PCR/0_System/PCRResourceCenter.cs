using Unity.VisualScripting;
using UnityEngine;

namespace LUP.PCR
{

    public class PCRResourceCenter : MonoBehaviour
    {
        private Inventory inventory;

        public void InitInventory()
        {
            ProductionStage stage = StageManager.Instance.GetCurrentStage() as ProductionStage;
            inventory = stage.PCRInven;
        }

        public int GetResourceAmount(ResourceType type)
        {
            int index = 9900 + (int)type;
            return inventory.GetItemCount(index);
        }

        public void AddResource(ResourceType type, int amount)
        {
            string itemName = type.ToString();
            IItemable item = ItemManager.Instance.GetItem(itemName);
            if (item != null)
            {
                inventory.AddItem(item, amount);
                return;
            }
        }

        public void UseResource(ResourceType type, int amount)
        {
            string itemName = type.ToString();
            IItemable item = ItemManager.Instance.GetItem(itemName);
            if (item != null)
            {
                inventory.UseItem(item.ItemID, amount);
                return;
            }

            //switch (type)
            //{
            //    case ResourceType.Stone:

            //        break;
            //    case ResourceType.Iron:

            //        break;
            //    case ResourceType.Coal:

            //        break;
            //    case ResourceType.Wheat:

            //        break;
            //    case ResourceType.Mushroom:

            //        break;
            //    case ResourceType.Meat:

            //        break;
            //    case ResourceType.Food:

            //        break;
            //    case ResourceType.Power:

            //        break;
            //    case ResourceType.Diamond:

            //        break;
            //}
        }
    }
}