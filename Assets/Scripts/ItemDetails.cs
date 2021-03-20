using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ItemDetails : MonoBehaviour
{
    [SerializeField] private TMP_Text ItemName;
    [SerializeField] private TMP_Text ItemWeight;
    [SerializeField] private TMP_Text ItemValue;
    [SerializeField] private TMP_Text ItemDescription;

    public void UpdateItemDetailsOverlay(string name, float weight, float value, string description, string Category)
    {
        ItemName.text = name + " (" + Category + ")";
        ItemWeight.text = "Weight: " + weight.ToString();
        ItemValue.text = "Value: " + value.ToString();
        ItemDescription.text = description;
    }
}
