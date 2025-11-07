using UnityEngine;

namespace ES
{
    public class Item
    {
        public BaseItemData baseItem;
        public int count;
    
        public Item(BaseItemData baseItem)
        {
            this.baseItem = baseItem;
        }
    }
}
