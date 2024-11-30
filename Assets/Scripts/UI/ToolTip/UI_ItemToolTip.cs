using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_ItemToolTip : UI_ToolTip
{
    [SerializeField] private TextMeshProUGUI itemNameText;
    [SerializeField] private TextMeshProUGUI itemTypeText;
    [SerializeField] private TextMeshProUGUI itemDescription;

    public void ShowToolTip(ItemData_Equipment item)
    {
        if (item == null)
        {
            return;
        }
        AdjustPosition();

        itemNameText.text = item.itemName;
        itemTypeText.text = item.itemType.ToString();
        itemDescription.text = item.GetDescription();

        itemNameText.fontSize = 30;

        if (itemNameText.text.Length > 12)
        {
            itemNameText.fontSize = itemNameText.fontSize * .8f;
        }

        gameObject.SetActive(true);
    }

    public void HideToolTip() => gameObject.SetActive(false);
}
