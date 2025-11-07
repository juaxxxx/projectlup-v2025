using NUnit.Framework.Interfaces;
using UnityEngine;

namespace ES
{
    public class InventorySlot
    {
        public Item item;

        public bool IsEmpty => item == null;
    }
}
