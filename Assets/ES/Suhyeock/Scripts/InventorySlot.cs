using NUnit.Framework.Interfaces;
using UnityEngine;

public class InventorySlot
{
    public Item item;

    public bool IsEmpty => item == null;
}
