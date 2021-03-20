using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ItemDisplay : MonoBehaviour
{
    public Item MyItme;
    public ItemDetails DetailsDisplay;

    private TMP_Text _name;

    private void Awake()
    {
        _name = GetComponentInChildren<TMP_Text>();
    }

    private void Start()
    {
        _name.text = MyItme.Properties.ItemName;
    }

    public void OnClick()
    {
        DetailsDisplay.UpdateItemDetailsOverlay(MyItme.Properties.ItemName, MyItme.Properties.Weight, MyItme.Properties.Value, MyItme.Properties.Description, MyItme.Properties.Category.ToString());
    }
}
