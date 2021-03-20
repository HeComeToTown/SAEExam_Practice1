using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/New Item")]
public class ItemProperties : ScriptableObject
{
    public string ItemName;
    public float Weight;
    public float Value;
    public string Description;
    public ItemCategory Category;
}

public enum ItemCategory
{
    Usable,
    Weapon,
    Armor,
    Misc
}