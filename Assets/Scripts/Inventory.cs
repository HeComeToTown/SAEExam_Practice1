using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    public List<Item> inventory = new List<Item>();
    public float MaximumWeight;
    public float CurrentWeight;

    public void InventoryChanged(Item itemToChange, bool add)
    {
        if (add)
        {
            inventory.Add(itemToChange);
            CurrentWeight += itemToChange.Properties.Weight;
        }
        else
        {
            inventory.Remove(itemToChange);
            CurrentWeight -= itemToChange.Properties.Weight;
        }
    }
}
